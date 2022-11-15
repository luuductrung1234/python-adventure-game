using UnityEngine;
using IronPython.Hosting;
using IronPython.Runtime;

public class TestScript : MonoBehaviour
{
	void Start()
	{
		var eng = Python.CreateEngine();
		var scope = eng.CreateScope();
		eng.Execute(@"
import sys

def greetings(name):
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
", scope);

		dynamic greetings = scope.GetVariable("greetings");
		Debug.Log(greetings("World"));

		dynamic showSysInfoList = scope.GetVariable("get_sys_info_list");
		var infoList = (List)showSysInfoList();
		Debug.Log($"{infoList.GetType()}");
		foreach (var info in infoList)
		{
			Debug.Log(info);
		}

		dynamic showSysInfoDict = scope.GetVariable("get_sys_info_dict");
		var infoDict = (PythonDictionary)showSysInfoDict();
		foreach (var entry in infoDict)
		{
			Debug.Log($"{entry.Key}: {entry.Value}");
		}
	}
}

