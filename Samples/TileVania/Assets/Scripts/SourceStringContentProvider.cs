/***************************************************************
*Title: Github, IronLanguages/dlr, SourceStringContentProvider
*Author: slozier
*Date: 30 July 2018
*Availability: https://github.com/IronLanguages/dlr/blob/master/Src/Microsoft.Scripting/Runtime/SourceStringContentProvider.cs, (accessed 20 November 2022)
*Code version: 1.3.3
****************************************************************/

using System;
using System.IO;
using Microsoft.Scripting;
using Microsoft.Scripting.Utils;

namespace IronPython.Custom
{
	[Serializable]
	internal sealed class SourceStringContentProvider : TextContentProvider
	{
		private readonly string _code;

		internal SourceStringContentProvider(string code)
		{
			ContractUtils.RequiresNotNull(code, nameof(code));

			_code = code;
		}

		public override SourceCodeReader GetReader()
		{
			return new SourceCodeReader(new StringReader(_code), null);
		}
	}
}