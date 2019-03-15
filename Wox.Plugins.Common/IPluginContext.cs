using Wox.Plugin;

namespace Wox.Plugins.Common {
    public interface IPluginContext {
        string Directory { get; }
    }

    public class PluginContext : IPluginContext {
        private readonly PluginInitContext _pluginInitContext;

        public PluginContext(PluginInitContext pluginInitContext) {
            _pluginInitContext = pluginInitContext;
        }

        public string Directory => _pluginInitContext.CurrentPluginMetadata.PluginDirectory;
    }
}