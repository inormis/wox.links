using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace Wox.Links.Parsers
{
    public class SaveParser : IParser
    {
        static readonly Regex SaveMatch = new Regex(@"--save\b|-s\b", RegexOptions.IgnoreCase);

        static readonly Regex LinkMatch =
            new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

        private readonly IStorage _storage;

        public SaveParser(IStorage storage)
        {
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results)
        {
            results = new List<Result>();
            if (terms.Length < 3)
                return false;

            var saveKeyWord = terms.FirstOrDefault(x => SaveMatch.IsMatch(x));
            var linkWord = terms.FirstOrDefault(x => LinkMatch.IsMatch(x));

            if (saveKeyWord == null || linkWord == null)
                return false;

            var shortCut = terms.First(x => x != saveKeyWord && x != linkWord);
            results.Add(CreateResult(shortCut, linkWord));
            return true;
        }

        private Result CreateResult(string shortCut, string linkWord)
        {
            return new Result
            {
                Title = $"Save the link as \'{shortCut}\'",
                SubTitle = linkWord,
                Action = context =>
                {
                    _storage.Set(shortCut, linkWord);
                    return true;
                }
            };
        }
    }
}