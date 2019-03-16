using System;
using System.Collections.Generic;
using System.IO;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links.Parsers {
    public class ImportParser : BaseParser {
        private readonly IFileService _fileService;

        private readonly IStorage _storage;

        public ImportParser(IStorage storage, IFileService fileService) : base("link") {
            _fileService = fileService;
            _storage = storage;
        }

        protected override bool CustomParse(IQuery query) {
            return _fileService.Exists(query.RawArgument) &&
                   string.Compare(_fileService.GetExtension(query.RawArgument),".json",StringComparison.InvariantCultureIgnoreCase)==0;
        }

        protected override List<Result> Execute(IQuery query) {
            var results = new List<Result> {Create(query.RawArgument)};
            return results;
        }

        public override ParserPriority Priority { get; } = ParserPriority.High;

        private Result Create(string jsonPath) {
            return new Result {
                Title = $"Import configuration file '{Path.GetFileName(jsonPath)}' and replace current",
                SubTitle = "Existing configuration will be deleted",
                Action = context => _storage.TryImport(jsonPath)
            };
        }
    }
}