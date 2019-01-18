using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Wox.Plugins.KeePass {
    public class Configuration : IConfiguration {
        private const string FileName = "KeePass.exe";
        private readonly IStorage _storage;


        public Configuration(IStorage storage) {
            _storage = storage;
        }

        public void Initialize() {
            if (File.Exists(_storage.KeePath.ApplicationPath)) {
                return;
            }

            Task.Run((Action) SearchKeePass);
        }

        private void SearchKeePass() {
            SearchIn(2, Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
        }

        private void SearchIn(int depth, params string[] directories) {
            if (_storage.KeePassPathIsConfigured) {
                return;
            }

            foreach (var directory in directories) {
                var files = Directory.GetFiles(directory);
                var path = files.FirstOrDefault(x => x.EndsWith(FileName, StringComparison.InvariantCultureIgnoreCase));
                if (path != null) {
                    _storage.SetApplicationPath(path);
                    return;
                }
            }

            if (depth == 0) {
                return;
            }

            foreach (var directory in directories) {
                SearchIn(depth - 1, Directory.GetDirectories(directory));
            }
        }
    }
}