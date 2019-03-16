using System.Linq;
using Wox.Plugins.Common;

namespace Wox.Plugin.Links {
    public class Link {
        public string Shortcut { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }

        public bool HasPlaceHolders => Shortcut.Contains("@@");

        public bool Matches(IQuery query) {
            return Shortcut.MatchShortcut(query.Search);
        }
    }
}