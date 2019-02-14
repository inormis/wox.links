using System.Collections.Generic;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public interface IParser {
        ParserPriority Priority { get; }
        bool TryParse(IQuery query, out List<Result> results);
    }
}