using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Links.Extensions;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public class DeleteParser : IParser {
        private static readonly Regex Match = new Regex(@"--delete\b|--remove\b|-d\b|-r\b", RegexOptions.IgnoreCase);

        private readonly IStorage _storage;

        public DeleteParser(IStorage storage) {
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results) {
            results = new List<Result>();
            if (terms.Length > 2) {
                return false;
            }

            var keyWord = terms.FirstOrDefault(x => Match.IsMatch(x));
            if (keyWord == null) {
                return false;
            }

            var shortcut = terms.SingleOrDefault(x => x != keyWord);
            var links = _storage.GetShortcuts();
            results.AddRange(GetResults(links, shortcut));
            return true;
        }

        private Result[] GetResults(Link[] links, string shortcut) {
            return links
                .Where(x => shortcut == null ||
                            x.Shortcut.ContainsCaseInsensitive(shortcut) ||
                            x.Path.ContainsCaseInsensitive(shortcut))
                .Select(x => new Result {
                    Title = "Delete '" + x.Shortcut + "' link",
                    SubTitle = x.Path,
                    Action = context => {
                        _storage.Delete(x.Shortcut);
                        return true;
                    }
                })
                .OrderBy(x => x.Title)
                .ToArray();
        }
    }
}