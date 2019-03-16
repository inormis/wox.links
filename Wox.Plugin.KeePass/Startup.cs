using System.Configuration;
using Autofac;
using Wox.Plugin;
using Wox.Plugins.Common;
using Wox.Plugins.KeePass.Parsers;

namespace Wox.Plugins.KeePass {
    public class Startup {
        private static IContainer _container;

        public static void Initialize(IPluginContext pluginContext) {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginContext).As<IPluginContext>();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<Configuration>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<OpenKeePassParser>().As<IParser>().SingleInstance();
            container.RegisterType<Storage>().As<IStorage>().SingleInstance();
            container.RegisterType<FileService>().As<IFileService>().SingleInstance();
            container.RegisterType<SetKeePassPathParser>().As<ISetKeePassPathParser>().SingleInstance();
            container.RegisterType<OpenKeePassParser>().As<IOpenKeePassParser>().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>() {
            return _container.Resolve<T>();
        }
    }
}