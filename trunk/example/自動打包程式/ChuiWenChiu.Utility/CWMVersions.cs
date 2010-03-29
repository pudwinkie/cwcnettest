using System;
using System.Collections;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// WM �������X
    /// </summary>
    public class CWMVersions {
        private ArrayList _col;
        public CWMVersions() {
            _col = new ArrayList();
        }

        /// <summary>
        /// �s�W�@�� WM ����
        /// </summary>
        /// <param name="data"></param>
        public void Add(CWMVersion data) {
            _col.Add(data);
        }

        /// <summary>
        /// ���ޤl
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CWMVersion this[int index] {
            get {
                return (CWMVersion)_col[index];
            }
        }

        /// <summary>
        /// WM �����ƥ�
        /// </summary>
        /// <returns></returns>
        public int Count {
            get {
                return _col.Count;
            }
        }

    }
}
