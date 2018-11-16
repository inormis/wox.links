using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Links
{
    internal interface IEngine
    {
        IEnumerable<Result> Execute(Query query);
    }
}