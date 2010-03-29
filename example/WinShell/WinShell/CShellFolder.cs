using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace WinShell {

    /// <summary>
    /// 
    /// </summary>
    public class CShellFolder: IDisposable {
        public CShellFolder() {
            desktop = API.GetDesktopFolder(/*out desktopPtr*/);
        }

        public void Dispose() {
            Marshal.ReleaseComObject(desktop);
        }

        /// <summary>
        /// 
        /// </summary>
        public class FetchFolderEventArgs : EventArgs {
            public FetchFolderEventArgs(String folder)
                : base() {
                foldername = folder;
            }

            public String Folder {
                get {
                    return foldername;
                }
            }

            private String foldername;
        }

        /// <summary>
        /// 
        /// </summary>
        public class FetchFileEventArgs : EventArgs {
            public FetchFileEventArgs(String fileName)
                : base() {
                filename = fileName;
            }

            public String Filename {
                get {
                    return filename;
                }
            }

            private String filename;
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<FetchFolderEventArgs> FetchFolderEventHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="folderName"></param>
        public void OnFetchFolder(Object sender, String folderName) {
            if (FetchFolderEventHandler != null) {
                FetchFolderEventHandler(sender, new FetchFolderEventArgs(folderName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<FetchFileEventArgs> FetchFileEventHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="fileName"></param>
        public void OnFatchFile(object sender, String fileName) {
            if (FetchFileEventHandler != null) {
                FetchFileEventHandler(sender, new FetchFileEventArgs(fileName));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FolderPath"></param>
        public void DoFatch(string FolderPath) {
            //獲取 C 盤的 PIDL
            //string FolderPath = @"C:\";
            IntPtr Pidl = IntPtr.Zero;
            IShellFolder Root;
            uint i, j = 0;
            // 獲得對象的PIDL，即便對像在目錄樹中處於當前目錄下一層或更多層。例如，對於文件對象來說，它的解析名就是它的路徑，我們用文件系統對象的完全路徑名來調用桌面的IshellFolder接口的 ParseDisplayName 方法，它會返回這個對象的完全PIDL
            desktop.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, FolderPath, out i, out Pidl, ref j);
            desktop.BindToObject(Pidl, IntPtr.Zero, ref Guids.IID_IShellFolder, out Root);
           

            //循環尋找 C 盤下面的文件/文件夾的 PIDL
            IEnumIDList fileEnum = null;
            IEnumIDList folderEnum = null;
            IntPtr fileEnumPtr = IntPtr.Zero;
            IntPtr folderEnumPtr = IntPtr.Zero;
            IntPtr pidlSub;
            int celtFetched;

            //獲取子文件夾
            if (Root.EnumObjects(IntPtr.Zero, SHCONTF.FOLDERS | SHCONTF.INCLUDEHIDDEN, out fileEnumPtr) == API.S_OK) {
                fileEnum = (IEnumIDList)Marshal.GetObjectForIUnknown(fileEnumPtr);
                while (fileEnum.Next(1, out pidlSub, out celtFetched) == 0 && celtFetched == API.S_FALSE) {
                    //獲取顯示名稱
                    string name = API.GetNameByPIDL(pidlSub);
                    OnFetchFolder(this, name);
                    //lvFile.Items.Add(name, 1);
                }
            }

            //獲取子文件
            if (Root.EnumObjects(IntPtr.Zero, SHCONTF.NONFOLDERS | SHCONTF.INCLUDEHIDDEN, out folderEnumPtr) == API.S_OK) {
                folderEnum = (IEnumIDList)Marshal.GetObjectForIUnknown(folderEnumPtr);
                while (folderEnum.Next(1, out pidlSub, out celtFetched) == 0 && celtFetched == API.S_FALSE) {
                    string name = API.GetNameByPIDL(pidlSub);
                    OnFatchFile(this, name);
                    //lvFile.Items.Add(name, 0);
                }
            }

            Marshal.ReleaseComObject(Root);
        }

        private IShellFolder desktop;
    }
}
