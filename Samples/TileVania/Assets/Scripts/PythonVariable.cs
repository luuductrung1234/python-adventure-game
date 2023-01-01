/***************************************************************
*Title: Github, IronLanguages/ironpython3, PythonVariable
*Author: BCSharp
*Date: 03 September 2022
*Availability: https://github.com/IronLanguages/ironpython3/blob/master/Src/IronPython/Compiler/Ast/PythonVariable.cs, (accessed 23 November 2022)
*Code version: 3.4.0
****************************************************************/

#nullable enable

using MSAst = System.Linq.Expressions;
using IronPython.Compiler.Ast;

namespace IronPython.Custom
{
	using Ast = MSAst.Expression;

	public class PythonVariable
	{

		public PythonVariable(string name, VariableKind kind, ScopeStatement/*!*/ scope)
		{
			Name = name;
			Kind = kind;
			Scope = scope;
		}

		/// <summary>
		/// The name of the variable as used in Python code.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// The original scope in which the variable is defined.
		/// </summary>
		public ScopeStatement Scope { get; }

		public VariableKind Kind { get; private set; }

		/// <summary>
		/// Transform a local kind of variable in a global scope (yes, they can be created)
		/// into a global kind, hence explicitly accessible to nested local scopes.
		/// </summary>
		internal void LiftToGlobal()
		{
			Kind = VariableKind.Global;
		}

		/// <summary>
		/// The actual variable represented by this variable instance.
		/// For reference variables this may be null if the reference is not yet resolved.
		/// </summary>
		public virtual PythonVariable? LimitVariable => this;

		/// <summary>
		/// Gets a value indicating whether the variable gets deleted by a <c>del</c> statement in any scope.
		/// </summary>
		internal bool MaybeDeleted { get; private set; }

		/// <summary>
		/// Mark the variable as argument to a del statement in some scope.
		/// </summary>
		internal void RegisterDeletion() => MaybeDeleted = true;

		/// <summary>
		/// Gets the index used for tracking in the flow checker.
		/// </summary>
		internal int Index { get; set; }

		/// <summary>
		/// True iff there is a path in the control flow graph of a single scope
		/// on which the variable is used before explicitly initialized (assigned or deleted)
		/// in that scope.
		/// </summary>
		public bool ReadBeforeInitialized { get; set; }

		/// <summary>
		/// True iff the variable is referred to from an inner scope.
		/// </summary>
		public bool AccessedInNestedScope { get; set; }
	}
}
