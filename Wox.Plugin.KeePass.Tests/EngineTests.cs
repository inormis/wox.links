using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Wox.Plugin;
using Wox.Plugins.KeePass.Parsers;
using Xunit;

namespace Wox.Plugins.KeePass.Tests {
    public class EngineTests {
        public EngineTests() {
            _parser = Substitute.For<ISetKeePassPathParser>();
            _storage = Substitute.For<IStorage>();
            _storage.KeePassPathIsConfigured.Returns(true);
            _configuration = Substitute.For<IConfiguration>();
            _engine = new Engine(_configuration, _storage, Substitute.For<IOpenKeePassParser>(), _parser);
        }

        private readonly Engine _engine;

        private readonly ISetKeePassPathParser _parser;

        private readonly Query _query = new Query {
            Terms = new[] {"kee", "https://jira.com", "jj"}
        };

        private readonly IStorage _storage;
        private readonly IConfiguration _configuration;

        [Theory]
        [InlineData("dd")]
        [InlineData("kk")]
        [InlineData("")]
        public void InvalidKeyword_RetrunEmptyResult(string key) {
            _engine.Execute(new Query {Terms = new[] {key}})
                .Should().BeEmpty();
        }

        [Fact]
        public void NoParserFound_ReturnEmptyResult() {
            _engine.Execute(_query).Should().BeEmpty();
        }

        [Fact]
        public void ReturnResultFromParser() {
            var expectedResult = new List<Result> {
                new Result {Title = "Two"},
                new Result {Title = "Ten"}
            };

            _parser.TryParse(Arg.Any<string>(), out _)
                .Returns(x => {
                    x[1] = expectedResult;
                    return true;
                });
            var actualResults = _engine.Execute(_query);

            actualResults.Should().BeEquivalentTo(expectedResult);
        }
    }
}