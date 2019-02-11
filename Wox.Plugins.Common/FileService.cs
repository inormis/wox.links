using System;
using System.Diagnostics;
using System.IO;

namespace Wox.Plugins.Common {
    public class FileService : IFileService {
        public void Open(string path) {
            Process.Start(path);
        }

        public bool Exists(string filePath) {
            return File.Exists(filePath);
        }

        public bool CheckExtension(string filePath, string extension) {
            return string.Compare(Path.GetExtension(filePath), extension,
                       StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        public void Start(string command, string args) {
            Process.Start(command, args);
        }
    }
}