using UnityEngine;
using IronPython.Hosting;
using IronPython.Runtime;
using IronPython.Custom;
using Microsoft.Scripting.Hosting;
using System.Collections.Generic;
using System;

public class TestScript : MonoBehaviour
{
	void Start()
	{
		var sourceCode = @"
import sys
import System

def greetings(name):
	if System.String.IsNullOrEmpty(name.title()):
		return 'Hello No one!'
	return 'Hello ' + name.title() + '!'

def get_sys_info_list():
	return [
		str(sys.modules), 
		str(sys.path), 
		str(sys.argv), 
		str(sys.version)
	]

def get_sys_info_dict():
	return {
		'modules': str(sys.modules), 
		'path': str(sys.path),
		'argv': str(sys.argv),
		'version': str(sys.version)
	}
";

		var engine = Python.CreateEngine(new Dictionary<string, object>() { { "Debug", false } });
		//RunScript(engine, sourceCode);
		AnalyzeScript(engine, sourceCode);
	}

	private void AnalyzeScript(ScriptEngine engine, string sourceCode)
	{
		var ast = engine.GenerateAst(sourceCode);
	}

	private void RunScript(ScriptEngine engine, string sourceCode)
	{
		var (source, scope) = engine.Prepare(sourceCode);

		try
		{
			var compiledCode = source.Compile();
			compiledCode.Execute(scope);
		}
		catch (Exception ex)
		{
			var eo = engine.GetService<ExceptionOperations>();
			Debug.Log($"{eo.FormatException(ex)}");
		}

		TryCallScriptMethods(scope);
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
			Debug.Log($"Found item (in scope): {item.Key}");
		}

		var isExist = scope.TryGetVariable("greetings", out dynamic greetings);
		if (isExist)
		{
			Debug.Log(greetings(""));
			Debug.Log(greetings("World"));
		}

		isExist = scope.TryGetVariable("get_sys_info_list", out dynamic showSysInfoList);
		if (isExist)
		{
			var infoList = (List)showSysInfoList();
			Debug.Log($"{infoList.GetType()}");
			foreach (var info in infoList)
			{
				Debug.Log(info);
			}
		}

		isExist = scope.TryGetVariable("get_sys_info_dict", out dynamic showSysInfoDict);
		if (isExist)
		{
			var infoDict = (PythonDictionary)showSysInfoDict();
			foreach (var entry in infoDict)
			{
				Debug.Log($"{entry.Key}: {entry.Value}");
			}
		}
	}
}
