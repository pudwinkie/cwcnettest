using System;
using System.Collections.Generic;
using System.Text;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// WM 版本
    /// </summary>
    public class CWMVersion {
        private string _id;
        private string _ip;
        private string _WorkDir;
        private CWMProjects _projects = null;

        public CWMVersion() {

        }

        public CWMVersion(string id, string ip, string workDir, CWMProjects projects) {
            this.ID = id;
            this.IP = ip;
            this.WorkDir = workDir;
            this.Projects = projects;
        }
        /// <summary>
        /// WM 版本
        /// </summary>
        public string ID {
            get {
                return _id;
            }

            set {
                _id = value;
            }
        }
        /// <summary>
        /// WM 專案機所在的 IP
        /// </summary>
        public string IP {
            get {
                return _ip;
            }

            set {
                _ip = value;
            }
        }
        /// <summary>
        /// 專案所在的根目錄
        /// </summary>
        public string WorkDir {
            get {
                return _WorkDir;
            }

            set {
                _WorkDir = value;
            }
        }
        /// <summary>
        /// 專案集合參考
        /// </summary>
        public CWMProjects Projects {
            get {
                return _projects;
            }

            set {
                _projects = value;
            }
        }
    }
}
