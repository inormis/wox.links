using System.Threading;
using NSubstitute;
using Xunit;

namespace Wox.Plugins.KeePass.Tests {
    public class ConfigurationTests {
        [Fact]
        public void FindKeePass() {
            var storage = Substitute.For<IStorage>();
            storage.KeePath.Returns(new KeePath());
            var configuration = new Configuration(storage);

            configuration.Initialize();
            Thread.Sleep(1000);
            storage.Received(1).SetApplicationPath(Arg.Any<string>());
        }
    }
}