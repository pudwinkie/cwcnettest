using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace WindowsApplication6 {
    public partial class FDonwload : Form {
        #region private

        #region data
        private WebClient wc; 
        private List< KeyValuePair<string, string>  > fileList; 
        #endregion

        #endregion

        public FDonwload() {
            InitializeComponent();

            wc = new WebClient();
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);

            fileList = new List<KeyValuePair<string, string>>(); 
            fileList.Add( new KeyValuePair<string, string>( "http://61.221.176.159:2527/agent/update/agent.exe", "agent.exe") );
            fileList.Add( new KeyValuePair<string, string>( "http://61.221.176.159:2527/agent/update/agent_total.exe", "agent_total.exe") );
            fileList.Add(new KeyValuePair<string, string>("http://61.221.176.159:2527/agent/update/agent_total_error.exe", "agent_total_error.exe"));  
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void btnDownload_Click(object sender, EventArgs e) {
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

        private void DownloadFile(int idx) {
            pbFileProgress.Value = 0;
            lblStatus.Text = String.Format("[{0}/{1}] {2}", idx+1, fileList.Count, fileList[idx].Value);

            wc.DownloadFileAsync(new Uri(fileList[idx].Key), fileList[idx].Value);
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
            }            
        }

        private void progressBar2_Click(object sender, EventArgs e) {

        }

        private void btnCancel_Click(object sender, EventArgs e) {
            if (wc != null) {
                wc.CancelAsync();
            }
        }

        #region properties
        
        #endregion
    }

}