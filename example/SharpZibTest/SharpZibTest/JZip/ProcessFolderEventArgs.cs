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
        /// <param name="filename">目前處理的檔案</param>
        public ProcessFolderEventArgs(string folder) {
            this.folder = folder;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 目前處理的檔案
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
