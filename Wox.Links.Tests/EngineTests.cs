using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
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
}