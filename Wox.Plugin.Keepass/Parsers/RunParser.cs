using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wox.Plugin.Keepass.Parsers {
    public class RunParser : IParser {
        private static readonly Regex SaveMatch = new Regex(@"--kee\b|-k\b|kee", RegexOptions.IgnoreCase);

        private readonly IStorage _storage;

        public RunParser(IStorage storage) {
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results) {
            results = new List<Result>();
            if (terms.Length < 2) {
                return false;
            }

            var keyWord = terms.FirstOrDefault(x => SaveMatch.IsMatch(x));
            if (keyWord == null)
                return false;

//            var password=
//            var shortcut = terms[1];
//            var linkPath = terms[2];
//            var description = terms.Length > 2 ? string.Join(" ", terms.Skip(3)) : "";
//
//            results.Add(CreateResult(shortcut, linkPath, description));
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