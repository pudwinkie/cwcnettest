#region
using System;
using System.Text;
using System.IO;
using java.util.zip;
#endregion

namespace SharpZibTest {
    /// <summary>
    /// 封裝 java.util.zip 
    /// </summary>
    public class JZip {
        #region constructor
        /// <summary>
        /// 
        /// </summary>
        public JZip() {
        }
        #endregion


        #region public
        #region method
        /// <summary>
        /// 解 zip 壓縮
        /// </summary>
        /// <param name="zipFile">來源 zip 壓縮檔完整名稱</param>
        /// <param name="dest">解壓縮路徑</param>
        public void ExtractZip(string zipFile, string dest) {
            ExtractZip(zipFile, dest, "");
        }

        /// <summary>
        /// 解 zip 壓縮
        /// </summary>
        /// <param name="zipFile">來源 zip 壓縮檔完整名稱</param>
        /// <param name="dest">解壓縮路徑</param>
        /// <param name="filter">副檔名過濾器(如：.txt)</param>
        public void ExtractZip(string zipFile, string dest, string filter) {            
            const string PathSep1 = "/";
            const string PathSep2 = @"\";
            const string ZipFolderSign = PathSep1;
            const string FilterAllPass1 = "";
            const string FilterAllPass2 = ".*";

            java.io.FileInputStream fis = new java.io.FileInputStream(zipFile);
            java.util.zip.ZipInputStream zis = new java.util.zip.ZipInputStream(fis);
            java.util.zip.ZipEntry ze;

            if (dest.EndsWith(PathSep1) == false && dest.EndsWith(PathSep2) == false) {
                dest += PathSep1;
            }

            string fileName = null;
            sbyte[] buf = new sbyte[1024];
            int len;
            bool isFolder = false;
            while ((ze = zis.getNextEntry()) != null) {
                fileName = dest + ze.getName();
                isFolder = fileName.EndsWith(ZipFolderSign);

                FileInfo fi = new FileInfo(fileName);
                if (isFolder) {
                    OnProcessFolder(this, new ProcessFolderEventArgs(fileName));
                }
                if (isFolder || filter == FilterAllPass1 || filter == FilterAllPass2 || fi.Extension == filter) {
                    int index = fileName.LastIndexOf(PathSep1[0]);
                    if (index > 1) {
                        string folder = fileName.Substring(0, index);
                        DirectoryInfo di = new DirectoryInfo(folder);
                        if (!di.Exists)
                            di.Create();
                    }
                    //try {
                        if (isFolder == false) {
                            OnProcessFile(this, new ProcessFileEventArgs(fileName));
                            java.io.FileOutputStream fos = new java.io.FileOutputStream(fileName);
                            while ((len = zis.read(buf)) >= 0) {
                                fos.write(buf, 0, len);
                            }
                            fos.close();
                        }
                    //} catch (Exception e) {
                    //}
                }
            }
            zis.close();
            fis.close();
        }
        #endregion
        #endregion

        #region event      

        public event EventHandler<ProcessFileEventArgs> ProcessFile;  
        protected void OnProcessFile(object sender, ProcessFileEventArgs e) {
            if (ProcessFile != null) {
                ProcessFile(sender, e); 
            }
        }


        public event EventHandler<ProcessFolderEventArgs> ProcessFolder;  
        protected void OnProcessFolder(object sender, ProcessFolderEventArgs e) {
            if (ProcessFolder != null) {
                ProcessFolder(sender, e); 
            }
        }
        #endregion
    }

}
