using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public interface IParser {
        bool TryParse(Query query, out List<Result> results);
    }
}