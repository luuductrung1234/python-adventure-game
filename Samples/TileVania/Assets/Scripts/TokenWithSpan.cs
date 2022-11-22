using IronPython.Compiler;
using Microsoft.Scripting;
using TokenKind = IronPython.Compiler.TokenKind;

namespace IronPython.Custom
{
	public class TokenWithSpan
	{
		public TokenWithSpan(Token token, IndexSpan span, SourceLocation location)
		{
			Token = token;
			Span = span;
			Location = location;
			ErrorMessage = token.Kind == TokenKind.Error ? ((ErrorToken)token).Message : null;
			AdvancedTokenKind = Custom.AdvancedTokenKind.Unspecified;
		}

		public TokenWithSpan(Token token, IndexSpan span, SourceLocation location, AdvancedTokenKind advancedTokenKind)
		{
			Token = token;
			Span = span;
			Location = location;
			AdvancedTokenKind = advancedTokenKind;
			ErrorMessage = token.Kind == TokenKind.Error ? ((ErrorToken)token).Message : null;
		}

		public IndexSpan Span { get; }
		public SourceLocation Location { get; }
		public Token Token { get; }
		public AdvancedTokenKind AdvancedTokenKind { get; private set; }
		public string ErrorMessage { get; }

		public void SetAdvancedTokenKind(AdvancedTokenKind kind)
				=> AdvancedTokenKind = kind;
	}
}