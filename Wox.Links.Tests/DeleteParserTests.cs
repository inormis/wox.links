using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests
{
    public class DeleteParserTests
    {
        private readonly IStorage _storage;
        private readonly DeleteParser _parser;

        public DeleteParserTests()
        {
            _storage = Substitute.For<IStorage>();
            _parser = new DeleteParser(_storage);
        }

        [Theory]
        [InlineData("--delete")]
        [InlineData("--remove")]
        [InlineData("-d")]
        [InlineData("-r")]
        public void DeleteWithShortcut_ReturnAllLinks(string key)
        {
            _storage.GetShortcuts().Returns(new[]
            {
                new Link
                {
                    Shortcut = "Ad1",
                    Path = "https://ad1"
                },
                new Link
                {
                    Shortcut = "movie",
                    Path = "https://movie"
                },
                new Link
                {
                    Path = "https://gl",
                    Shortcut = "google"
                },
            });
            _parser.TryParse(new[] {key}, out List<Result> results).Should().BeTrue();
            results.Should().HaveCount(3);

            results[0].Title.Should().Be("Delete 'Ad1' link");
            results[0].SubTitle.Should().Be("https://ad1");

            results[1].Title.Should().Be("Delete 'google' link");
            results[1].SubTitle.Should().Be("https://gl");

            results[2].Title.Should().Be("Delete 'movie' link");
            results[2].SubTitle.Should().Be("https://movie");
            
            results[1].Action(new ActionContext());
            _storage.Received(1).Delete("google");
        }

        [Fact]
        public void DeleteWithShortcut_ReturnLinksMatchingKeyWork()
        {
            _storage.GetShortcuts().Returns(new[]
            {
                new Link
                {
                    Shortcut = "Ad1",
                    Path = "https://ad1"
                },
                new Link
                {
                    Shortcut = "movie",
                    Path = "https://movie"
                },
                new Link
                {
                    Shortcut = "Movart",
                    Path = "https://gl"
                },
            });
            _parser.TryParse(new[] {"-d", "mov"}, out List<Result> results).Should().BeTrue();
            results.Should().HaveCount(2);

            results[1].Title.Should().Be("Delete 'movie' link");
            results[1].SubTitle.Should().Be("https://movie");

            results[0].Title.Should().Be("Delete 'Movart' link");
            results[0].SubTitle.Should().Be("https://gl");
            
            results[1].Action(new ActionContext());
            _storage.Received(1).Delete("movie");
        }

        [Theory]
        [InlineData("--del")]
        [InlineData("-remo")]
        [InlineData("-re")]
        public void NotSaveKeyWord_ReturnFalse(string key)
        {
            _parser.TryParse(new[] {key, "https://some.com/link", "Shortcut"}, out List<Result> results).Should()
                .BeFalse();
            results.Should().HaveCount(0);
        }
    }
}