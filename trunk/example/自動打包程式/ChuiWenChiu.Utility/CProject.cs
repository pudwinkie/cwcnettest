using System;
using System.Collections.Generic;
using System.Text;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// �M��
    /// </summary>
    public class CWMProject {
        #region �p������
        private string _name;
        private string _tag;
        #endregion

        #region �غc�l
        public CWMProject() {
            this.name = "";
            this.tag = "";
        }

        public CWMProject(string name, string tag) {
            this.name = name;
            this.tag = tag;
        }
        #endregion

        #region �ݩ�
        /// <summary>
        /// �M�׻���
        /// </summary>
        public string name {
            get {
                return _name;
            }

            set {
                _name = value;
            }
        }
        /// <summary>
        /// �M�ץN�X
        /// </summary>
        public string tag {
            get {
                return _tag;
            }

            set {
                _tag = value;
            }
        }
        #endregion
    }
}
