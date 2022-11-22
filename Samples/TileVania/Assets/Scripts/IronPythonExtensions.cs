using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Compiler.Ast;
using Microsoft.Scripting;
using IronPython.Compiler;
using TokenKind = IronPython.Compiler.TokenKind;

namespace IronPython.Custom
{
    public static class IronPythonExtensions
    {
        /// <summary>
        /// Generate Python Abstract Syntax Tree (AST), a data structure, which is generated by IronPython's parser. 
        /// Generate AST is the first step of compiling process, before the compiler produce <see cref="ScriptCode"/>, a machine code or Intermediate Language (IL).
        /// <br/>
        /// https://learn.microsoft.com/en-us/archive/msdn-magazine/2007/october/clr-inside-out-ironpython-and-the-dynamic-language-runtime
        /// </summary>
        /// <param name="eng">Python engine</param>
        /// <param name="sourceCode">source code</param>
        /// <returns></returns>
        /// <remarks>
        /// IronPython code first generates an AST particular to IronPython, and then it maps this tree to a DLR AST. Some languages have their own intermediate tree so that they can perform analysis or support tools (code editors) over their ASTs. Tools need a tree that is close to source code that users have typed in an editor. Many languages may have ASTs that will be similar to the DLR's, but the DLR ASTs will have more information about semantics explicitly represented. You might think of the DLR AST as one of the various transformations that later stages of a compiler need.
        /// </remarks>
        public static PythonAst GenerateAst(this ScriptEngine eng, string sourceCode)
        {
            var languageContext = eng.TryGetLanguageContext();
            var compilerContext = languageContext.GetCompilerContext(sourceCode);
            var ast = languageContext.ParseAndBindAst(compilerContext);
            return ast;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eng"></param>
        /// <param name="sourceCode"></param>
        /// <returns></returns>
        public static List<TokenWithSpan> Tokenize(this ScriptEngine eng, string sourceCode)
        {
            var tokenizer = eng.GetTokenizer(sourceCode);
            var tokens = new List<TokenWithSpan>();
            try
            {
                var currentToken = tokenizer.NextToken();
                while (true)
                {
                    if (currentToken.Token.Kind == TokenKind.EndOfFile)
                        break;
                    if (currentToken.Token.Kind is TokenKind.NewLine or TokenKind.NLToken)
                    {
                        currentToken = tokenizer.NextToken();
                        continue;
                    }

                    tokens.Add(currentToken);
                    currentToken = tokenizer.NextToken();
                }
            }
            catch (SyntaxErrorException ex)
            {
                tokens.Add(ex.ToToken(tokenizer));
            }

            // TODO: fix parsing comment token
            foreach (var commentIndex in sourceCode.IndexesOf('#'))
            {
                var location = tokenizer.IndexToLocation(commentIndex);
            }
            
            // TODO: implement logic for AdvancedTokenKind

            return tokens;
        }

        public static TokenWithSpan ToToken(this SyntaxErrorException ex, Tokenizer tokenizer)
        {
            var span = new IndexSpan(ex.RawSpan.Start.Index, ex.RawSpan.Length);
            var value = ex.SourceCode.Substring(ex.RawSpan.Start.Index, ex.RawSpan.Length);
            var message = $"Error[{ex.ErrorCode}] Start[{ex.RawSpan.Start.Line}:{ex.RawSpan.Start.Column}] " +
                          $"- End[{ex.RawSpan.End.Line}:{ex.RawSpan.End.Column}] " +
                          $"Value[{value}] Message[{ex.Message}]";
            var token = new ErrorToken(message);
            return new TokenWithSpan(token, span, tokenizer.IndexToLocation(span.Start));
        }

        public static TokenWithSpan NextToken(this Tokenizer tokenizer)
        {
            var token = tokenizer.GetNextToken();
            var span = tokenizer.TokenSpan;
            var location = tokenizer.IndexToLocation(span.Start);
            return new TokenWithSpan(token, span, location);
        }

        public static Tokenizer GetTokenizer(this ScriptEngine eng, string sourceCode)
        {
            var languageContext = eng.TryGetLanguageContext();
            var compilerContext = languageContext.GetCompilerContext(sourceCode);
            var compilerOptions = compilerContext.Options as PythonCompilerOptions;

            SourceCodeReader reader;
            try
            {
                reader = compilerContext.SourceUnit.GetReader();

                if (compilerOptions is { SkipFirstLine: true })
                {
                    reader.ReadLine();
                }
            }
            catch (IOException e)
            {
                compilerContext.Errors.Add(compilerContext.SourceUnit, e.Message, SourceSpan.Invalid, 0,
                    Severity.Error);
                throw;
            }

            var tokenizer = new Tokenizer(compilerContext.Errors, compilerOptions);
            tokenizer.Initialize(null, reader, compilerContext.SourceUnit, SourceLocation.MinValue);
            return tokenizer;
        }

        public static CompilerContext GetCompilerContext(this LanguageContext languageContext, string sourceCode)
        {
            var sourceUnit = languageContext.CreateSourceUnit(new SourceStringContentProvider(sourceCode), null,
                SourceCodeKind.Statements);
            return new CompilerContext(sourceUnit,
                (PythonCompilerOptions)languageContext.GetCompilerOptions(),
                ThrowingErrorSink.Default);
        }

        public static LanguageContext TryGetLanguageContext(this ScriptEngine eng)
            => (LanguageContext)eng
                   .GetType()
                   .GetProperty("LanguageContext", BindingFlags.Instance | BindingFlags.NonPublic)?
                   .GetValue(eng, null)
               ?? eng.GetLanguageContext();

        /// <summary>
        /// Try to acquire <see cref="LanguageContext"/> from <see cref="ScriptEngine"/>
        /// </summary>
        /// <param name="eng"></param>
        /// <returns></returns>
        public static LanguageContext GetLanguageContext(this ScriptEngine eng)
        {
            var runtime = eng.Runtime;
            var manager = (ScriptDomainManager)runtime
                              .GetType()
                              .GetProperty("Manager", BindingFlags.Instance | BindingFlags.NonPublic)?
                              .GetValue(runtime, null)
                          ?? runtime.GetScriptDomainManager();
            return manager.GetLanguageByTypeName(typeof(PythonContext).AssemblyQualifiedName);
        }

        /// <summary>
        /// Generate <see cref="ScriptDomainManager"/> the same way as <see cref="ScriptRuntime"/>'s constructor do
        /// </summary>
        /// <param name="runtime"></param>
        /// <returns></returns>
        public static ScriptDomainManager GetScriptDomainManager(this ScriptRuntime runtime)
        {
            var config = (DlrConfiguration)runtime.Setup
                .GetType()
                .GetMethod("ToConfiguration", BindingFlags.Instance | BindingFlags.NonPublic)?
                .Invoke(runtime, Array.Empty<object>());

            var hostProxy = new ScriptHostProxy(runtime.Host);
            var manager = new ScriptDomainManager(hostProxy, config);
            return manager;
        }

        /// <summary>
        /// Explicitly add modules into <see cref="ScriptRuntime"/>
        /// explicitly create/setup <see cref="ScriptScope"/>,
        /// and generate <see cref="ScriptSource"/>
        /// </summary>
        /// <param name="eng"></param>
        /// <param name="sourceCode"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static (ScriptSource, ScriptScope) Prepare(this ScriptEngine eng, string sourceCode, List args = null)
        {
            var runtime = eng.Runtime;
            //runtime.LoadAssembly(typeof(String).Assembly);

            var scope = eng.GetSysModule();
            //scope.SetVariable("__name__", "__main__");

            // setup command-line argument
            if (args != null)
            {
                var argList = new List();
                argList.extend(args);
                scope.SetVariable("argv", argList);
            }

            // setup search paths
            var path = Assembly.GetExecutingAssembly().Location;
            var rootDir = Directory.GetParent(path)?.FullName;
            List<string> paths = new() { rootDir };
            var env = Environment.GetEnvironmentVariable("IRONPYTHONPATH");
            if (!string.IsNullOrWhiteSpace(env))
            {
                var items = env.Split(';');
                paths.AddRange(items.Where(item => !string.IsNullOrWhiteSpace(item)));
            }

            //scope.SetVariable("path", paths);
            eng.SetSearchPaths(paths);

            var source = eng.CreateScriptSourceFromString(sourceCode, SourceCodeKind.Statements);
            return (source, scope);
        }

        private static PythonAst ParseAndBindAst(this LanguageContext languageContext, CompilerContext context)
        {
            var properties = ScriptCodeParseResult.Complete;
            var propertiesSet = false;
            var errorCode = 0;

            PythonAst ast;
            var pythonOptions = (PythonOptions)languageContext.Options;
            using (var parser = Parser.CreateParser(context, pythonOptions))
            {
                switch (context.SourceUnit.Kind)
                {
                    case SourceCodeKind.InteractiveCode:
                        ast = parser.ParseInteractiveCode(out properties);
                        propertiesSet = true;
                        break;

                    case SourceCodeKind.Expression:
                        ast = parser.ParseTopExpression();
                        break;

                    case SourceCodeKind.SingleStatement:
                        ast = parser.ParseSingleStatement();
                        break;

                    case SourceCodeKind.File:
                        ast = parser.ParseFile(true, false);
                        break;

                    case SourceCodeKind.Statements:
                        ast = parser.ParseFile(false, false);
                        break;

                    case SourceCodeKind.Unspecified:
                    default:
                    case SourceCodeKind.AutoDetect:
                        ast = parser.ParseFile(true, true);
                        break;
                }

                errorCode = parser.ErrorCode;
            }

            if (!propertiesSet && errorCode != 0)
            {
                properties = ScriptCodeParseResult.Invalid;
            }

            context.SourceUnit.CodeProperties = properties;

            if (errorCode != 0 || properties == ScriptCodeParseResult.Empty)
            {
                return null;
            }

            ast.Bind();
            return ast;
        }
    }
}