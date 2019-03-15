namespace Wox.Plugins.Common {
    public interface IFileService {
        void Open(string path);

        bool Exists(string filePath);

        bool CheckExtension(string filePath, string extension);
        void Start(string command, string args);
        bool WriteAllText(string filePath, string content);
        string ReadAllText(string path);
    }
}