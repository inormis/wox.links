using System.Collections.Generic;
using Wox.Links.Parsers;
using Wox.Plugin;

namespace Wox.Links
{
    public class Engine : IEngine
    {
        private IEnumerable<IParser> _parsers;

        public Engine(IEnumerable<IParser> parsers)
        {
            _parsers = parsers;
        }

        public List<Result> Execute(Query query)
        {
            var terms = query.Terms;
            foreach (var parser in _parsers)
            {
                if (parser.TryParse(terms, out List<Result> results))
                    return results;
            }

            return new List<Result>();
        }
    }
}