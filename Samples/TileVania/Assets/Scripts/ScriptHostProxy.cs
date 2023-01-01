/***************************************************************
*Title: Github, IronLanguages/dlr, ScriptHostProxy
*Author: gpetrou
*Date: 10 November 2018
*Availability: https://github.com/IronLanguages/dlr/blob/master/Src/Microsoft.Scripting/Hosting/ScriptHostProxy.cs, (accessed 20 November 2022)
*Code version: 1.3.3
****************************************************************/

using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Utils;

namespace IronPython.Custom
{
	internal sealed class ScriptHostProxy : DynamicRuntimeHostingProvider
	{
		private readonly ScriptHost _host;

		public ScriptHostProxy(ScriptHost host)
		{
			Assert.NotNull(host);
			_host = host;
		}

		public override PlatformAdaptationLayer PlatformAdaptationLayer => _host.PlatformAdaptationLayer;
	}
}