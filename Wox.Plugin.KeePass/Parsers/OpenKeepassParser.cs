﻿using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Wox.Plugin;
using Wox.Plugins.Common;

namespace Wox.Plugins.KeePass.Parsers {
    public interface IOpenKeePassParser : IParser {
    }

    public class OpenKeePassParser : IOpenKeePassParser {
        private const string logPath = @"C:\Projects\log.txt";
        private readonly IFileService _fileService;
        private readonly IStorage _storage;
        private string _password;

        public OpenKeePassParser(IStorage storage, IFileService fileService) {
            _fileService = fileService;
            _storage = storage;
            _password = new string(new char[0]);
        }


        private bool PasswordIsSet => _password.Length > 0;

        public bool TryParse(string query, out List<Result> results) {
            results = new List<Result>();
            var isValid = _storage.KeePassPathIsConfigured && (PasswordIsSet || query.NotEmpty());
            if (!isValid) {
                return false;
            }

            var result = query.IsEmpty()
                ? CreateResultFromSavedPassword()
                : CreateResultFromTypedPassword(query);
            results.Add(result);

            return true;
        }

        private Result CreateResultFromSavedPassword() {
            return new Result {
                Title = GetTitle(),
                Action = context => {
                    OpenKeePass();
                    return true;
                }
            };
        }

        private string GetTitle() {
            return $"Open '{Path.GetFileName(_storage.KeePath.KeePassFilePath)}' with given password";
        }

        private Result CreateResultFromTypedPassword(string query) {
            var sb = new StringBuilder(query);
            return new Result {
                Title = GetTitle(),
                SubTitle = sb.ToString().GetHashCode().ToString(),
                Action = context => {
                    _password = sb.ToString();
                    OpenKeePass();
                    return true;
                }
            };
        }

        private void OpenKeePass() {
            var password = GetPassword();
            var formattableString =
                $@"""{_storage.KeePath.KeePassFilePath}"" -pw:{password}";
            _fileService.Start(_storage.KeePath.ApplicationPath, formattableString);
        }

        private string GetPassword() {
            return _password;
            return new NetworkCredential(string.Empty, _password).Password;
        }
    }
}