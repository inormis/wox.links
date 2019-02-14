using System.Collections.Generic;
using System.Linq;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    internal class Main : IPlugin {
        private IEngine _engine;

        public List<Result> Query(Query query) {
            var result = _engine.Execute(new QueryInstance(query));
            return result.ToList();
        }

        public void Init(PluginInitContext context) {
            Startup.Initialize(context);
            _engine = Startup.Resolve<IEngine>();
        }
    }
}