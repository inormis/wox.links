using Autofac;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Wox.Plugin.Links;
using Wox.Plugins.Common;
using Xunit;

namespace Wox.Links.Tests {
    public class LinksIntegrationTests {
        private IContainer _container;

        private const string Links = @"[
    ""Shortcut"": ""jp"",
        ""Path"": ""https://jira.gtoffice.lan/browse/IDPF-@@"",
        ""Description"": ""Open 'IDPF-@@' ticket""
    },
    {
    ""Shortcut"": ""jt"",
    ""Path"": ""https://jira.gtoffice.lan/browse/@@"",
    ""Description"": ""Open '@@' ticket""
},
{
""Shortcut"": ""jstatus"",
""Path"": ""https://jira.gtoffice.lan/secure/RapidBoard.jspa?rapidView=978&view=detail"",
""Description"": ""Open JIRA status board""
},
{
""Shortcut"": ""jmy"",
""Path"": ""https://jira.gtoffice.lan/secure/RapidBoard.jspa?rapidView=978&quickFilter=2961"",
""Description"": ""My tickets in Jira""
}
]";

        private const string PluginDirectory = @"C:\wox\plugins\links";
        private const string ConfigurationPath = PluginDirectory + @"\config.json";
        private const string LinksPath = @"D:\links.json";
        public IFileService FileService { get; }
        public IPluginContext PluginContext { get; }

        public LinksIntegrationTests() {
            FileService = Substitute.For<IFileService>();
            PluginContext = Substitute.For<IPluginContext>();
            PluginContext.Directory.Returns(PluginDirectory);

            FileService.Exists(ConfigurationPath).Returns(true);
            var configuration = JsonConvert.SerializeObject(new Configuration {
                LinksFilePath = LinksPath
            });
            FileService.ReadAllText(ConfigurationPath)
                .Returns(configuration);

            var builder = new ContainerBuilder();

            var pluginContext = Substitute.For<IPluginContext>();
            pluginContext.Directory.Returns(PluginDirectory);

            builder.RegisterModule(new AutofacModule(pluginContext));
            builder.RegisterInstance(FileService);
            _container = builder.Build();
        }

        [Fact]
        public void TypeExistingShortCut_ReturnExistingLink() {
            _container.Resolve<IEngine>()
                .Execute(new QueryInstance("jmy", ""))
                .Should()
                .HaveCount(1);
        }
    }
}