using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests.Parsers {
    public class SaveParserTests {
        private readonly SaveParser _saveParser;
        private readonly IStorage _storage;

        public SaveParserTests() {
            _storage = Substitute.For<IStorage>();
            _saveParser = new SaveParser(_storage);
        }

        [Theory]
        [InlineData("--save")]
        [InlineData("-s")]
        public void SaveTerms_ReturnTrueAndProposeToSave(string key) {
            _saveParser.TryParse(new[] {key, "Shortcut", "https://some.com/link-{0}", "my home page"}, out var results)
                .Should()
                .BeTrue();
            results.Should().HaveCount(1);

            var result = results[0];
            result.Title.Should().Be("Save the link as 'Shortcut'");
            result.SubTitle.Should().Be("https://some.com/link-{0}");

            result.Action(new ActionContext());
            _storage.Received(1).Set("Shortcut", "https://some.com/link-{0}", "my home page");
        }

        [Theory]
        [InlineData("--save1")]
        [InlineData("-save")]
        [InlineData("-s2")]
        public void NotSaveKeyWord_ReturnFalse(string key) {
            _saveParser.TryParse(new[] {key, "https://some.com/link", "Shortcut"}, out var results).Should()
                .BeFalse();
            results.Should().HaveCount(0);
        }
    }
}