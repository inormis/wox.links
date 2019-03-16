using System;
using Wox.Plugin;

namespace Wox.Plugins.Common {
    public class QueryInstance : IQuery {
        public QueryInstance(Query query)
            : this(query.FirstSearch, query.SecondToEndSearch) {
        }

        public QueryInstance(string search, string rawArgument) {
            Search = search;
            RawArgument = rawArgument;
            Arguments = rawArgument.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        public string Search { get; }
        public string[] Arguments { get; }
        public string RawArgument { get; }
    }
}