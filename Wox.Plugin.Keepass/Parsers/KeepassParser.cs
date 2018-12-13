using System.Collections.Generic;
using System.Diagnostics;

namespace Wox.Plugin.Keepass.Parsers {
    public class KeepassParser : IParser {
        public bool TryParse(string[] terms, out List<Result> results) {

            results=new List<Result>();
            if (terms.Length == 0)
                return false;
            
            //""C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe"" ""C:\Users\akuci\Google Drive\Storage\KeePass\KEYHUB.kdbx"" -pw:764591382tT_secure

            Process.Start(
                @"""C:\Program Files (x86)\KeePass Password Safe 2\KeePass.exe"" ""C:\Users\akuci\Google Drive\Storage\KeePass\KEYHUB.kdbx"" -pw:764591382tT_secure");
            return true;
        }
    }
}