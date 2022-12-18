using System;
using IronPython.Custom;

namespace TestLegacyConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
	        var sourceCode = seedQuizAnswer;
            var myPython = new MyPython(Console.WriteLine);
            myPython.SampleUsage(sourceCode);
        }
        
        private const string seedQuizAnswer = @"# import sys
# import System
# you can import build-in modules (of Python or .NET Clr)

from_rod = 'A'
aux_rod = 'B'
to_rod = 'C'
disks_num = 3

# move all disks (3) from rod A to rod C

# answer start

def answer():
	# write your own code to solve the challenge here...
	steps = [
		{ 'disk': 1, 'from': from_rod, 'to': to_rod },
		{ 'disk': 2, 'from': from_rod, 'to': aux_rod },
		{ 'disk': 1, 'from': to_rod, 'to': aux_rod },
	]
	return steps

# answer end
"; 
        
	    private const string sample01 = @"
import sys
import System
# just to test how comment token look like
some_text = 'abcXYZ'
another_text = '123'
some_num = 34.5
another_num = 120
some_arr = [1, 2, '3', 'abc', 5.0]
some_dic = {'name': 'abc', 'age': 2, 'jobs': [ 'abc', 'def' ]}
some_flag = True
some_tuple = ('abc', 123, '3da')
some_set = {'abc', 123, '3da'}
some_class = type('abc')
another_class = type(25.2)
greeting_call_count = 0	# I have some comment here

# just to test how comment token look like

def greetings(name):
	'''note something about this method'''
	global greeting_call_count
	greeting_call_count = greeting_call_count + 1
	if System.String.IsNullOrEmpty(name.title()):
		return 'Hello No one! [' + str(greeting_call_count) + ']'
	return 'Hello ' + name.title() + '! [' + str(greeting_call_count) + ']'

def test_nested_function(some_input):
	'''test nested function/scope and variable in python'''
	x = 10
	nested_function('text text')
	def nested_function(another_input):
		y = 20
		if x < y:
			print('x is smaller than y')
			z = 30
	class NestedClass:
		a = 100
		b = 'text text'

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

class YoungPerson:
	money = 5
	address = 'some value'
	def __init__(self, name, age):
		self.name = name
		self.age = age
	def __str__(self):
		return self.name + '(' + str(self.age) + ')'
	def myfunc(self):
		print('Hello my name is ' + self.name)
";
    }
}