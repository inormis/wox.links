using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public abstract class BaseCommandParser : BaseParser {
        private readonly Regex CommandMatch;

        public override ParserPriority Priority { get; } = ParserPriority.High;

        public BaseCommandParser(string commandKey) {
            CommandMatch = new Regex($@"^{commandKey}\b|^-{commandKey[0]}\b", RegexOptions.IgnoreCase);
        }

        public sealed override bool TryParse(IQuery query, out List<Result> results) {
            results = null;
            if (query.Terms.Length < 2) {
                return false;
            }

            if (IsLinkKeyword(query.Terms[0]) && CommandMatch.IsMatch(query.Terms[1])) {
                results = Execute(query.Terms.Skip(2).ToArray());
                return true;
            }

            return false;
        }

        protected List<Result> GetErrorResult(string message, string subTitle = null) {
            return new List<Result> {
                new Result {
                    Title = message,
                    SubTitle = subTitle,
                    Action = context => false
                }
            };
        }

        protected abstract List<Result> Execute(string[] termsArguments);
    }
}