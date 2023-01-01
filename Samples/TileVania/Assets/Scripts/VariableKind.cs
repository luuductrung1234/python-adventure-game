/***************************************************************
*Title: Github, IronLanguages/ironpython3, VariableKind
*Author: BCSharp
*Date: 9 August 2022
*Availability: https://github.com/IronLanguages/ironpython3/blob/master/Src/IronPython/Compiler/Ast/VariableKind.cs, (accessed 23 November 2022)
*Code version: 3.4.0
****************************************************************/

namespace IronPython.Custom
{

	/// <summary>
	/// Represents different kinds of a Python variable depending on how the variable was defined or declared.
	/// </summary>
	public enum VariableKind
	{

		/// <summary>
		/// Local variable.
		///
		/// Local variables can be referenced from nested lambdas.
		/// </summary>
		Local,

		/// <summary>
		/// Parameter to a LambdaExpression.
		///
		/// Like locals, they can be referenced from nested lambdas.
		/// </summary>
		Parameter,

		/// <summary>
		/// Global variable.
		///
		/// Should only appear in global (top level) lambda.
		/// </summary>
		Global,

		/// <summary>
		/// Nonlocal variable.
		///
		/// Provides a by-reference access to a local variable in an outer scope.
		/// </summary>
		Nonlocal,

		/// <summary>
		/// Attrribute variable.
		///
		/// Like a local variable, but is stored directly in the context dictionary,
		/// rather than a closure cell.
		/// Should only appear in a class lambda.
		/// </summary>
		Attribute
	}
}
