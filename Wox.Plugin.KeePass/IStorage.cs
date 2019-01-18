using System.IO;
using Newtonsoft.Json;
using Wox.Plugin;

namespace Wox.Plugins.KeePass {
    public interface IStorage {
        bool KeePassPathIsConfigured { get; }

        IKeePath KeePath { get; }
        void SetKeePassPath(string path);
        void SetApplicationPath(string path);
    }

    internal class Storage : IStorage {
        private readonly PluginInitContext _context;
        private readonly string _directory;

        public Storage(PluginInitContext context) {
            _context = context;
            _directory = _context.CurrentPluginMetadata?.PluginDirectory;

            KeePath = Load();
        }

        public string ConfigPath => Path.Combine(_directory ?? "", "keepas.config.json");
        public IKeePath KeePath { get; }

        public bool KeePassPathIsConfigured =>
            File.Exists(KeePath.ApplicationPath) && File.Exists(KeePath.KeePassFilePath);

        public void SetApplicationPath(string path) {
            KeePath.ApplicationPath = path;
            Save();
        }

        public void SetKeePassPath(string path) {
            KeePath.KeePassFilePath = path;
            Save();
        }

        private KeePath Load() {
            return File.Exists(ConfigPath)
                ? JsonConvert.DeserializeObject<KeePath>(File.ReadAllText(ConfigPath))
                : new KeePath();
        }

        private void Save() {
            var content = JsonConvert.SerializeObject(KeePath, Formatting.Indented);
            File.WriteAllText(ConfigPath, content);
        }
    }
}