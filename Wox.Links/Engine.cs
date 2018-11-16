using System.Collections.Generic;
using Wox.Links.Parsers;
using Wox.Plugin;

namespace Wox.Links
{
    public class Engine : IEngine
    {
        private readonly IEnumerable<IParser> _parsers;

        public Engine(IEnumerable<IParser> parsers)
        {
            _parsers = parsers;
        }

        public IEnumerable<Result> Execute(Query query)
        {
            var terms = query.Terms;
            foreach (var parser in _parsers)
            {
                if (parser.TryParse(terms, out var results))
                {
                    foreach (var result in results)
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}