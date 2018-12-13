using System.Collections.Generic;
using System.Linq;

namespace Wox.Plugin.Keepass {
    internal class Main : IPlugin {
        private IEngine _engine;

        public List<Result> Query(Query query) {
            var result = _engine.Execute(query);
            return result.ToList();
        }

        public void Init(PluginInitContext context) {
            Startup.Initialize(context);
            _engine = Startup.Resolve<IEngine>();
            
        }
    }
}