using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Plugins.KeePass.Parsers {
    public interface IParser {
        bool TryParse(string query, out List<Result> results);
    }
}