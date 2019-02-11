using System.Configuration;
using Autofac;
using Wox.Links.Parsers;
using Wox.Links.Services;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Links {
    public class Startup {
        private static IContainer _container;

        public static void Initialize(PluginInitContext pluginInitContext) {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginInitContext).AsSelf();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<GetLinkParser>().As<IParser>().SingleInstance();
            container.RegisterType<SaveParser>().As<IParser>().SingleInstance();
            container.RegisterType<DeleteParser>().As<IParser>().SingleInstance();
            container.RegisterType<Storage>().As<IStorage>().SingleInstance();
            container.RegisterType<LinkProcess>().As<ILinkProcess>().SingleInstance();
            container.RegisterType<ImportConfigParser>().As<IParser>().SingleInstance();
            container.RegisterType<FileService>().As<IFileService>().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }
    }
}