using Microsoft.Scripting;

using IronPython.Runtime.Operations;

namespace IronPython.Custom
{
	internal class ThrowingErrorSink : ErrorSink
	{
		public static new readonly ThrowingErrorSink/*!*/ Default = new();

		private ThrowingErrorSink() { }

		public override void Add(SourceUnit sourceUnit, string message, SourceSpan span, int errorCode, Severity severity)
		{
			if (severity == Severity.Warning)
			{
				PythonOps.SyntaxWarning(message, sourceUnit, span, errorCode);
			}
			else
			{
				throw PythonOps.SyntaxError(message, sourceUnit, span, errorCode);
			}
		}
	}
}
