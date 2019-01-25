using System.Collections.Generic;
using System.Linq;
using Wox.Links.Services;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Links.Parsers {
    public class ImportConfigParser : IParser {
        private readonly ILinkProcess _linkProcess;
        private readonly IStorage _storage;

        public ImportConfigParser(IStorage storage, ILinkProcess linkProcess) {
            _linkProcess = linkProcess;
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results) {
            results = new List<Result>();

            if (terms.Length == 0) {
                return false;
            }

            var key = terms.First();
            var links = _storage.GetShortcuts().Where(x => x.Shortcut.MatchShortcut(key)).ToArray();

            results.AddRange(links.Select(link => {
                var args = terms.Skip(1).ToArray();
                return Create(link, args.FirstOrDefault());
            }));
            return true;
        }

        private Result Create(Link x, string arg) {
            var url = Format(x.Path, arg);
            var canOpenLink = CanOpenLink(url);
            var description = string.IsNullOrEmpty(x.Description) ? "" : FormatDescription(x.Description, arg);
            return new Result {
                Title = $"Import configuration file and replace current",
                SubTitle = "Existing configuration will be deleted",
                Action = context => {
                    if (canOpenLink) {
                        _linkProcess.Open(url);
                    }

                    return canOpenLink;
                }
            };
        }

        private static string Format(string format, string arg) {
            if (string.IsNullOrWhiteSpace(arg) && format.Contains("@@")) {
                return format;
            }

            return format?.Replace("@@", arg);
        }

        private static string FormatDescription(string format, string arg) {
            if (string.IsNullOrWhiteSpace(arg)) {
                arg = "{Parameter is missing}";
            }

            return format?.Replace("@@", arg);
        }

        private static bool CanOpenLink(string url) {
            return !url.Contains("@@");
        }
    }
}