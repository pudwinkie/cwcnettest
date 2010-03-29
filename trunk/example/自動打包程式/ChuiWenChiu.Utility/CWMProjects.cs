using System;
using System.Collections;
using System.Text;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// �M�׶��X
    /// </summary>
    public class CWMProjects {
        private ArrayList _col;
        public CWMProjects() {
            _col = new ArrayList();
        }

        public void Add(CWMProject data) {
            _col.Add(data);
        }

        /// <summary>
        /// ���ޤl
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public CWMProject this[int index] {
            get {
                return (CWMProject)_col[index];
            }
        }

        /// <summary>
        /// �M�׼ƥ�
        /// </summary>
        /// <returns></returns>
        public int Count {
            get {
                return _col.Count;
            }
        }
    }
}
