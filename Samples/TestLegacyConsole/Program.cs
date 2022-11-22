using System;
using IronPython.Custom;

namespace TestLegacyConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var sourceCode = @"
import sys
import System
# just to test how comment token look like
someText = 'abcXYZ'
greetingCallCount = 0	# I have some comment here

# just to test how comment token look like

def greetings(name):
	'''note something about this method'''
	global greetingCallCount
	greetingCallCount = greetingCallCount + 1
	if System.String.IsNullOrEmpty(name.title()):
		return 'Hello No one! [' + str(greetingCallCount) + ']'
	return 'Hello ' + name.title() + '! [' + str(greetingCallCount) + ']'

def get_sys_info_list():
	'''
	this is how multiple line docstring
	look like
	'''
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
            myPython.SampleUsage(sourceCode);
        }
    }
}