using InGameCodeEditor.Lexer;
using IronPython.Compiler;
using IronPython.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InGameCodeEditor
{
	/// <summary>
	/// A theme that can be assigned to a code editor to change the syntax highlighting rules.
	/// </summary>
	[Serializable]
	[CreateAssetMenu(fileName = "Code Language Theme", menuName = "InGame Code Editor/Code Language Theme")]
	public class CodeLanguageTheme : ScriptableObject
	{
		// Internal
		internal static readonly StringBuilder sharedBuilder = new();

		// Private
		[NonSerialized]
		private char[] delimiterSymbolCache = null;
		[NonSerialized]
		private MatchLexer[] matchers = null;

		[Header("General")]
		// Public        
		/// <summary>
		/// The name of the language that this theme provides syntax highlighting for. For example: 'C#'.
		/// </summary>
		public string languageName;

		[Tooltip("This is selectively parsing and highlighting source code using modern or legacy technique")]
		public bool enableLegacyMatcher = true;

		[Header("Indentation")]
		public TokenColor[] tokenColors;

		#region Legacy Matchers

		[Header("Legacy Matchers")]
		/// <summary>
		/// A string containing special characters separated by a space that can act as delimiter symbols when they appear before or after a keyword.
		/// Only single characters are allowed and must be separated by a space if multiple symbols need to be specified.
		/// </summary>
		[TextArea]
		[Tooltip("Any special characters that can act as delimiters when they are immediately before or after a keyword. Single characters only separated by a space")]
		public string delimiterSymbols;

		/// <summary>
		/// An array of keyword groups used to specify which words should be highlighted.
		/// </summary>
		public KeywordGroupMatch[] keywordGroups;
		/// <summary>
		/// A symbol group used to specify which symbols should be highlighted.
		/// </summary>
		public SymbolGroupMatch symbolGroup;
		/// <summary>
		/// A number group used to specify whether numbers should be highlighted.
		/// </summary>
		public NumberGroupMatch numberGroup;
		/// <summary>
		/// A comment group used to specify which strings open and close comments.
		/// </summary>
		public CommentGroupMatch commentGroup;
		/// <summary>
		/// A literal group used to specify whether quote strings should be highlighted.
		/// </summary>
		public LiteralGroupMatch literalGroup;

		#endregion Legacy Matchers

		[Header("Indentation")]
		/// <summary>
		/// Options group for all auto indent related settings.
		/// </summary>
		public AutoIndent autoIndent;

		// Properties
		internal char[] DelimiterSymbols
		{
			get
			{
				if (delimiterSymbolCache == null)
				{
					// Split by space
					string[] symbols = delimiterSymbols.Split(' ');

					int count = 0;

					// Count the number of valid symbols
					for (int i = 0; i < symbols.Length; i++)
						if (symbols[i].Length == 1)
							count++;

					// Allocate array
					delimiterSymbolCache = new char[count];

					// Copy symbols
					for (int i = 0, index = 0; i < symbols.Length; i++)
					{
						// Require only 1 character
						if (symbols[i].Length == 1)
						{
							// Get the first character for the string
							delimiterSymbolCache[index] = symbols[i][0];
							index++;
						}
					}
				}
				return delimiterSymbolCache;
			}
		}

		internal MatchLexer[] Matchers
		{
			get
			{
				if (matchers == null)
				{
					List<MatchLexer> matcherList = new()
					{
						commentGroup,
						symbolGroup,
						numberGroup,
						literalGroup
					};
					matcherList.AddRange(keywordGroups);

					matchers = matcherList.ToArray();
				}
				return matchers;
			}
		}

		internal TokenColor GetTokenColor(TokenWithSpan token)
		{
			var tokenColor = this.tokenColors.FirstOrDefault(tc => tc.advancedTokenKinds.Contains(token.AdvancedTokenKind))
				?? this.tokenColors.FirstOrDefault(tc => tc.tokenKinds.Contains(token.Token.Kind))
				?? this.tokenColors.FirstOrDefault(tc => tc.tokenRange.fromKind <= token.Token.Kind && tc.tokenRange.toKind >= token.Token.Kind);

			Debug.Log($"Match token kind {token.Token.Kind}:{token.AdvancedTokenKind} with color name {tokenColor?.name}");
			return tokenColor;
		}

		// Methods
		internal void Invalidate()
		{
			foreach (KeywordGroupMatch group in keywordGroups)
				group.Invalidate();

			symbolGroup.Invalidate();
			commentGroup.Invalidate();
			numberGroup.Invalidate();
			literalGroup.Invalidate();
		}
	}
}
