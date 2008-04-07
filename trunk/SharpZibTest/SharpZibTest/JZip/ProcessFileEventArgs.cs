using System;
using System.Collections.Generic;
using System.Text;

namespace SharpZibTest {
    public class ProcessFileEventArgs : EventArgs {
        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">�ثe�B�z���ɮ�</param>
        public ProcessFileEventArgs(string filename) {
            this.filename = filename;
        }
        #endregion

        #region Properties
        /// <summary>
        /// �ثe�B�z���ɮ�
        /// </summary>
        public string Filename {
            get {
                return filename;
            }
        }
        private string filename;
        #endregion
    }
}
