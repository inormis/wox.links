using System.Collections.Generic;

namespace Wox.Plugin.Keepass {
    internal interface IEngine {
        IEnumerable<Result> Execute(Query query);
    }
}