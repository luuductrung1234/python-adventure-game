using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using TokenKind = IronPython.Compiler.TokenKind;

namespace IronPython.Custom
{
    public class MyPython
    {
        public MyPython(Action<object> logging)
        {
            Engine = Python.CreateEngine(new Dictionary<string, object>() { { "Debug", false } });
            Logging = logging;
        }

        public ScriptEngine Engine { get; }
        public Action<object> Logging { get; }
        public SuiteStatement Body { get; private set; }
        public List<TokenWithSpan> Tokens { get; private set; }
        public string CachedSourceCode { get; private set; }

        public void SampleUsage(string sourceCode)
        {
            AnalyzeScript(sourceCode);
            RunScript();
        }

        public void AnalyzeScript(string sourceCode)
        {
            Logging($"Analyze {sourceCode}");
            CachedSourceCode = sourceCode;

            TokenWithSpan errorToken = null;
            try
            {
                var ast = Engine.GenerateAst(sourceCode);
                if (ast == null)
                    throw new InvalidOperationException("Fail to generate IronPython Abstract Syntax Tree (AST)");
                Body = (SuiteStatement)ast.Body;
            }
            catch (SyntaxErrorException ex)
            {
                var value = ex.SourceCode.Substring(ex.RawSpan.Start.Index, ex.RawSpan.Length);
                var message = $"Error[{ex.ErrorCode}] Start[{ex.RawSpan.Start.Line}:{ex.RawSpan.Start.Column}] " +
                              $"- End[{ex.RawSpan.End.Line}:{ex.RawSpan.End.Column}] " +
                              $"Value[{value}] Message[{ex.Message}]";
                Logging(message);
                errorToken = ex.ToToken(Engine.GetTokenizer(CachedSourceCode));
            }

            Tokens = Engine.Tokenize(sourceCode, Body);
            if (Tokens == null)
                throw new InvalidOperationException("Fail to tokenize source code");

            if (errorToken == null) return;
            var tokenNotInErrorRange = Tokens.Where(token =>
                (errorToken.Span.Start > token.Span.Start || errorToken.Span.End < token.Span.Start) &&
                (errorToken.Span.Start > token.Span.End || errorToken.Span.End < token.Span.End)).ToList();
            tokenNotInErrorRange.Add(errorToken);
            Tokens = tokenNotInErrorRange;
        }

        public ScriptScope RunScript()
        {
            var (source, scope) = Engine.Prepare(CachedSourceCode);

            try
            {
                var compiledCode = source.Compile();
                compiledCode.Execute(scope);
            }
            catch (Exception ex)
            {
                var eo = Engine.GetService<ExceptionOperations>();
                Logging($"{eo.FormatException(ex)}");
            }

            TryCallScriptMethods(scope);
            return scope;
        }

        /// <summary>
        /// Try to get Python methods from executed script 
        /// (script must be executed before calling this method)
        /// </summary>
        /// <param name="scope"></param>
        private void TryCallScriptMethods(ScriptScope scope)
        {
            foreach (var item in scope.GetItems())
            {
                Logging($"Found item (in scope): {item.Key}");
            }

            var isExist = scope.TryGetVariable("greetings", out var greetings);
            if (isExist)
            {
                Logging(greetings(""));
                Logging(greetings("World"));
            }

            isExist = scope.TryGetVariable("get_sys_info_list", out var showSysInfoList);
            if (isExist)
            {
                var infoList = (List)showSysInfoList();
                Logging($"{infoList.GetType()}");
                foreach (var info in infoList)
                {
                    Logging(info);
                }
            }

            isExist = scope.TryGetVariable("get_sys_info_dict", out var showSysInfoDict);
            if (!isExist) return;
            var infoDict = (PythonDictionary)showSysInfoDict();
            foreach (var entry in infoDict)
            {
                Logging($"{entry.Key}: {entry.Value}");
            }
        }
    }
}