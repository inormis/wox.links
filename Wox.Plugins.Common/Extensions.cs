using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace Wox.Plugins.Common {
    public static class Extensions {
        public static bool ContainsCaseInsensitive(this string value, string part) {
            if (string.IsNullOrWhiteSpace(part)) {
                return false;
            }

            return value.IndexOf(part, StringComparison.InvariantCultureIgnoreCase) != -1;
        }

        public static bool NotEmpty(this string value) {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsEmpty(this string value) {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool MatchShortcut(this string value, string input) {
            if (string.IsNullOrWhiteSpace(input)) {
                return false;
            }

            if (value.ContainsCaseInsensitive(input)) {
                return true;
            }

            var inputs = input.SplitCamelCase();
            if (inputs.Length < 2) {
                return false;
            }

            var lastIndex = -1;
            foreach (var part in inputs) {
                var index = value.IndexOf(part);
                if (index <= lastIndex) {
                    return false;
                }

                lastIndex = index;
            }

            return true;
        }

        private static string[] SplitCamelCase(this string source) {
            return Regex.Split(source, @"(?<!^)(?=[A-Z])");
        }
    }
}