using System.Collections.Generic;
using Autofac;
using Wox.Plugin;
using Wox.Plugin.Links;
using Wox.Plugin.Links.Parsers;
using Wox.Plugins.Common;
using Xunit;

namespace Wox.Links.Tests {
    public class StartupTests {
        private IContainer _container;

        [Fact]
        public void Setup() {
            Startup.Initialize(new PluginContext(new PluginInitContext()));

            Startup.Resolve<IEnumerable<IParser>>();
        }

        public StartupTests() {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule(new PluginContext(new PluginInitContext())));
            _container = builder.Build();
        }
        
        
    }
}