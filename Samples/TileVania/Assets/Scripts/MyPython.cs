using System;
using System.Collections.Generic;
using System.Linq;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using IronPython.Modules;
using IronPython.Runtime;
using IronPython.Runtime.Types;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

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
		public CodeContext CodeContext { get; private set; }
		public List<TokenWithSpan> Tokens { get; private set; }
		public string CachedSourceCode { get; private set; }

		public void SampleUsage(string sourceCode)
		{
			AnalyzeScript(sourceCode);
			var scope = RunScript();
			TryCallScriptMethods(scope);
			TryTypes(scope);
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
				CodeContext = ast.GetCodeContext();
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
			return scope;
		}

		public ScriptScope RunScript(string sourceCode)
		{
			var (source, scope) = Engine.Prepare(sourceCode);
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
			return scope;
		}

		public bool QuickAssert(string sourceCode, string variableName, out dynamic output, string expectedResult = null, params object[] inputs)
		{
			AnalyzeScript(sourceCode);
			var scope = RunScript();
			var isSuccess = expectedResult == null
				? TryRunFunction(scope, variableName, out output, inputs)
				: TryRunFunction(scope, variableName, out output, inputs) && DynamicEqual(output, expectedResult, CodeContext);
			// TODO: assert global variable, lambda,...
			return isSuccess;
		}

		public bool TryRunFunction(ScriptScope scope, string functionName, out dynamic output, params object[] inputs)
		{
			var isExist = scope.TryGetVariable(functionName, out var userDefinedFunc);
			if (isExist && userDefinedFunc is Runtime.PythonFunction)
			{
				output = inputs.Length > 0 ? userDefinedFunc(inputs) : userDefinedFunc();
				return true;
			}
			Logging("User didn't define 'answer' function correctly!");
			output = null;
			return false;
		}

		private void TryTypes(ScriptScope scope)
		{
			var someText = scope.GetVariable("some_text");
			Logging(someText.GetType().FullName);
			Logging(DynamicEqual(someText, "abcXYZ", CodeContext));

			var anotherText = scope.GetVariable("another_text");
			Logging(anotherText.GetType().FullName);
			Logging(DynamicEqual(anotherText, "123", CodeContext));

			var someNum = scope.GetVariable("some_num");
			Logging(someNum.GetType().FullName);
			Logging(DynamicEqual(someNum, "34.5", CodeContext));

			var anotherNum = scope.GetVariable("another_num");
			Logging(anotherNum.GetType().FullName);
			Logging(DynamicEqual(anotherNum, "120", CodeContext));

			var someArr = scope.GetVariable("some_arr");
			Logging(someArr.GetType().FullName);
			Logging(DynamicEqual(someArr, "[1, 2, '3', 'abc', 5.0]", CodeContext));

			var someDic = scope.GetVariable("some_dic");
			Logging(someDic.GetType().FullName);
			Logging(DynamicEqual(someDic, "{ 'name': 'abc', 'age': 2, 'jobs': [ 'abc','def' ] }", CodeContext));

			var someFlag = scope.GetVariable("some_flag");
			Logging(someFlag.GetType().FullName);
			Logging(DynamicEqual(someFlag, "True", CodeContext));

			var someTuple = scope.GetVariable("some_tuple");
			Logging(someTuple.GetType().FullName);
			Logging(DynamicEqual(someTuple, "('abc','3da', 123 )", CodeContext));

			var someSet = scope.GetVariable("some_set");
			Logging(someSet.GetType().FullName);
			Logging(DynamicEqual(someSet, "{'3da', 'abc', 123}", CodeContext));

			var someClass = scope.GetVariable("some_class");
			Logging(someClass.GetType().FullName);
			Logging(DynamicEqual(someClass, "<type 'str'>", CodeContext));

			var anotherClass = scope.GetVariable("another_class");
			Logging(anotherClass.GetType().FullName);
			Logging(DynamicEqual(anotherClass, "<type 'float'>", CodeContext));
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
			if (isExist)
			{
				var infoDict = (PythonDictionary)showSysInfoDict();
				foreach (var entry in infoDict)
				{
					Logging($"{entry.Key}: {entry.Value}");
				}
			}

			isExist = scope.TryGetVariable("answer", out var answer);
			if (isExist)
			{
				Runtime.List rows = (Runtime.List)answer();
				foreach (string row in rows)
				{
					Logging($"{row}");
				}
			}
		}

		private static bool DynamicEqual(dynamic value, string text, CodeContext codeContext)
		{
			switch (value)
			{
				case double doubleValue:
					{
						var isSuccess = double.TryParse(text, out var doubleFromString);
						return isSuccess && doubleValue.Equals(doubleFromString);
					}
				case int intValue:
					{
						var isSuccess = int.TryParse(text, out var intFromString);
						return isSuccess && intValue.Equals(intFromString);
					}
				case bool boolValue:
					{
						var isSuccess = bool.TryParse(text, out var boolFromString);
						return isSuccess && boolValue.Equals(boolFromString);
					}
				case List listValue:
					{
						return Builtin.eval(codeContext, text) is List generatedList
									 && listValue.Count == generatedList.Count
									 && listValue.All(item => generatedList.Contains(item));
					}
				case PythonDictionary dictValue:
					{
						return Builtin.eval(codeContext, text) is PythonDictionary generatedDict
							&& dictValue.Keys.Count == generatedDict.Keys.Count
							&& dictValue.Values.Count == generatedDict.Values.Count
							&& dictValue.__cmp__(codeContext, generatedDict) == 0;
					}
				case PythonTuple tupleValue:
					{
						return Builtin.eval(codeContext, text) is PythonTuple generatedTuple
							&& tupleValue.Count == generatedTuple.Count
							&& tupleValue.All(item => generatedTuple.Contains(item));
					}
				case SetCollection setValue:
					{
						var textInSetFormat = "set(" + text.Replace('{', '[')
							.Replace('}', ']')
							.Replace(" ", string.Empty) + ")";
						return Builtin.eval(codeContext, textInSetFormat) is SetCollection generatedSet
							&& setValue.Count == generatedSet.Count
							&& setValue.All(item => generatedSet.Contains(item));
					}
				case PythonType typeValue:
					{
						return typeValue.__repr__(codeContext).Equals(text);
					}
				default:
					return value is string stringValue && stringValue.Equals(text);
			}
		}
	}
}