using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;

namespace GettingOfficeVersion {

    public class OfficeVersion {
        private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";

        public String GetWordVersion() {
            return GetAppVersion(GetComponentPath(OfficeComponent.Word));
        }

        public String GetExcelVersion() {
            return GetAppVersion(GetComponentPath(OfficeComponent.Excel));
        }

        public String GetPowerpointVersion() {
            return GetAppVersion(GetComponentPath(OfficeComponent.PowerPoint));
        }

        public String GetOutlookVersion() {
            return GetAppVersion(GetComponentPath(OfficeComponent.Outlook));
        }

        private string GetAppVersion(string _wordPath) {
            if (GetMajorVersion(_wordPath) == 12) {
                return "2007";
            } else if (GetMajorVersion(_wordPath) == 11) {
                return "2003";
            } else {
                return string.Empty;
            }
        }

        /// <summary>
        /// gets the component's path from the registry. if it can't find it - retuns an empty string
        /// </summary>
        /// <param name="_component"></param>
        /// <returns></returns>
        private string GetComponentPath(OfficeComponent _component) {
            string toReturn = string.Empty;
            string _key = string.Empty;

            switch (_component) {
                case OfficeComponent.Word:
                    _key = "winword.exe";
                    break;
                case OfficeComponent.Excel:
                    _key = "excel.exe";
                    break;
                case OfficeComponent.PowerPoint:
                    _key = "powerpnt.exe";
                    break;
                case OfficeComponent.Outlook:
                    _key = "outlook.exe";
                    break;
            }

            //looks inside CURRENT_USER:
            RegistryKey _mainKey = Registry.CurrentUser;
            try {
                _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                if (_mainKey != null) {
                    toReturn = _mainKey.GetValue(string.Empty).ToString();
                }
            } catch { }

            //if not found, looks inside LOCAL_MACHINE:
            _mainKey = Registry.LocalMachine;
            if (string.IsNullOrEmpty(toReturn)) {
                try {
                    _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                    if (_mainKey != null) {
                        toReturn = _mainKey.GetValue(string.Empty).ToString();
                    }
                } catch {
                }
            }

            //closing the handle:
            if (_mainKey != null)
                _mainKey.Close();

            return toReturn;
        }

        /// <summary>
        /// Gets the major version of the path. if file not found (or any other exception occures
        /// - returns 0
        /// </summary>
        private int GetMajorVersion(string _path) {
            int toReturn = 0;
            if (File.Exists(_path)) {
                try {
                    FileVersionInfo _fileVersion = FileVersionInfo.GetVersionInfo(_path);
                    toReturn = _fileVersion.FileMajorPart;
                } catch { }
            }

            return toReturn;
        }

        public enum OfficeComponent {
            Word,
            Excel,
            PowerPoint,
            Outlook
        }
    }
}
