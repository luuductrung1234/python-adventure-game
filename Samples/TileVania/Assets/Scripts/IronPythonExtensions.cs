using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Compiler.Ast;
using Microsoft.Scripting;
using IronPython.Compiler;

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
		/// <param name="eng"></param>
		/// <param name="sourceCode"></param>
		/// <returns></returns>
		/// <remarks>
		/// IronPython code first generates an AST particular to IronPython, and then it maps this tree to a DLR AST. Some languages have their own intermediate tree so that they can perform analysis or support tools (code editors) over their ASTs. Tools need a tree that is close to source code that users have typed in an editor. Many languages may have ASTs that will be similar to the DLR's, but the DLR ASTs will have more information about semantics explicitly represented. You might think of the DLR AST as one of the various transformations that later stages of a compiler need.
		/// </remarks>
		public static PythonAst GenerateAst(this ScriptEngine eng, string sourceCode)
		{
			var context = (LanguageContext)eng
					.GetType()
					.GetProperty("LanguageContext", BindingFlags.Instance | BindingFlags.NonPublic)
					.GetValue(eng, null)
				?? eng.GetLanguageContext();

			Debug.Log($"LanguageContext ID:{context.ContextId}");
			Debug.Log($"LanguageContext Language:{context.LanguageGuid}");
			Debug.Log($"LanguageContext Version:{context.LanguageVersion}");

			var sourceUnit = context.CreateSourceUnit(new SourceStringContentProvider(sourceCode), null, Microsoft.Scripting.SourceCodeKind.Statements);
			var errorSink = ThrowingErrorSink.Default;
			var pythonOptions = (PythonCompilerOptions)context.GetCompilerOptions();
			var compilerContext = new CompilerContext(sourceUnit, pythonOptions, errorSink);

			PythonAst ast = context.ParseAndBindAst(compilerContext);
			return ast;
		}

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
					.GetProperty("Manager", BindingFlags.Instance | BindingFlags.NonPublic)
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
				.GetMethod("ToConfiguration", BindingFlags.Instance | BindingFlags.NonPublic)
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
			string path = Assembly.GetExecutingAssembly().Location;
			string rootDir = Directory.GetParent(path).FullName;
			List<string> paths = new() { rootDir };
			var env = Environment.GetEnvironmentVariable("IRONPYTHONPATH");
			if (!string.IsNullOrWhiteSpace(env))
			{
				var items = env.Split(';');
				foreach (var item in items)
				{
					if (!string.IsNullOrWhiteSpace(item))
						paths.Add(item);
				}
			}
			//scope.SetVariable("path", paths);
			eng.SetSearchPaths(paths);

			var source = eng.CreateScriptSourceFromString(sourceCode, Microsoft.Scripting.SourceCodeKind.Statements);
			return (source, scope);
		}

		private static PythonAst ParseAndBindAst(this LanguageContext languageContext, CompilerContext context)
		{
			ScriptCodeParseResult properties = ScriptCodeParseResult.Complete;
			bool propertiesSet = false;
			int errorCode = 0;

			PythonAst ast;
			var pythonOptions = (PythonOptions)languageContext.Options;
			using (Parser parser = Parser.CreateParser(context, pythonOptions))
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