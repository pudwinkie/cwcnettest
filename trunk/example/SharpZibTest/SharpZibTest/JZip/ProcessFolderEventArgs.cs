using System;

namespace SharpZibTest {
    /// <summary>
    /// 
    /// </summary>
    public class ProcessFolderEventArgs:EventArgs  {
        #region constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename">�ثe�B�z���ɮ�</param>
        public ProcessFolderEventArgs(string folder) {
            this.folder = folder;
        }
        #endregion

        #region Properties
        /// <summary>
        /// �ثe�B�z���ɮ�
        /// </summary>
        public string Folder {
            get {
                return folder;
            }
        }
        private string folder;
        #endregion
    }
}
