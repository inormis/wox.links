using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugins.Common;
using Wox.Plugins.KeePass.Parsers;
using Xunit;

namespace Wox.Plugins.KeePass.Tests {
    public class StartupTests {
        [Fact]
        public void Setup() {
            Startup.Initialize(new PluginContext(new PluginInitContext()));

            Startup.Resolve<IEnumerable<IParser>>();
        }
    }
}