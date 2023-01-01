/***************************************************************
*Title: n/a
*Author: Luu Duc Trung
*Date: n/a
*Availability: n/a
*Code version: n/a
****************************************************************/

using System;
using System.Collections.Generic;
using IronPython.Compiler;
using UnityEngine;

namespace IronPython.Custom
{
	[Serializable]
	public class TokenColor
	{
		public string name;

		[Tooltip("AdvancedTokenKind has higher precedence than TokenKind")]
		public List<AdvancedTokenKind> advancedTokenKinds;

		[Tooltip("TokenKind has higher precedence than TokenRange")]
		public List<TokenKind> tokenKinds;

		[Tooltip("TokenKind has higher precedence than TokenRange")]
		public TokenRange tokenRange;

		public ColorSettings colorSettings;
	}

	[Serializable]
	public class TokenRange
	{
		public TokenKind fromKind;
		public TokenKind toKind;
	}

	[Serializable]
	public class ColorSettings
	{
		public Color foreground = Color.black;
	}
}