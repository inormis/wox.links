using System.Configuration;
using Autofac;
using Wox.Plugin.Keepass.Parsers;
using Wox.Plugin.Keepass.Services;

namespace Wox.Plugin.Keepass {
    public class Startup {
        private static IContainer _container;

        public static void Initialize(PluginInitContext pluginInitContext) {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginInitContext).AsSelf();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<Configuration>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<GetLinkParser>().As<IParser>().SingleInstance();
            container.RegisterType<RunParser>().As<IParser>().SingleInstance();
            container.RegisterType<DeleteParser>().As<IParser>().SingleInstance();
            container.RegisterType<Storage>().As<IStorage>().SingleInstance();
            container.RegisterType<LinkProcess>().As<ILinkProcess>().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }
    }
}