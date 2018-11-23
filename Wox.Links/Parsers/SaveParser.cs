using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public class SaveParser : IParser {
        private static readonly Regex SaveMatch = new Regex(@"--save\b|-s\b", RegexOptions.IgnoreCase);

        private static readonly Regex LinkMatch =
            new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

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
            var linkPath = terms.FirstOrDefault(x => LinkMatch.IsMatch(x));

            if (saveKeyWord == null || linkPath == null) {
                return false;
            }

            var shortCut = terms.First(x => x != saveKeyWord && x != linkPath);
            results.Add(CreateResult(shortCut, linkPath));
            return true;
        }

        private Result CreateResult(string shortCut, string linkPath) {
            return new Result {
                Title = $"Save the link as \'{shortCut}\'",
                SubTitle = linkPath,
                Action = context => {
                    _storage.Set(shortCut, linkPath);
                    return true;
                }
            };
        }
    }
}