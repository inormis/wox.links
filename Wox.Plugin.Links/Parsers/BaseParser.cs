using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public abstract class BaseParser : IParser {
        protected const string PluginKey = "link";
        
        private static readonly Regex LinkRegex = new Regex($@"^{PluginKey}\b", RegexOptions.IgnoreCase);
        public virtual ParserPriority Priority { get; } = ParserPriority.Normal;

//        protected bool IsLinkKeyword(string key) {
//            return LinkRegex.IsMatch(key);
//        }
        
        private readonly Regex CommandMatch;

        public BaseParser(string commandKey) {
            CommandMatch = new Regex($@"^--{commandKey}\b|^{commandKey}\b|^-{commandKey[0]}\b", RegexOptions.IgnoreCase);
        }

        public virtual bool TryParse(IQuery query, out List<Result> results) {
            results = new List<Result>();

            if (CommandMatch.IsMatch(query.Search) && CustomParse(query)) {
                results = Execute(query);
                return true;
            }
            
            return false;
        }

        protected virtual bool CustomParse(IQuery query) {
            return true;
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

        protected abstract List<Result> Execute(IQuery query);
    }
}