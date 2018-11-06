using System.Collections.Generic;
using Wox.Plugin;

namespace Wox.Links
{
    internal interface IEngine
    {
        List<Result> Execute(Query query);
    }
}