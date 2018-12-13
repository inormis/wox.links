using System.Diagnostics;

namespace Wox.Plugin.Keepass.Services {
    public interface ILinkProcess {
        void Open(string path);
    }

    class LinkProcess : ILinkProcess {
        public void Open(string path) {
            Process.Start(path);
        }
    }
}