using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public interface IParser {
        bool TryParse(string[] terms, out List<Result> results);
    }
}