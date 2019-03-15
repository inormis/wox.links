using System.Collections.Generic;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    public interface IEngine {
        IEnumerable<Result> Execute(IQuery query);
    }
}