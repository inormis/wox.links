using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugins.KeePass.Parsers;

namespace Wox.Plugins.KeePass {
    public class Engine : IEngine {
        private readonly IOpenKeePassParser _openKeePassParser;
        private readonly ISetKeePassPathParser _setKeePassPathParser;

        public Engine(IConfiguration configuration,
            IOpenKeePassParser openKeePassParser, 
            ISetKeePassPathParser setKeePassPathParser) {
            _openKeePassParser = openKeePassParser;
            _setKeePassPathParser = setKeePassPathParser;
            configuration.Initialize();
        }

        public IEnumerable<Result> Execute(Query query) {
            var _parsers = GetParsers();

            var terms = query.Search;
            foreach (var parser in _parsers) {
                if (parser.TryParse(terms, out var results)) {
                    return results;
                }
            }

            return new Result[0];
        }

        private IEnumerable<IParser> GetParsers() {
            yield return _setKeePassPathParser;
            yield return _openKeePassParser;
        }
    }
}