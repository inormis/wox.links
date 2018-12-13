using System;

namespace Wox.Plugin.Keepass.Extensions {
    public static class Extensions {
        public static bool ContainsCaseInsensitive(this string value, string part) {
            return value.IndexOf(part, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
    }
}