using IronPython.Compiler;
using Microsoft.Scripting;

namespace IronPython.Custom
{
	public readonly struct TokenWithSpan
	{
		public TokenWithSpan(Token token, IndexSpan span, SourceLocation location)
		{
			Token = token;
			Span = span;
			Location = location;
		}

		public IndexSpan Span { get; }
		public SourceLocation Location { get; }
		public Token Token { get; }

	}
}