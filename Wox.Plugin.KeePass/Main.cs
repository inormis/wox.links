using System.Collections.Generic;
using System.Linq;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Plugins.KeePass {
    internal class Main : IPlugin {
        private IEngine _engine;

        public List<Result> Query(Query query) {
            var result = _engine.Execute(query);
            return result.ToList();
        }

        public void Init(PluginInitContext context) {
            Startup.Initialize(new PluginContext(context));
            _engine = Startup.Resolve<IEngine>();
        }
    }
}