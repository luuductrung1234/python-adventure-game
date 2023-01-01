/***************************************************************
*Title: Github, IronLanguages/ironpython3, Token
*Author: solzier
*Date: 14 March 2022
*Availability: https://github.com/IronLanguages/ironpython3/blob/master/Src/IronPython/Compiler/Token.cs, (accessed 23 November 2022)
*Code version: 3.4.0
****************************************************************/

using IronPython.Compiler;
using IronPython.Compiler.Ast;
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
			ParentScope = null;
		}

		public TokenWithSpan(Token token, IndexSpan span, SourceLocation location, AdvancedTokenKind advancedTokenKind)
		{
			Token = token;
			Span = span;
			Location = location;
			AdvancedTokenKind = advancedTokenKind;
			ErrorMessage = token.Kind == TokenKind.Error ? ((ErrorToken)token).Message : null;
			ParentScope = null;
		}

		public IndexSpan Span { get; }
		public SourceLocation Location { get; }
		public Token Token { get; }
		public AdvancedTokenKind AdvancedTokenKind { get; private set; }
		public string ErrorMessage { get; }
		public ScopeStatement ParentScope { get; private set; }

		public void SetContext(AdvancedTokenKind kind, ScopeStatement parentScope = null)
		{
			AdvancedTokenKind = kind;
			ParentScope = parentScope;
		}
	}
}