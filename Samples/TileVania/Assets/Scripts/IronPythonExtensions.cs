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
using IronPython.Runtime.Types;
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
		/// Break sourcecode into tokens (with useful information)
		/// </summary>
		/// <param name="eng"></param>
		/// <param name="sourceCode"></param>
		/// <param name="suiteStatement"></param>
		/// <returns></returns>
		public static List<TokenWithSpan> Tokenize(this ScriptEngine eng, string sourceCode,
				SuiteStatement suiteStatement)
		{
			var tokenizer = eng.GetTokenizer(sourceCode);
			var context = (PythonContext)eng.GetLanguageContext();
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

					CheckAdvancedTokenKind(currentToken, suiteStatement, context);
					tokens.Add(currentToken);
					currentToken = tokenizer.NextToken();
				}
			}
			catch (SyntaxErrorException ex)
			{
				tokens.Add(ex.ToToken(tokenizer));
			}

			//fix parsing comment token
			var lines = tokenizer.GetLineLocations().OrderBy(l => l).ToList();
			foreach (var commentSymbolIndex in sourceCode.IndexesOf('#'))
			{
				var nextLineIndex = lines.FirstOrDefault(l => commentSymbolIndex < l);
				var comment = nextLineIndex == 0
						? sourceCode.Substring(commentSymbolIndex)
						: sourceCode.Substring(commentSymbolIndex, nextLineIndex - commentSymbolIndex - 1);
				var location = tokenizer.IndexToLocation(commentSymbolIndex);
				var span = new IndexSpan(commentSymbolIndex, comment.Length);
				var token = new CommentToken(comment);
				tokens.Add(new TokenWithSpan(token, span, location));
			}

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

		public static Dictionary<string, PythonVariable> GetVariables(this ScopeStatement scopeStatement)
		{
			var variables = (Dictionary<string, PythonVariable>)scopeStatement
					.GetType()
					.GetProperty("Variables", BindingFlags.Instance | BindingFlags.NonPublic)?
					.GetValue(scopeStatement, null);
			return variables;
		}

		private static TokenWithSpan CheckAdvancedTokenKind(TokenWithSpan token, SuiteStatement suiteStatement,
				PythonContext context)
		{
			switch (token.Token.Kind)
			{
				case TokenKind.Constant:
					if (token.Token.Image.Length == 0)
						return token;
					// var firstChar = token.Token.Image[0].ToString();
					// var lastChar = token.Token.Image[token.Token.Image.Length - 1].ToString();
					// if (!int.TryParse(firstChar, out _) || !int.TryParse(lastChar, out _))
					//     return token;
					// var image = token.Token.Image.Replace("_", "");
					// token.SetContext(double.TryParse(image, out _)
					//     ? AdvancedTokenKind.Number
					//     : AdvancedTokenKind.Unspecified);
					var isNumber =
							token.Token.Value is int or float or double or long or short or uint or ulong or ushort;
					token.SetContext(isNumber
							? AdvancedTokenKind.Number
							: AdvancedTokenKind.Unspecified);
					break;
				case TokenKind.Name:
					var scope = suiteStatement.TryGetParentScope(token);
					switch (scope.NodeName)
					{
						// parentScope is null, if token is global variable/function/import/class
						// parentScope is not null, if token is argument or local variable/function/import/class
						case nameof(PythonAst):
							{
								var advancedKind = scope.IsFunction(token) ? AdvancedTokenKind.Function :
										scope.IsImport(token) ? AdvancedTokenKind.Module :
										scope.IsClass(token) ? AdvancedTokenKind.Class :
										context.IsBuildInFunction(token) ? AdvancedTokenKind.BuildInFunction :
										context.IsBuildIn(token) ? AdvancedTokenKind.BuildIn : AdvancedTokenKind.Variable;
								token.SetContext(advancedKind, scope);
								break;
							}
						case nameof(FunctionDefinition):
							{
								var advancedKind = scope.IsFunction(token) || suiteStatement.Parent.IsFunction(token)
										? AdvancedTokenKind.Function
										: scope.IsImport(token) || suiteStatement.Parent.IsImport(token)
												? AdvancedTokenKind.Module
												: scope.IsClass(token) || suiteStatement.Parent.IsClass(token)
														? AdvancedTokenKind.Class
														: scope.IsParameter(token)
																? AdvancedTokenKind.Parameter
																: context.IsBuildInFunction(token)
																		? AdvancedTokenKind.BuildInFunction
																		: context.IsBuildIn(token)
																				? AdvancedTokenKind.BuildIn
																				: AdvancedTokenKind.Variable;
								token.SetContext(advancedKind, scope);
								break;
							}
						case nameof(ClassDefinition):
							{
								var advancedKind = scope.IsFunction(token) || suiteStatement.Parent.IsFunction(token)
										? AdvancedTokenKind.Function
										: scope.IsImport(token) || suiteStatement.Parent.IsImport(token)
												? AdvancedTokenKind.Module
												: scope.IsClass(token) || suiteStatement.Parent.IsClass(token)
														? AdvancedTokenKind.Class
														: context.IsBuildInFunction(token)
																? AdvancedTokenKind.BuildInFunction
																: context.IsBuildIn(token)
																		? AdvancedTokenKind.BuildIn
																		: AdvancedTokenKind.Variable;
								token.SetContext(advancedKind, scope);
								break;
							}
					}

					break;
			}

			return token;
		}

		private static SuiteStatement GetSuite(this ScopeStatement nestedScope)
		{
			return nestedScope?.NodeName switch
			{
				nameof(FunctionDefinition) => (SuiteStatement)((FunctionDefinition)nestedScope).Body,
				nameof(ClassDefinition) => (SuiteStatement)((ClassDefinition)nestedScope).Body,
				nameof(PythonAst) => (SuiteStatement)((PythonAst)nestedScope).Body,
				_ => null
			};
		}

		private static bool IsFunction(this ScopeStatement scope, TokenWithSpan token)
		{
			var suite = scope.GetSuite();
			return (scope.Name == token.Token.Image && scope.NodeName == nameof(FunctionDefinition)) ||
						 suite.Statements.Any(s => s is FunctionDefinition f && f.Name == token.Token.Image);
		}

		private static bool IsImport(this ScopeStatement scope, TokenWithSpan token)
		{
			var suite = scope.GetSuite();
			return suite.Statements.Any(s =>
					s is ImportStatement i && i.Names.SelectMany(n => n.Names).Contains(token.Token.Image));
		}

		private static bool IsClass(this ScopeStatement scope, TokenWithSpan token)
		{
			var suite = scope.GetSuite();
			return (scope.Name == token.Token.Image && scope.NodeName == nameof(ClassDefinition)) ||
						 suite.Statements.Any(s => s is ClassDefinition c && c.Name == token.Token.Image);
		}

		private static bool IsParameter(this ScopeStatement scope, TokenWithSpan token)
		{
			return scope is FunctionDefinition function && function.Parameters.Any(p => p.Name == token.Token.Image);
		}

		private static bool IsBuildInFunction(this PythonContext context, TokenWithSpan token)
		{
			return context.BuiltinModuleDict.Any(item =>
					(string)item.Key == token.Token.Image && item.Value is BuiltinFunction);
		}

		private static bool IsBuildIn(this PythonContext context, TokenWithSpan token)
		{
			return context.BuiltinModuleDict.Any(item =>
					(string)item.Key == token.Token.Image);
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

		private static ScopeStatement TryGetParentScope(this SuiteStatement suiteStatement, TokenWithSpan token)
		{
			var parentScope = suiteStatement.Parent;
			var currentSuit = suiteStatement;
			var nestedScope = suiteStatement.Statements.FirstOrDefault(statement =>
					statement.StartIndex <= token.Location.Index && statement.EndIndex >= token.Location.Index);
			do
			{
				switch (nestedScope?.NodeName)
				{
					case nameof(FunctionDefinition):
						parentScope = (FunctionDefinition)nestedScope;
						currentSuit = (SuiteStatement)((FunctionDefinition)nestedScope).Body;
						break;
					case nameof(ClassDefinition):
						parentScope = (ClassDefinition)nestedScope;
						currentSuit = (SuiteStatement)((ClassDefinition)nestedScope).Body;
						break;
				}

				nestedScope = currentSuit.Statements.FirstOrDefault(statement =>
						statement.StartIndex <= token.Location.Index && statement.EndIndex >= token.Location.Index);
			} while (nestedScope?.NodeName == nameof(FunctionDefinition));

			return parentScope;
		}
	}
}