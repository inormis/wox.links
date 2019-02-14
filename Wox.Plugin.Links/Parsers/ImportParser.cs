using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public class ImportParser : IParser {
        private static readonly Regex SaveMatch = new Regex(@"^link\b|^-l\b", RegexOptions.IgnoreCase);
        private readonly IFileService _fileService;

        private readonly IStorage _storage;

        public ImportParser(IStorage storage, IFileService fileService) {
            _fileService = fileService;
            _storage = storage;
        }

        public bool TryParse(IQuery query, out List<Result> results) {
            results = new List<Result>();
            var indexOf = query.Search.IndexOf(' ');
            if (indexOf == -1) {
                return false;
            }

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
                Title = $"Import configuration file {Path.GetFileName(jsonPath)} and replace current",
                SubTitle = "Existing configuration will be deleted",
                Action = context => _storage.TryImport(jsonPath)
            };
        }
    }
}