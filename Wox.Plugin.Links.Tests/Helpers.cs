using Wox.Plugins.Common;

namespace Wox.Links.Tests {
    public static class Helpers {
        public static IQuery CreateQuery(this string search, string rawArgument = "") {
            return new QueryInstance(search, rawArgument);
        }
    }
}