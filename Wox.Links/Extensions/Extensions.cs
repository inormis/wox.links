using System;

namespace Wox.Links.Extensions
{
    public static class Extensions
    {
        public static bool ContainsCaseInsensitive(this string value, string part)
        {
            return value.IndexOf(part, StringComparison.InvariantCultureIgnoreCase) != -1;
        }
    }
}