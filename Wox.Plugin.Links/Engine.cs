using System.Collections.Generic;
using System.Linq;
using Wox.Plugin.Links.Parsers;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    public class Engine : IEngine {
        private readonly IEnumerable<IParser> _parsers;

        public Engine(IEnumerable<IParser> parsers) {
            _parsers = parsers.OrderByDescending(x=>x.Priority);
        }

        public IEnumerable<Result> Execute(IQuery query) {
            foreach (var parser in _parsers) {
                if (parser.TryParse(query, out var results)) {
                    foreach (var result in results) {
                        yield return result;
                    }
                    yield break;
                }
            }
        }
    }
}