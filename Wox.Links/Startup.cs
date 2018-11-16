using System.Configuration;
using Autofac;
using Wox.Plugin;

namespace Wox.Links {
    public class Startup {
        private static IContainer _container;

        public static void Initialize(PluginInitContext pluginInitContext) {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginInitContext).AsSelf();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }
    }
}