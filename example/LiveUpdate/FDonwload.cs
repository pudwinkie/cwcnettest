#region using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
#endregion

namespace ChuiWenChiu.LiveUpdate { 
    /// <summary>
    /// 
    /// </summary>
    public partial class FDonwload : Form {

        #region constructor
        /// <summary>
        /// 
        /// </summary>
        public FDonwload() {
            InitializeComponent();

            wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);

            fileList = new List<KeyValuePair<string, string>>();
        }
        #endregion
        #region event
        public event EventHandler<EventArgs> DownloadComplete;
        protected void OnDownloadComplete(object sender, EventArgs e){
            if (DownloadComplete != null){
                DownloadComplete(sender, e); 
            }
        }

        public event EventHandler<EventArgs> UpdateComplete;
        protected void OnUpdateComplete(object sender, EventArgs e) {
            if (UpdateComplete != null) {
                UpdateComplete(sender, e); 
            }
        }
        #endregion

        #region public

        #region method
        /// <summary>
        /// 新增一個檔案下載
        /// </summary>
        /// <param name="url"></param>
        /// <param name="localName"></param>
        public void AddTask(string url, string localName) {
            fileList.Add(new KeyValuePair<string, string>(url, localName));              
        }

        /// <summary>
        /// 開始下載
        /// </summary>
        public void Start() {
            if (fileList.Count <= 0) {
                // 沒有檔案需要下載
                return;
            }
            pbFileCount.Maximum = fileList.Count;
            pbFileCount.Value = 0;

            btnDownload.Enabled = false;
            btnCancel.Enabled = true;

            DownloadFile(0);
        }

        /// <summary>
        /// 停止下載
        /// </summary>
        public void Stop() {
            if (wc != null) {
                wc.CancelAsync();
            }
        }

        /// <summary>
        /// 執行外部程式
        /// </summary>
        public void Update(string exe, string args) {
            System.Diagnostics.Process.Start(exe, args);
            OnUpdateComplete(this, new EventArgs()); 
        }
        #endregion

        #endregion

        #region private

        #region data
        private WebClient wc;
        private List<KeyValuePair<string, string>> fileList;
        #endregion

        #region method
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idx"></param>
        private void DownloadFile(int idx) {
            pbFileProgress.Value = 0;
            lblStatus.Text = String.Format("[{0}/{1}] {2}", idx+1, fileList.Count, fileList[idx].Value);

            wc.DownloadFileAsync(new Uri(fileList[idx].Key), fileList[idx].Value);
        }


        #endregion
        #endregion

        #region Event Handler
        private void Form1_Load(object sender, EventArgs e) {

        }

        private void btnDownload_Click(object sender, EventArgs e) {
            Start();
        }

        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {            
            pbFileProgress.Value = e.ProgressPercentage; 
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e) {
            if (e.Cancelled) {
                // 使用者取消
                btnCancel.Enabled = false;
                btnDownload.Enabled = !btnCancel.Enabled;
                MessageBox.Show("使用者取消");
                return;
            } 

            if (e.Error != null) {
                // 發生錯誤
                btnCancel.Enabled = false;
                btnDownload.Enabled = !btnCancel.Enabled;
                MessageBox.Show("發生錯誤" + e.Error.Message);
                return;
            } 

            ++pbFileCount.Value;
            // 成功
            if (pbFileCount.Value < pbFileCount.Maximum) {
                DownloadFile(pbFileCount.Value);
            } else {
                btnCancel.Enabled = false;
                btnDownload.Enabled = false;
                OnDownloadComplete(this, new EventArgs());
                //if (MessageBox.Show("下載完成，是否進行安裝？","安裝?", MessageBoxButtons.YesNo) == DialogResult.Yes) {
                //    MessageBox.Show("Installing");                    
                //}
            }
        }

        private void progressBar2_Click(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Stop();
        }
        #endregion

        #region properties

        #endregion
    }

}