using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public class SaveParser : IParser {
        private static readonly Regex SaveMatch = new Regex(@"--save\b|-s\b", RegexOptions.IgnoreCase);

        private readonly IStorage _storage;

        public SaveParser(IStorage storage) {
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results) {
            results = new List<Result>();
            if (terms.Length < 3) {
                return false;
            }

            var saveKeyWord = terms.FirstOrDefault(x => SaveMatch.IsMatch(x));
            if (saveKeyWord == null)
                return false;

            var shortcut = terms[1];
            var linkPath = terms[2];
            var description = terms.Length > 2 ? terms[3] : "";

            results.Add(CreateResult(shortcut, linkPath, description));
            return true;
        }

        private Result CreateResult(string shortCut, string linkPath, string description) {
            return new Result {
                Title = $"Save the link as \'{shortCut}\'",
                SubTitle = linkPath,
                Action = context => {
                    _storage.Set(shortCut, linkPath, description);
                    return true;
                }
            };
        }
    }
}