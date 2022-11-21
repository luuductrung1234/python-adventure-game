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