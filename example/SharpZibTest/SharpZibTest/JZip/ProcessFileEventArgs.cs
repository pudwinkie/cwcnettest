using System;
using System.Collections.Generic;
using System.Text;

namespace SharpZibTest {
    public class ProcessFileEventArgs : EventArgs {
        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">目前處理的檔案</param>
        public ProcessFileEventArgs(string filename) {
            this.filename = filename;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 目前處理的檔案
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
