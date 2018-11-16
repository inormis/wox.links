using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Links.Services;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests
{
    public class GetLinkParserTests
    {
        private readonly IStorage _storage;
        private readonly GetLinkParser _saveParser;

        private Link[] _links = new[]
        {
            new Link
            {
                Shortcut = "Shortcut",
                Path = "https://some.com/do"
            },
            new Link
            {
                Shortcut = "Google",
                Path = "https://google.com/action"
            },
            new Link
            {
                Shortcut = "Austriacut",
                Path = "https://some.com/do"
            }
        };

        private ILinkProcess _linkProcess;

        public GetLinkParserTests()
        {
            _storage = Substitute.For<IStorage>();
            _linkProcess = Substitute.For<ILinkProcess>();
            _saveParser = new GetLinkParser(_storage, _linkProcess);
        }

        [Fact]
        public void MatchByName_ReturnFullUrl()
        {
            _storage.GetShortcuts().Returns(_links);

            _saveParser.TryParse(new[] {"cut"}, out List<Result> results).Should()
                .BeTrue();
            results.Should().HaveCount(2);


            results[0].Title.Should().Be("https://some.com/do");
            results[0].SubTitle.Should().BeNull();

            results[1].Title.Should().Be("https://some.com/do");
            results[1].SubTitle.Should().BeNull();

            results[1].Action(new ActionContext()).Should().BeTrue();
            _linkProcess.Received(1).Open("https://some.com/do");
        }

        [Fact]
        public void MatchByNameWithReplacement_ReturnFullUrlFalse()
        {
            _storage.GetShortcuts().Returns(new[]
            {
                new Link
                {
                    Shortcut = "Shortcut",
                    Path = "https://jira.com/STF-{0}"
                }
            });

            _saveParser.TryParse(new[] {"cut"}, out List<Result> results)
                .Should().BeTrue();


            results[0].Title.Should().Be("https://jira.com/STF-{0}");
            results[0].SubTitle.Should().BeNull();
            results[0].Action(new ActionContext()).Should().BeFalse();
        }

        [Fact]
        public void MatchByNameWithReplacement_ReturnFullUrl()
        {
            _storage.GetShortcuts().Returns(new[]
            {
                new Link
                {
                    Shortcut = "Shortcut",
                    Path = "https://jira.com/STF-{0}"
                }
            });

            _saveParser.TryParse(new[] {"cut", "1332"}, out List<Result> results)
                .Should().BeTrue();
            results.Should().HaveCount(1);

            results[0].Title.Should().Be("https://jira.com/STF-1332");
            results[0].SubTitle.Should().BeNull();
        }
    }
}