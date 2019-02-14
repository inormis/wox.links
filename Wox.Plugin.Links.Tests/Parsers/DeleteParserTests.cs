using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugin.Links;
using Wox.Plugin.Links.Parsers;
using Wox.Plugins.Common;
using Xunit;

namespace Wox.Links.Tests.Parsers {
    public class DeleteParserTests {
        public DeleteParserTests() {
            _storage = Substitute.For<IStorage>();
            _parser = new DeleteParser(_storage);
        }

        private readonly IStorage _storage;
        private readonly DeleteParser _parser;

        [Theory]
        [InlineData("--delete")]
        [InlineData("--remove")]
        [InlineData("-d")]
        [InlineData("-r")]
        public void DeleteWithShortcut_ReturnAllLinks(string key) {
            _storage.GetShortcuts().Returns(new[] {
                new Link {
                    Shortcut = "Ad1",
                    Path = "https://ad1"
                },
                new Link {
                    Shortcut = "movie",
                    Path = "https://movie"
                },
                new Link {
                    Path = "https://gl",
                    Shortcut = "google"
                }
            });
            _parser.TryParse(new QueryInstance(new Query { Terms = new []{key}}), out var results).Should().BeTrue();
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

        [Theory]
        [InlineData("--del")]
        [InlineData("-remo")]
        [InlineData("-re")]
        public void NotSaveKeyWord_ReturnFalse(string key) {
            _parser.TryParse(new QueryInstance(new Query { Terms = new [] {key, "https://some.com/link", "Shortcut"}}), out var results).Should()
                .BeFalse();
            results.Should().HaveCount(0);
        }

        [Fact]
        public void DeleteWithShortcut_ReturnLinksMatchingKeyWork() {
            _storage.GetShortcuts().Returns(new[] {
                new Link {
                    Shortcut = "Ad1",
                    Path = "https://ad1"
                },
                new Link {
                    Shortcut = "movie",
                    Path = "https://movie"
                },
                new Link {
                    Shortcut = "Movart",
                    Path = "https://gl"
                }
            });
            _parser.TryParse(new QueryInstance("", new [] {"-d", "mov"}), out var results).Should().BeTrue();
            results.Should().HaveCount(2);

            results[1].Title.Should().Be("Delete 'movie' link");
            results[1].SubTitle.Should().Be("https://movie");

            results[0].Title.Should().Be("Delete 'Movart' link");
            results[0].SubTitle.Should().Be("https://gl");

            results[1].Action(new ActionContext());
            _storage.Received(1).Delete("movie");
        }
    }
}