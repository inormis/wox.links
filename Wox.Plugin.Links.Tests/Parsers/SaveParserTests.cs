using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugin.Links;
using Wox.Plugin.Links.Parsers;
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
        [InlineData("-l")]
        [InlineData("link")]
        public void SaveTerms_ReturnTrueAndProposeToSave(string key) {
            _saveParser.TryParse(key.CreateQuery("Shortcut https://some.com/link-{@@} my description"),
                    out var results)
                .Should().BeTrue();
            results.Should().HaveCount(1);

            var result = results[0];
            result.Title.Should().Be("Save the link as 'Shortcut': 'my description'");
            result.SubTitle.Should().Be("https://some.com/link-{@@}");

            result.Action(new ActionContext());
            _storage.Received(1).Set("Shortcut", "https://some.com/link-{@@}", "my description");
        }

        [Theory]
        [InlineData("link1")]
        [InlineData("-link")]
        [InlineData("-l2")]
        public void NotSaveKeyWord_ReturnFalse(string key) {
            _saveParser.TryParse(key.CreateQuery("https://some.com/link Shortcut"), out var results)
                .Should()
                .BeFalse();
            results.Should().HaveCount(0);
        }
    }
}