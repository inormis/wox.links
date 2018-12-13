using System.Collections.Generic;

namespace Wox.Plugin.Keepass.Parsers {
    public interface IParser {
        bool TryParse(string[] terms, out List<Result> results);
    }
}