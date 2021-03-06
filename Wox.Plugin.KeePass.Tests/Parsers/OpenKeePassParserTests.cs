using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugins.Common;
using Wox.Plugins.KeePass.Parsers;
using Xunit;

namespace Wox.Plugins.KeePass.Tests.Parsers {
    public class OpenKeePassParserTests {
        public OpenKeePassParserTests() {
            _storage = Substitute.For<IStorage>();
            _storage.KeePath.Returns(new KeePath {
                ApplicationPath = @"c:\keepass.exe",
                KeePassFilePath = @"c:\file.kdbx"
            });
            _storage.KeePassPathIsConfigured.Returns(true);
            _fileService = Substitute.For<IFileService>();
            parser = new OpenKeePassParser(_storage, _fileService);
        }

        private readonly OpenKeePassParser parser;
        private readonly IStorage _storage;
        private readonly IFileService _fileService;
        private const string FilePath = @"c:\file.kdbx";

        [Fact]
        public void PassingPasswordForFirstTime_SavesIt() {
            var password = "somepassword";
            parser.TryParse(password, out var results);

            results.Should().HaveCount(1);
            results.Single().Title.Should().Be("Open 'file.kdbx' with given password");
            results.Single().Action(new ActionContext());

            _fileService.Received(1).Start($@"{_storage.KeePath.ApplicationPath}",
                $@"""{_storage.KeePath.KeePassFilePath}"" -pw:{password}");

            parser.TryParse("", out results);

            results.Single().Action(new ActionContext());
            _fileService.Received(2).Start($@"{_storage.KeePath.ApplicationPath}",
                $@"""{_storage.KeePath.KeePassFilePath}"" -pw:{password}");
        }
    }
}