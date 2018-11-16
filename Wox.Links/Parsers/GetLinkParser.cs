using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Links.Extensions;
using Wox.Links.Services;
using Wox.Plugin;

namespace Wox.Links.Parsers {
    public class GetLinkParser : IParser {
        private readonly ILinkProcess _linkProcess;
        private readonly IStorage _storage;

        public GetLinkParser(IStorage storage, ILinkProcess linkProcess) {
            _linkProcess = linkProcess;
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results) {
            results = new List<Result>();

            if (terms.Length == 0) {
                return false;
            }

            var key = terms.First();
            var links = _storage.GetShortcuts().Where(x => x.Shortcut.ContainsCaseInsensitive(key)).ToArray();

            results.AddRange(links.Select(link => Create(link, terms.Skip(1).ToArray())));
            return true;
        }

        private Result Create(Link x, string[] args) {
            var url = args.Length == 0 ? x.Path : string.Format(x.Path, args);
            return new Result {
                Title = url,
                Action = context => {
                    _linkProcess.Open(url);
                    return CanOpenLink(url);
                }
            };
        }

        private static bool CanOpenLink(string url) {
            return !Regex.IsMatch(url, @"\{\d\}");
        }
    }
}