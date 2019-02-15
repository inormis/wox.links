using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public class DeleteParser : BaseCommandParser {
        private readonly IStorage _storage;

        public DeleteParser(IStorage storage) : base("delete") {
            _storage = storage;
        }

        protected override List<Result> Execute(string[] termsArguments) {
            if (termsArguments.Length >=2) {
                return GetErrorResult("Only one argument has to be specified");
            }

            var arg = termsArguments.SingleOrDefault();

            var predicate = string.IsNullOrWhiteSpace(arg)
                ? (Func<string, bool>) (s => true)
                : s=> s.ContainsCaseInsensitive(arg);
            return _storage.GetShortcuts()
                .Where(link => predicate(link.Shortcut))
                .Select(x => new Result {
                    Title = x.Shortcut,
                    SubTitle = $"DELETE shortcut [{x.Shortcut}]",
                    Action = context => {
                        _storage.Delete(x.Shortcut);
                        return true;
                    }
                })
                .OrderBy(x=>x.Title)
                .ToList();
        }
    }
}