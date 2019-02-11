using System.Collections.Generic;
using System.Text.RegularExpressions;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Links.Parsers {
    public class ImportConfigParser : IParser {
        private static readonly Regex SaveMatch = new Regex(@"^link\b|^-l\b", RegexOptions.IgnoreCase);

        private readonly IStorage _storage;
        private readonly IFileService _fileService;

        public ImportConfigParser(IStorage storage, IFileService fileService) {
            _fileService = fileService;
            _storage = storage;
        }

        public bool TryParse(Query query, out List<Result> results) {
            results = new List<Result>();
            var indexOf = query.Search.IndexOf(' ');
            if (indexOf == -1)
                return false;

            var key = query.Search.Substring(0, indexOf);
            var path = query.Search.Substring(indexOf + 1);

            if (SaveMatch.IsMatch(key) && _fileService.Exists(path) &&
                _fileService.CheckExtension(path, ".json")) {
                results.Add(Create(path));
                return true;
            }

            return false;
        }

        private Result Create(string jsonPath) {
            return new Result {
                Title = "Import configuration file and replace current",
                SubTitle = "Existing configuration will be deleted",
                Action = context => { return _storage.TryImport(jsonPath); }
            };
        }
    }
}