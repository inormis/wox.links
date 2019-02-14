using System.Collections.Generic;
using Wox.Plugin.Links.Parsers;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    public class Engine : IEngine {
        private readonly IEnumerable<IParser> _parsers;

        public Engine(ImportParser importParser, 
            ExportParser exportParser, 
            DeleteParser deleteParser,
            GetLinkParser getLinkParser, 
            SaveParser saveParser) {
            _parsers = new IParser[] {
                importParser,
                exportParser,
                deleteParser,
                saveParser,
                getLinkParser
            };
        }

        public IEnumerable<Result> Execute(IQuery query) {
            foreach (var parser in _parsers) {
                if (!parser.TryParse(query, out var results)) {
                    continue;
                }

                foreach (var result in results) {
                    yield return result;
                }

                yield break;
            }
        }
    }
}