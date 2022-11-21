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