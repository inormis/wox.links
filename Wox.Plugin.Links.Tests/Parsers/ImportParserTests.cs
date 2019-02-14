using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugin.Links;
using Wox.Plugin.Links.Parsers;
using Wox.Plugins.Common;
using Xunit;

namespace Wox.Links.Tests.Parsers {
    public class ImportParserTests {
        private const string FilePath = @"C:\file.json";
        private readonly ImportParser _saveParser;
        private readonly IStorage _storage;
        private readonly IFileService _fileService;
        private readonly QueryInstance _queryInstance;

        public ImportParserTests() {
            _storage = Substitute.For<IStorage>();
            _fileService = Substitute.For<IFileService>();
            _saveParser = new ImportParser(_storage, _fileService);
            _queryInstance = new QueryInstance(@"link C:\file.json", new []{"link", FilePath});
        }

        [Fact]
        public void ImportedFileExisting_ReturnFalse() {
            _fileService.Exists(FilePath).Should().BeTrue();
            _saveParser.TryParse(_queryInstance, out var results).Should()
                .BeTrue();
            results.Should().HaveCount(1);
            results.Single().Title.Should().Be("Import configuration file and replace current");
        }
        [Fact]
        public void ImportedFileNotExisting_ReturnFalse() {
            _saveParser.TryParse(_queryInstance, out var results).Should()
                .BeTrue();
            results.Should().HaveCount(1);
            results.Single().Title.Should().Be("Import configuration file and replace current");
        }
    }
}