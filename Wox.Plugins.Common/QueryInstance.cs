using Wox.Plugin;

namespace Wox.Plugins.Common {
    public class QueryInstance : IQuery {
        public QueryInstance(Query query) {
            Search = query.Search;
            Terms = query.Terms;
        }

        public QueryInstance(string search, string[] terms) {
            Search = search;
            Terms = terms;
        }

        public string Search { get; }
        public string[] Terms { get; }
    }
}