using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests
{
    public class EngineTests
    {
        private Engine _engine;

        private IParser _parser;

        private Query _query = new Query
        {
            Terms = new[] {"-save", "https://jira.com", "jj"}
        };

        public EngineTests()
        {
            _parser = Substitute.For<IParser>();
            _engine = new Engine(new[] {_parser});
        }

        [Fact]
        public void NoParserFound_ReturnEmptyResult()
        {
            _engine.Execute(_query).Should().BeEmpty();
        }
        [Fact]
        public void ReturnResultFromParser()
        {
            var expectedResult = new List<Result>();

            _parser.TryParse(_query.Terms, out List<Result> any)
                .Returns(x =>
                {
                    x[1] = expectedResult;
                    return true;
                });
            var actualResults = _engine.Execute(_query);

            actualResults.Should().BeSameAs(expectedResult);
        }
    }

    public class SaveParserTests
    {
        private IStorage _storage;

        public SaveParserTests()
        {
            _storage = Substitute.For<IStorage>();
        }
        [Fact]
        public void SaveTerms_ReturnTrueAndProposeToSave()
        {
            var saveParser = new SaveParser(_storage);

            saveParser.TryParse(new[] {"--save", "https://some.com/link", "Shortcut"}, out List<Result> results).Should().BeTrue();
            results.Should().HaveCount(1);

            var result = results[0];
            result.Title.Should().Be("Save the link as 'Shortcut'");
            result.SubTitle.Should().Be("https://some.com/link");

            result.Action(new ActionContext());
            _storage.Received(1).Add("Shortcut", "https://some.com/link");
        }
    }
}