using System;
using System.Collections.Generic;
using System.Text;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// WM ����
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
        /// WM ����
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
        /// WM �M�׾��Ҧb�� IP
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
        /// �M�שҦb���ڥؿ�
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
        /// �M�׶��X�Ѧ�
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
