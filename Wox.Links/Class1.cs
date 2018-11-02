using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Autofac;
using Wox.Plugin;

namespace Wox.Links
{
    internal class Main : IPlugin
    {
        private IEngine _engine;

        public List<Result> Query(Query query)
        {
            var result = _engine.Execute(query);
            return result.ToList();
        }

        public void Init(PluginInitContext context)
        {
            Startup.Initialize(context);
            _engine = Startup.Resolve<IEngine>();
        }
    }

    public interface IStorage
    {
        void Add(string shortcut, string url);
    }


    internal interface IEngine
    {
        List<Result> Execute(Query query);
    }

    public class Startup
    {
        private static IContainer _container;

        public static void Initialize(PluginInitContext pluginInitContext)
        {
            var container = new ContainerBuilder();
            container.RegisterInstance(pluginInitContext).AsSelf();
            container.RegisterType<Engine>().AsImplementedInterfaces().SingleInstance();
            container.RegisterType<SettingsProvider>().AsImplementedInterfaces().SingleInstance();
            _container = container.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }

    public class SaveParser : IParser
    {
        static Regex SaveMatch = new Regex(@"--save\b|-s\b", RegexOptions.IgnoreCase);

        static Regex LinkMatch =
            new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

        private IStorage _storage;

        public SaveParser(IStorage storage)
        {
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results)
        {
            results = new List<Result>();
            if (terms.Length < 3)
                return false;

            var saveKeyWord = terms.FirstOrDefault(x => SaveMatch.IsMatch(x));
            var linkWord = terms.FirstOrDefault(x => LinkMatch.IsMatch(x));

            if (saveKeyWord == null || linkWord == null)
                return false;

            var shortCut = terms.First(x => x != saveKeyWord && x != linkWord);
            results.Add(CreateResult(shortCut, linkWord));
            return true;
        }

        private Result CreateResult(string shortCut, string linkWord)
        {
            return new Result
            {
                Title = $"Save the link as \'{shortCut}\'",
                SubTitle = linkWord,
                Action = context =>
                {
                    _storage.Add(shortCut, linkWord);
                    return true;
                }
            };
        }
    }

    public interface IParser
    {
        bool TryParse(string[] terms, out List<Result> results);
    }

    public class Engine : IEngine
    {
        private IEnumerable<IParser> _parsers;

        public Engine(IEnumerable<IParser> parsers)
        {
            _parsers = parsers;
        }

        public List<Result> Execute(Query query)
        {
            var terms = query.Terms;
            foreach (var parser in _parsers)
            {
                if (parser.TryParse(terms, out List<Result> results))
                    return results;
            }

            return new List<Result>();
        }
    }
}