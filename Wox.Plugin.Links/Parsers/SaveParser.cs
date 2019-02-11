using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public class SaveParser : IParser {
        private static readonly Regex linkMatch = new Regex(@"^link\b|^-l\b", RegexOptions.IgnoreCase);

        private readonly IStorage _storage;

        public SaveParser(IStorage storage) {
            _storage = storage;
        }

        public bool TryParse(Query query, out List<Result> results) {
            results = new List<Result>();
            if (query.Terms.Length < 3) {
                return false;
            }

            if (!linkMatch.IsMatch(query.Terms[0])) {
                return false;
            }

            var shortcut = query.Terms[1];
            var linkPath = query.Terms[2];
            var description = query.Terms.Length > 2 ? string.Join(" ", query.Terms.Skip(3)) : "";

            results.Add(CreateResult(shortcut, linkPath, description));
            return true;
        }

        private Result CreateResult(string shortCut, string linkPath, string description) {
            Debug.WriteLine(shortCut + " => " + description);

            return new Result {
                Title = $"Save the link as \'{shortCut}\': \'{description}\'",
                SubTitle = linkPath,
                Action = context => {
                    _storage.Set(shortCut, linkPath, description);
                    return true;
                }
            };
        }
    }
}