using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugins.Common;
using Wox.Plugins.KeePass.Parsers;
using Xunit;

namespace Wox.Plugins.KeePass.Tests.Parsers {
    public class SetKeePassPathParserTests {
        public SetKeePassPathParserTests() {
            _storage = Substitute.For<IStorage>();
            _fileService = Substitute.For<IFileService>();
            parser = new SetKeePassPathParser(_storage, _fileService);
            _storage.KeePassPathIsConfigured.Returns(true);
        }

        private readonly SetKeePassPathParser parser;
        private readonly IStorage _storage;
        private readonly IFileService _fileService;
        private const string FilePath = @"c:\file.kdbx";

        [Fact]
        public void PassingInvalidKdbxFilePath_ReturnsFalse() {
            _fileService.Exists(FilePath).Returns(false);
            var tryParse = parser
                .TryParse(FilePath, out var results);
            tryParse.Should().BeFalse();
            results.Should().BeEmpty();
        }

        [Fact]
        public void PassingKdbxFilePath_UpdatesStorage() {
            _fileService.Exists(FilePath).Returns(true);
            _fileService.CheckExtension(FilePath, ".kdbx").Returns(true);
            var tryParse = parser
                .TryParse(FilePath, out var results);
            tryParse.Should().BeTrue();
            results.Should().HaveCount(1);

            results.Single().Title.Should().Be($"Set '{FilePath}' as keepass path");
            results.Single().Action.Invoke(new ActionContext());

            _storage.Received(1).SetKeePassPath(FilePath);
        }
    }
}