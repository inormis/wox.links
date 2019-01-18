using System.Collections.Generic;
using Wox.Links.Parsers;
using Wox.Plugin;
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