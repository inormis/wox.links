using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Wox.Links.Parsers;
using Wox.Plugin;
using Xunit;

namespace Wox.Links.Tests {
    public class EngineTests {
        public EngineTests() {
            _parser = Substitute.For<IParser>();
            _engine = new Engine(new[] {_parser});
        }

        private readonly Engine _engine;

        private readonly IParser _parser;

        private readonly Query _query = new Query {
            Terms = new[] {"-save", "https://jira.com", "jj"}
        };

        [Fact]
        public void NoParserFound_ReturnEmptyResult() {
            _engine.Execute(_query).Should().BeEmpty();
        }

        [Fact]
        public void ReturnResultFromParser() {
            var expectedResult = new List<Result> {
                new Result{Title = "Two"},
                new Result{Title = "Ten"}
            };

            _parser.TryParse(_query.Terms, out _)
                .Returns(x => {
                    x[1] = expectedResult;
                    return true;
                });
            var actualResults = _engine.Execute(_query);

            actualResults.Should().BeEquivalentTo(expectedResult);
        }
    }
}