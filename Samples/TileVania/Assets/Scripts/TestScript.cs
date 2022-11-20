using UnityEngine;
using IronPython.Custom;

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
		var myPython = new MyPython((object message) => Debug.Log(message));
		myPython.PlayingAround(sourceCode);
	}
}
