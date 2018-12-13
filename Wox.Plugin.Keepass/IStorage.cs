using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Wox.Plugin.Keepass {
    public interface IStorage {
        void Set(string shortcut, string url, string description);

        Link[] GetShortcuts();

        void Delete(string shortcut);
        string KeePassPath { get; }
        
        bool KeepassPathIsConfigured { get; }
        void SetKeepassPath(string path);
    }

    class Storage : IStorage {
        private PluginInitContext _context;
        private string _directory;
        private readonly Dictionary<string, Link> _links;

        public string Path => System.IO.Path.Combine(_directory ?? "", "keepas.config.json");

        public Storage(PluginInitContext context) {
            _context = context;
            _directory = _context.CurrentPluginMetadata?.PluginDirectory;

            _links = Load();
        }

        private Dictionary<string, Link> Load() {
            if (File.Exists(Path)) {
                var links = JsonConvert.DeserializeObject<Link[]>(File.ReadAllText(Path));
                return links.ToDictionary(x => x.Shortcut, x => x);
            }

            return new Dictionary<string, Link>();
        }

        public void Set(string shortcut, string url, string description) {
            _links[shortcut] = new Link {
                Path = url,
                Shortcut = shortcut,
                Description = description
            };
            Save();
        }

        public Link[] GetShortcuts() {
            return _links.Values.ToArray();
        }

        public void Delete(string shortcut) {
            _links.Remove(shortcut);
            Save();
        }

        public string KeePassPath { get; }
        public bool KeepassPathIsConfigured { get; }
        public void SetKeepassPath(string path) {
            throw new System.NotImplementedException();
        }

        private void Save() {
            var content = JsonConvert.SerializeObject(_links.Values.ToArray(), Formatting.Indented);
            File.WriteAllText(Path, content);
        }
    }

    public class StorageDto {
        public string KeePassPath { get; set; }

        public int Version { get; set; }
    }
}