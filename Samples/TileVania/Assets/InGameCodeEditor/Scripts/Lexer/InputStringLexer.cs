using System;
using System.Collections.Generic;

namespace InGameCodeEditor.Lexer
{
	internal struct InputStringMatchInfo
	{
		public int startIndex;
		public int endIndex;
		public string htmlColor;
	}

	internal class InputStringLexer : ILexer
	{
		// Private
		private string inputString = null;
		private MatchLexer[] matchers = null;
		private readonly HashSet<char> specialStartSymbols = new();
		private readonly HashSet<char> specialEndSymbols = new();
		private char current = ' ';
		private char previous = ' ';
		private int currentIndex = 0;
		private int currentLookaheadIndex = 0;

		// Properties
		public bool EndOfStream
		{
			get { return currentLookaheadIndex >= inputString.Length; }
		}

		public char Previous
		{
			get { return previous; }
		}

		// Methods
		public void UseMatchers(char[] delimiters, MatchLexer[] matchers)
		{
			// Store matchers
			this.matchers = matchers;

			// Clear old symbols
			specialStartSymbols.Clear();
			specialEndSymbols.Clear();

			// Check for any delimiter characters
			if (delimiters != null)
			{
				// Add delimiters
				foreach (char character in delimiters)
				{
					// Add to start
					if (specialStartSymbols.Contains(character) == false)
						specialStartSymbols.Add(character);

					// Add to end
					if (specialEndSymbols.Contains(character) == false)
						specialEndSymbols.Add(character);
				}
			}

			// Check for any matchers
			if (matchers != null)
			{
				// Add all special symbols which can act as a delimiter
				foreach (MatchLexer lexer in matchers)
				{
					foreach (char special in lexer.SpecialStartCharacters)
						if (specialStartSymbols.Contains(special) == false)
							specialStartSymbols.Add(special);

					foreach (char special in lexer.SpecialEndCharacters)
						if (specialEndSymbols.Contains(special) == false)
							specialEndSymbols.Add(special);
				}
			}
		}

		public IEnumerable<InputStringMatchInfo> LexInputString(string input)
		{
			// Check for no matchers
			if (matchers == null || matchers.Length == 0)
				yield break;

			// Store the input string
			this.inputString = input ?? throw new ArgumentNullException("input");
			this.current = ' ';
			this.previous = ' ';
			this.currentIndex = 0;
			this.currentLookaheadIndex = 0;

			// Process the input string
			while (EndOfStream == false)
			{
				bool didMatchLexer = false;

				// Read any white spaces
				ReadWhiteSpace();

				// Process each matcher
				foreach (MatchLexer matcher in matchers)
				{
					// Get the current index
					int startIndex = currentIndex;

					// Try to match
					bool isMatched = matcher.IsMatch(this);

					if (isMatched == true)
					{
						// Get the end index of the match
						int endIndex = currentIndex;

						// Set matched flag
						didMatchLexer = true;

						// Register the match
						yield return new InputStringMatchInfo
						{
							startIndex = startIndex,
							endIndex = endIndex,
							htmlColor = matcher.HTMLColor,
						};

						// Move to next character
						break;
					}
				}

				if (didMatchLexer == false)
				{
					// Move to next
					ReadNext();
					Commit();
				}
			}
		}

		/// <summary>
		/// Read single character at <see cref="currentLookaheadIndex"/>
		/// </summary>
		/// <returns></returns>
		public char ReadNext()
		{
			// Check for end of stream
			if (EndOfStream == true)
				return '\0';

			// Update previous character
			previous = current;

			// Get the character
			current = inputString[currentLookaheadIndex];
			currentLookaheadIndex++;

			return current;
		}

		/// <summary>
		/// Rollback for both <see cref="currentLookaheadIndex"/> and <see cref="previous"/>
		/// </summary>
		/// <param name="amount"></param>
		public void Rollback(int amount = -1)
		{
			if (amount == -1)
			{
				// Revert to index
				currentLookaheadIndex = currentIndex;
			}
			else
			{
				if (currentLookaheadIndex > currentIndex)
					currentLookaheadIndex -= amount;
			}

			int previousIndex = currentLookaheadIndex - 1;

			if (previousIndex >= inputString.Length)
				previous = inputString[^1];
			else if (previousIndex >= 0)
				previous = inputString[previousIndex];
			else
				previous = ' ';
		}

		/// <summary>
		/// Sync <see cref="currentIndex"/> with <see cref="currentLookaheadIndex"/>
		/// </summary>
		public void Commit()
		{
			currentIndex = currentLookaheadIndex;
		}

		public bool IsSpecialSymbol(char character, SpecialCharacterPosition position = SpecialCharacterPosition.Start)
		{
			if (position == SpecialCharacterPosition.Start)
				return specialStartSymbols.Contains(character);

			return specialEndSymbols.Contains(character);
		}

		/// <summary>
		/// Skip all white space from current index
		/// </summary>
		private void ReadWhiteSpace()
		{
			// Read until white space
			while (char.IsWhiteSpace(ReadNext()) == true)
			{
				// Consume the char
				Commit();
			}

			// Return lexer state
			Rollback();
		}
	}
}
