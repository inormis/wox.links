using System;
using System.Collections.Generic;
using System.Linq;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public class RenameParser : BaseCommandParser {
        private readonly IStorage _storage;

        public RenameParser(IStorage storage) : base("rename") {
            _storage = storage;
        }

        protected override List<Result> Execute(string[] termsArguments) {
            if (termsArguments.Length == 0) {
                return GetErrorResult("Pass the new name of shortcut");
            }

            var newName = termsArguments.First();

            if (termsArguments.Length == 1) {
                return GetErrorResult("Pass a name of the link to be updated");
            }

            if (termsArguments.Length > 2) {
                return GetErrorResult("One or two arguments have to be specified argument has to be specified");
            }

            if (_storage.TryGetByShortcut(newName, out Link existingLink))
                return GetErrorResult($"Shortcut [{newName}] already exists", existingLink.Description);
            
            var existingShortCut = termsArguments[1];
            
            var predicate = string.IsNullOrWhiteSpace(existingShortCut)
                ? (Func<string, bool>) (s => true)
                : s=> s.ContainsCaseInsensitive(existingShortCut);


            var results = _storage.GetShortcuts()
                .Where(link => predicate(link.Shortcut))
                .Select(x => new Result {
                    Title = $"Rename shortcut [{x.Shortcut}] to => [{newName}]",
                    SubTitle = $"Description: {x.Description}",
                    Action = context => {
                        _storage.Rename(x.Shortcut, newName);
                        return true;
                    }
                })
                .OrderBy(x=>x.Title)
                .ToList();
            return results.Any()
                ? results
                : GetErrorResult("No shortcuts were found matching your query");
        }
    }
}