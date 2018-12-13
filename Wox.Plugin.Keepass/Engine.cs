using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Wox.Plugin.Keepass.Parsers;

namespace Wox.Plugin.Keepass {
    public class Engine : IEngine {
        private readonly IEnumerable<IParser> _parsers;
        private IConfiguration _configuration;
        private readonly IStorage _storage;

        public Engine(IEnumerable<IParser> parsers, IConfiguration configuration, IStorage storage) {
            _configuration = configuration;
            _storage = storage;
            _parsers = parsers;
            configuration.Initialize();
        }

        public IEnumerable<Result> Execute(Query query) {
            if(!_storage.KeepassPathIsConfigured) {
                yield return new Result {
                    Title = "Keepass executable not found"
                };
                yield break;

            }
            var terms = query.Terms;
            foreach (var parser in _parsers) {
                if (parser.TryParse(terms, out var results)) {
                    foreach (var result in results) {
                        yield return result;
                    }
                }
            }
        }
    }

    public interface IConfiguration {
        void Initialize();
    }

    class Configuration : IConfiguration {
        private const string FileName = "KeePass.exe";
        private IStorage _storage;


        public Configuration(IStorage storage) {
            _storage = storage;
        }

        public void Initialize() {
            if (File.Exists(_storage.KeePassPath))
                return;

            Task.Run((Action) SearchKeePass);
        }

        private void SearchKeePass() {
            SearchIn(5, Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles));
        }

        private void SearchIn(int depth, params string[] directories) {
            if (_storage.KeepassPathIsConfigured)
                return;
            foreach (var directory in directories) {
                var files = Directory.GetFiles(directory);
                var path = files.FirstOrDefault(x => x.EndsWith(FileName, StringComparison.InvariantCultureIgnoreCase));
                if (path != null) {
                    _storage.SetKeepassPath(path);
                    return;
                }
            }

            if (depth == 0)
                return;

            foreach (var directory in directories) {
                SearchIn(depth - 1, Directory.GetDirectories(directory));
            }
        }
    }
}