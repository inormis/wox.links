namespace Wox.Plugins.Common {
    public interface IFile {
        void Open(string path);

        bool Exists(string filePath);

        bool CheckExtension(string filePath, string extension);
        void Start(string command, string args);
    }
}