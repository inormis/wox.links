using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Links.Services;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests.Parsers {
    public class GetLinkParserTests {
        public GetLinkParserTests() {
            _storage = Substitute.For<IStorage>();
            _linkProcess = Substitute.For<ILinkProcess>();
            _saveParser = new GetLinkParser(_storage, _linkProcess);
        }

        private readonly IStorage _storage;
        private readonly GetLinkParser _saveParser;

        private readonly Link[] _links = {
            new Link {
                Shortcut = "Shortcut",
                Path = "https://some.com/do"
            },
            new Link {
                Shortcut = "Google",
                Path = "https://google.com/action"
            },
            new Link {
                Shortcut = "Austriacut",
                Path = "https://austria.com/"
            },
            new Link {
                Shortcut = "Jira",
                Path = "https://some.com/idpf-@@"
            }
        };

        private readonly ILinkProcess _linkProcess;
        
        [Fact]
        public void MatchByName_ReturnFullUrl() {
            _storage.GetShortcuts().Returns(_links);

            _saveParser.TryParse(new[] {"cut"}, out var results).Should()
                .BeTrue();
            results.Should().HaveCount(2);

            results[0].Title.Should().Be("Shortcut");
            results[0].SubTitle.Should().Be("https://some.com/do");

            results[1].Title.Should().Be("Austriacut");
            results[1].SubTitle.Should().Be("https://austria.com/");
        }

        [Fact]
        public void MatchByNameWithReplacement_ReturnFullUrl() {
            _storage.GetShortcuts().Returns(new[] {
                new Link {
                    Shortcut = "Shortcut",
                    Path = "https://jira.com/STF-@@",
                    Description = "Open IDPF-@@ ticket"
                }
            });

            _saveParser.TryParse(new[] {"cut", "8700"}, out var results)
                .Should().BeTrue();


            results[0].Title.Should().Be("Open IDPF-8700 ticket");
            results[0].SubTitle.Should().Be("https://jira.com/STF-8700");
            results[0].Action(new ActionContext()).Should().BeTrue();
        }

        [Fact]
        public void MatchByNameWithReplacement_ReturnFullUrlFalse() {
            _storage.GetShortcuts().Returns(new[] {
                new Link {
                    Shortcut = "Shortcut",
                    Path = "https://jira.com/STF-@@",
                    Description = "Open IDPF-@@ ticket"
                }
            });

            _saveParser.TryParse(new[] {"cut"}, out var results)
                .Should().BeTrue();


            results[0].Title.Should().Be("Open IDPF-{Parameter is missing} ticket");
            results[0].SubTitle.Should().Be("https://jira.com/STF-{Parameter is missing}");
            results[0].Action(new ActionContext()).Should().BeFalse();
        }
    }
}