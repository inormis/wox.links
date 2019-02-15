using System.Configuration;
using Autofac;
using Wox.Plugin.Links.Parsers;
using Wox.Plugin.Links.Services;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    public class Startup {
        private static IContainer _container;

        public static void Initialize(PluginInitContext pluginInitContext) {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginInitContext).AsSelf();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<Storage>().As<IStorage>().SingleInstance();
            container.RegisterType<LinkProcess>().As<ILinkProcess>().SingleInstance();
            container.RegisterType<FileService>().As<IFileService>().SingleInstance();
            
            container.RegisterType<SaveParser>().As<IParser>().AsSelf().SingleInstance();
            container.RegisterType<DeleteParser>().As<IParser>().AsSelf().SingleInstance();
            container.RegisterType<ImportParser>().As<IParser>().AsSelf().SingleInstance();
            container.RegisterType<ExportParser>().As<IParser>().AsSelf().SingleInstance();
            container.RegisterType<RenameParser>().As<IParser>().AsSelf().SingleInstance();
            container.RegisterType<GetLinkParser>().As<IParser>().AsSelf().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }
    }
}