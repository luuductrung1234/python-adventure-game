/***************************************************************
*Title: Github, IronLanguages/ironpython3, ThrowingErrorSink
*Author: solzier
*Date: 11 August 2019
*Availability: https://github.com/IronLanguages/ironpython3/blob/master/Src/IronPython/Runtime/ThrowingErrorSink.cs, (accessed 20 November 2022)
*Code version: 3.4.0
****************************************************************/

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
