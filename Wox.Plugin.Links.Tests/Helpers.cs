using Wox.Plugins.Common;

namespace Wox.Links.Tests {
    public static class Helpers {
        public static IQuery CreateQuery(params string[] terms) {
            return new QueryInstance("", terms);
        }
        
        
    }
}