using System.Collections.Generic;
using System.Text.RegularExpressions;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public abstract class BaseParser : IParser {
        protected const string PluginKey = "link";
        private static readonly Regex LinkRegex = new Regex($@"^{PluginKey}\b", RegexOptions.IgnoreCase);
        public abstract bool TryParse(IQuery query, out List<Result> results);
        public virtual ParserPriority Priority { get; } = ParserPriority.Normal;

        protected bool IsLinkKeyword(string key) {
            return LinkRegex.IsMatch(key);
        }
    }
}