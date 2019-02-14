using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugin.Links;
using Wox.Plugin.Links.Parsers;
using Xunit;

namespace Wox.Links.Tests {
    public class StartupTests {
        [Fact]
        public void Setup() {
            Startup.Initialize(new PluginInitContext());

            Startup.Resolve<IEnumerable<IParser>>();
        }
    }
}