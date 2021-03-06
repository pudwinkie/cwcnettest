using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;

namespace ChuiWenChiu.Utility {
    /// <summary>
    /// 檔案收集的事件參數
    /// </summary>
    public class CollectEventArgs : EventArgs {
        private readonly string msg;
        /// <summary>
        /// 訊息參數
        /// </summary>
        /// <param name="msg">訊息內容</param>
        public CollectEventArgs(string msg) {
            this.msg = msg;
        }
         
        /// <summary>
        /// 訊息內容
        /// </summary>
        public string Message {
            get {
                return this.msg;
            }
        }
    }

    public delegate void SuccessEventHandler(CollectEventArgs e);
    public delegate void FailEventHandler(CollectEventArgs e);
    public delegate void ReplaceEventHandler(CollectEventArgs e);
    public delegate void NoExistEventHandler(CollectEventArgs e);
    public delegate void AbortEventHandler();
    public delegate void EnumFileEventHandler(CollectEventArgs e);

    /// <summary>
    /// 檔案收及類別
    /// </summary>
    public class CCollect {
        public event SuccessEventHandler Success;
        public event FailEventHandler       Fail;
        public event ReplaceEventHandler    Replace;
        public event NoExistEventHandler    NoExist;
        public event AbortEventHandler      EAbort;
        public event EnumFileEventHandler   EEnumFile;

        #region 資料成員
        private readonly string _filelist;
        private string _srcPath;
        private string _dstPath;
        private bool isAbort;
        #endregion
        #region 建構子
        /// <summary>
        /// 預設建構子
        /// </summary>
        public CCollect() {
            this.SourcePath = "";
            this.DestPath   = "";

        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="filelist">存放檔案清單之檔名</param>
        public CCollect(string filelist) {
            isAbort = false;  
            if (File.Exists(filelist) == false) {
                throw new Exception(filelist + " : 不存在");
            }

            this._filelist = filelist;
            StreamReader sw = new StreamReader(this._filelist, Encoding.Default);
            string tmp;
            if (!sw.EndOfStream) {
                tmp = sw.ReadLine();
                tmp = tmp.Trim();
                tmp = cutLastSep(tmp);

                this.DestPath = tmp;
            }

            if (!sw.EndOfStream) {
                tmp = sw.ReadLine();
                tmp = tmp.Trim();
                tmp = cutLastSep(tmp);

                this._srcPath = tmp;
                if (Directory.Exists(this._srcPath) == false) {
                    sw.Close();
                    throw new Exception("Exception: 來源路徑不存在");
                }
            }
            sw.Close();
        }

        #endregion 
        #region 屬性
        /// <summary>
        /// 來源路徑
        /// </summary>
        public string SourcePath {
            get {
                return this._srcPath;
            }

            set {
                _srcPath = value;
            }
        }
        /// <summary>
        /// 目的路徑
        /// </summary>
        public string DestPath{
            get {
                return this._dstPath;
            }

            set {
                _dstPath = value;
            }
        }
        #endregion
        #region 事件
        /// <summary>
        /// 丟出成功複製的訊息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSuccess(CollectEventArgs e) {
            if (Success != null) {
                Success(e); 
            }
        }
        /// <summary>
        /// 丟出複製失敗的訊息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFail(CollectEventArgs e) {
            if (Fail != null) {
                Fail(e); 
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnAbort() {
            if (EAbort != null) {
                EAbort();
            }
        }
        /// <summary>
        /// 列舉檔案清單
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEnumFile(CollectEventArgs e) {
            if (EEnumFile != null) {
                EEnumFile(e);
            }
        }
        /// <summary>
        /// 丟出檔案重複的訊息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnReplace(CollectEventArgs e) {
            if (Replace != null) {
                Replace(e); 
            }
        }
        /// <summary>
        /// 丟出來源檔案不存在的訊息
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnNoExist(CollectEventArgs e) {
            if (NoExist != null) {
                NoExist(e);
            }
        }
        #endregion
        #region 私有方法

        /// <summary>
        /// 修正跳脫符號
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string converSep(string str) {
            StringBuilder sb = new StringBuilder(str);             
            
            return sb.Replace("/", "\\").ToString() ;
        }
        /// <summary>
        ///裁減最後一個跳脫符號
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string cutLastSep(string path){
            string sep = path.Substring(path.Length - 1);
            if (sep == "/" || sep == "\\") {
                path = path.Substring(0, path.Length - 1);
            }
            return path;
        }
        /// <summary>
        /// 插入一個跳脫符號到字串開頭
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string insertFirstSep(string filename) {             
            string sep = filename.Substring(0, 1);
            if (sep != "/" && sep != "\\") {
                filename = "\\" + filename ;
            }
            return filename;
        }
        /// <summary>
        /// 檢查檔案是否為目錄
        /// </summary>
        /// <param name="filename">待確認檔名</param>
        /// <returns>true = 成功, false = 失敗</returns>
        private bool isFolder(string filename){  
/*
            string patch = filename.Substring(filename.Length - 1 );
            return patch == "*";
*/
             
            return Path.GetFileName(filename) == "*";
        }
        /// <summary>
        /// 資料夾複製
        /// </summary>
        /// <param name="srcPath">來源目錄</param>
        /// <param name="aimPath">目的目錄</param>
        private void CopyDir(string srcPath, string aimPath) {
            // 检查目标目录是否以目录分割字符结束如果不是则添加之
            if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar) {
                aimPath += Path.DirectorySeparatorChar;
            }
            if (!Directory.Exists(aimPath)) Directory.CreateDirectory(aimPath);
            string[] fileList = Directory.GetFileSystemEntries(srcPath);            
            foreach (string file in fileList) {
                if (isAbort == true) {
                    return;
                }
                if (Directory.Exists(file)){
                    this.CopyDir(file, aimPath + Path.GetFileName(file));             
                }else{
                    bool isReplace = false;
                    if (File.Exists(aimPath + Path.GetFileName(file))){
                        isReplace = true;
                    }
                    string filename = aimPath + Path.GetFileName(file);
                    if (isReplace == true) {

                        FileCopyCanAbort(filename, filename + Guid.NewGuid(), true);

                    }
                    FileCopyCanAbort(file, filename, true);

                    if (File.Exists(filename)){
                        if (isReplace ==true){
                            this.OnReplace(new CollectEventArgs(filename)); 
                        }else{
                            this.OnSuccess(new CollectEventArgs(filename)); 
                        }                         
                    }else{
                        this.OnFail(new CollectEventArgs(filename)); 
                    }
                }
            }
        }
        /// <summary>
        /// 檔案複製之前確認是否目前是否處於放棄狀態
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="isReplace"></param>
        private void FileCopyCanAbort(string src, string dst, bool isReplace) {
            if (isAbort == true) {
                return;
            }
            File.Copy(src, dst, true);
        }
#endregion
        #region 公用方法
        /// <summary>
        /// 放棄檔案收集
        /// </summary>
        public void abort() {
            isAbort = true;
        }
        /// <summary>
        /// 取得檔案中的修改清單
        /// </summary>
        public void getList() {
            StreamReader sw = new StreamReader(this._filelist, Encoding.Default);
            string tmp;
            sw.ReadLine();
            sw.ReadLine();
            while (!sw.EndOfStream) {
                tmp = sw.ReadLine();
                tmp = tmp.Trim();
                this.OnEnumFile(new CollectEventArgs(tmp));
            }            
            sw.Close(); 
        }
        /// <summary>
        /// 從指定檔案進行檔案收集
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool catchFile(string filename) {
            string src;
            string dst;
            string backup = this._srcPath + "backup";
            string new_filename = "";
            if (Directory.Exists(backup) == false  ){
                Directory.CreateDirectory(backup);   
            }
            filename = filename.Trim();
            if (filename.Length > 0) {
                filename = this.converSep(filename);
                filename = insertFirstSep(filename);

                src = this._srcPath + filename;
                dst = this._dstPath + filename;
                backup += filename; 
                if (this.isFolder(src) == true) {
                    if (Directory.Exists(src.Remove(src.Length - 1))) {  
                        this.CopyDir(src.Remove(src.Length - 1), dst.Remove(dst.Length - 1));
                    }else{
                        this.NoExist(new CollectEventArgs(dst));
                    }
                } else {
                    // 檢查來源檔案是否存在
                    if (File.Exists(src) == true) {
                        bool isReplace = false;
                        // 檢查目的檔案是否存在
                        if (File.Exists(dst) == false) {
                            // 如果父目錄不存在則建立
                            string folder = Directory.GetParent(dst).ToString();
                            if (Directory.Exists(folder) == false) {
                                Directory.CreateDirectory(folder);
                            }
                        } else {
                            isReplace = true;
                            // 備份已存在的檔案到 backup                            
                            new_filename = string.Format("{0}.{1}", backup, Guid.NewGuid());
                            string parent_folder = Directory.GetParent(new_filename).ToString() ;
                            if (Directory.Exists(parent_folder) == false) {
                                Directory.CreateDirectory(parent_folder);  
                            }
                            FileCopyCanAbort(dst, new_filename, true);                                  
                            //FileCopyCanAbort(dst, dst + Guid.NewGuid(), true);                                  
                        }
                        //if (isReplace == true) {
                            //FileCopyCanAbort(dst, dst + Guid.NewGuid(), true);                                  
                        //}
                        FileCopyCanAbort(src, dst, true);

                        if (File.Exists(dst)) {
                            if (isReplace == true) {                                    
                                //this.OnReplace(new CollectEventArgs(dst));
                                this.OnReplace(new CollectEventArgs( string.Format("{0} => {1}", dst, new_filename) ));
                            } else {
                                this.OnSuccess(new CollectEventArgs(dst));
                            }
                        } else {
                            this.OnFail(new CollectEventArgs(dst));
                        }
                    } else {
                        this.NoExist(new CollectEventArgs(dst));
                    }
                }

            }
            return true;
        }
        /// <summary>
        /// 從指定的檔案清單進行檔案收集
        /// </summary>
        /// <param name="list">檔案清單</param>
        public void CatchByList(string list) {
            string[] dataSet = list.Split(new char[] { '\r', '\n' });
            for (int i = 0; i < dataSet.Length; ++i) {
                if (isAbort == true) {
                    break;
                }                                
                catchFile( dataSet[i] );
            }
        }
        

        /// <summary>
        /// 開始收集檔案
        /// </summary>
        public void Go() {
            StreamReader sw = new StreamReader(this._filelist, Encoding.Default);
            
            string tmp;
            sw.ReadLine();
            sw.ReadLine();
             
            while (!sw.EndOfStream) {
                if (isAbort == true) {
                    break;
                }
                //System.Windows.Forms.Application.DoEvents();
                tmp = sw.ReadLine();
                tmp = tmp.Trim();
                if (tmp.Length > 0) {
                    tmp = this.converSep(tmp);
                    tmp = insertFirstSep(tmp);

                    string src = this._srcPath + tmp;
                    string dst = this._dstPath + tmp;

                    if (this.isFolder(src) == true) {
                        if (Directory.Exists(src.Remove(src.Length - 1))) {  
                            this.CopyDir(src.Remove(src.Length - 1), dst.Remove(dst.Length - 1));
                        }else{
                            this.NoExist(new CollectEventArgs(dst));
                        }
                    } else {
                        if (File.Exists(src) == true) {
                            bool isReplace = false;
                            if (File.Exists(dst) == false) {
                                string folder = Directory.GetParent(dst).ToString();
                                if (Directory.Exists(folder) == false) {
                                    Directory.CreateDirectory(folder);
                                }
                            } else {
                                isReplace = true;
                            }
                            if (isReplace == true) {
                                //FileCopyCanAbort(dst, dst + Guid.NewGuid(), true);
                                
                                FileCopyCanAbort(dst, dst + Guid.NewGuid(), true);                                  
                            }
                            FileCopyCanAbort(src, dst, true);

                            if (File.Exists(dst)) {
                                if (isReplace == true) {                                    
                                    this.OnReplace(new CollectEventArgs(dst));
                                } else {
                                    this.OnSuccess(new CollectEventArgs(dst));
                                }
                            } else {
                                this.OnFail(new CollectEventArgs(dst));
                            }
                        } else {
                            this.NoExist(new CollectEventArgs(dst));
                        }
                    }
                }
                
            }
                         
            sw.Close();
        }
        #endregion
    }
        
}
