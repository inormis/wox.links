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
            _file = Substitute.For<IFile>();
            parser = new SetKeePassPathParser(_storage, _file);
            _storage.KeePassPathIsConfigured.Returns(true);
        }

        private readonly SetKeePassPathParser parser;
        private readonly IStorage _storage;
        private readonly IFile _file;
        private const string FilePath = @"c:\file.kdbx";

        [Fact]
        public void PassingInvalidKdbxFilePath_ReturnsFalse() {
            _file.Exists(FilePath).Returns(false);
            var tryParse = parser
                .TryParse(FilePath, out var results);
            tryParse.Should().BeFalse();
            results.Should().BeEmpty();
        }

        [Fact]
        public void PassingKdbxFilePath_UpdatesStorage() {
            
            _file.Exists(FilePath).Returns(true);
            _file.CheckExtension(FilePath, ".kdbx").Returns(true);
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