using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IronPython.Custom;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace TestLegacyConsole
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var sourceCode = @"
import sys
import System

greetingCallCount = 0

def greetings(name):
	global greetingCallCount
	greetingCallCount = greetingCallCount + 1
	if System.String.IsNullOrEmpty(name.title()):
		return 'Hello No one! [' + str(greetingCallCount) + ']'
	return 'Hello ' + name.title() + '! [' + str(greetingCallCount) + ']'

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
            var myPython = new MyPython(Console.WriteLine);
            myPython.PlayingAround(sourceCode);
        }
    }
}