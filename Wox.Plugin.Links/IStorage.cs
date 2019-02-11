using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Wox.Plugin;

namespace Wox.Links {
    public interface IStorage {
        void Set(string shortcut, string url, string description);

        Link[] GetShortcuts();

        void Delete(string shortcut);
        bool TryImport(string jsonPath);
    }

    internal class Storage : IStorage {
        private readonly Configuration _configuration;
        private readonly PluginInitContext _context;
        private readonly string _directory;
        private Dictionary<string, Link> _links;

        public Storage(PluginInitContext context) {
            _context = context;
            _directory = _context.CurrentPluginMetadata?.PluginDirectory;
            _configuration = LoadConfiguration();
            _links = LoadLinks();
        }

        private string ConfigurationPath => Path.Combine(_directory ?? "", "config.json");
        private string DefaultLinksPath => Path.Combine(_directory ?? "", "links.json");

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

        public bool TryImport(string jsonPath) {
            try {
                _links = ReadLinksFromFile(jsonPath);
                return true;
            }
            catch {
                return false;
            }
        }

        private Configuration LoadConfiguration() {
            if (File.Exists(ConfigurationPath)) {
                return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(ConfigurationPath));
            }

            var configuration = new Configuration {
                LinksFilePath = DefaultLinksPath
            };
            return configuration;
        }

        private Dictionary<string, Link> LoadLinks() {
            var linksFilePath = _configuration.LinksFilePath;
            if (File.Exists(linksFilePath)) {
                return ReadLinksFromFile(linksFilePath);
            }

            return new Dictionary<string, Link>();
        }

        private static Dictionary<string, Link> ReadLinksFromFile(string linksFilePath) {
            var links = JsonConvert.DeserializeObject<Link[]>(File.ReadAllText(linksFilePath));
            return links.ToDictionary(x => x.Shortcut, x => x);
        }

        private void Save() {
            var serializedConfiguration = JsonConvert.SerializeObject(_configuration, Formatting.Indented);
            File.WriteAllText(ConfigurationPath, serializedConfiguration);
            
            var content = JsonConvert.SerializeObject(_links.Values.ToArray(), Formatting.Indented);
            File.WriteAllText(_configuration.LinksFilePath, content);
        }
    }

    public class Configuration {
        public string LinksFilePath { get; set; }
    }
}