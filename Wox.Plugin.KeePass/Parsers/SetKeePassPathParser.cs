using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Plugins.KeePass.Parsers {
    public interface ISetKeePassPathParser : IParser {
    }

    public class SetKeePassPathParser : ISetKeePassPathParser {
        private readonly IFile _file;
        private readonly IStorage _storage;

        public SetKeePassPathParser(IStorage storage, IFile file) {
            _file = file;
            _storage = storage;
        }

        public bool TryParse(string query, out List<Result> results) {
            results = new List<Result>();

            if (TryToGetFilePath(query))
            {
                var result = new Result {
                    Title = $"Set '{query}' as keepass path",
                    Action = context => {
                        _storage.SetKeePassPath(query);
                        return true;
                    }
                };
                results.Add(result);
                return true;
            }

            if (!_storage.KeePassPathIsConfigured) {
                results.Add(new Result {
                    Title = "KeePass executable or storage not found"
                });
                return true;
            }

            return false;
        }

        private bool TryToGetFilePath(string query) {
            return _file.Exists(query) && _file.CheckExtension(query, ".kdbx");
        }
    }
}