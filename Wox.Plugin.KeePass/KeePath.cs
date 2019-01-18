namespace Wox.Plugins.KeePass {
    public interface IKeePath {
        string ApplicationPath { get; set; }
        string KeePassFilePath { get; set; }
    }

    public class KeePath : IKeePath {
        public string ApplicationPath { get; set; }

        public string KeePassFilePath { get; set; }

        public string CreateCommand(string password) {
            return $@"{ApplicationPath} {KeePassFilePath} -p {password}";
        }
    }
}