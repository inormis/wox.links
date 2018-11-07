using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wox.Plugin;

namespace Wox.Links.Parsers
{
    public interface ILinkProcess
    {
        void Process(string url, params string[] args);
        void Open(string httpsSomeComDo);
    }

    public class GetLinkParser : IParser
    {
        static Regex SaveMatch = new Regex(@"--save\b|-s\b", RegexOptions.IgnoreCase);

        static Regex LinkMatch =
            new Regex(@"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$");

        private IStorage _storage;
        private ILinkProcess _linkProcess;

        public GetLinkParser(IStorage storage, ILinkProcess linkProcess)
        {
            _linkProcess = linkProcess;
            _storage = storage;
        }

        public bool TryParse(string[] terms, out List<Result> results)
        {
            results = new List<Result>();

            if (terms.Length == 0)
                return false;
            var key = terms.First();
            var links = _storage.GetShortcuts().Where(x => x.Shortcut.ContainsCaseInsensitive(key)).ToArray();

            results.AddRange(links.Select(link => Create(link, terms.Skip(1).ToArray())));
            return true;
        }

        private Result Create(Link x, string[] args)
        {
            var url = args.Length == 0 ? x.Url : string.Format(x.Url, args);
            return new Result
            {
                Title = url,
                Action = context =>
                {
                    _linkProcess.Open(url);
                    return CanOpenLink(url);
                }
            };
        }

        private static bool CanOpenLink(string url)
        {
            return !Regex.IsMatch(url, @"\{\d\}");
        }
    }
}