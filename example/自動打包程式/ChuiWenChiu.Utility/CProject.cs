using System;
using System.Collections.Generic;
using System.Text;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// 專案
    /// </summary>
    public class CWMProject {
        #region 私有成員
        private string _name;
        private string _tag;
        #endregion

        #region 建構子
        public CWMProject() {
            this.name = "";
            this.tag = "";
        }

        public CWMProject(string name, string tag) {
            this.name = name;
            this.tag = tag;
        }
        #endregion

        #region 屬性
        /// <summary>
        /// 專案說明
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
        /// 專案代碼
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
