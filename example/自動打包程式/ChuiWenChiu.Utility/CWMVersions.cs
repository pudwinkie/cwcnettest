using System;
using System.Collections;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// WM 版本集合
    /// </summary>
    public class CWMVersions {
        private ArrayList _col;
        public CWMVersions() {
            _col = new ArrayList();
        }

        /// <summary>
        /// 新增一個 WM 版本
        /// </summary>
        /// <param name="data"></param>
        public void Add(CWMVersion data) {
            _col.Add(data);
        }

        /// <summary>
        /// 索引子
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CWMVersion this[int index] {
            get {
                return (CWMVersion)_col[index];
            }
        }

        /// <summary>
        /// WM 版本數目
        /// </summary>
        /// <returns></returns>
        public int Count {
            get {
                return _col.Count;
            }
        }

    }
}
