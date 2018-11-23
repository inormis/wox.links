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

            results.AddRange(links.Select(link => {
                string[] args = terms.Skip(1).ToArray();
                return Create(link, args.FirstOrDefault());
            }));
            return true;
        }

        private Result Create(Link x, string arg) {
            var url = Format(x.Path, arg);
            return new Result {
                Title = string.IsNullOrEmpty(x.Description) ? x.Shortcut : FormatDescription(x.Description, arg),
                SubTitle = FormatDescription(x.Path, arg),
                Action = context => {
                    _linkProcess.Open(url);
                    return CanOpenLink(url);
                }
            };
        }

        private static string Format(string format, string arg) {
            if (string.IsNullOrWhiteSpace(arg) && format.Contains("@@"))
                return format;
            return format?.Replace("@@", arg);
        }
        private static string FormatDescription(string format, string arg) {
            if (string.IsNullOrWhiteSpace(arg))
                arg = "{Parameter is missing}";
            return format?.Replace("@@", arg);
        }

        private static bool CanOpenLink(string url) {
            return !url.Contains("@@");
        }
    }
}