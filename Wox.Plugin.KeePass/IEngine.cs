using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Plugins.KeePass {
    internal interface IEngine {
        IEnumerable<Result> Execute(Query query);
    }
}