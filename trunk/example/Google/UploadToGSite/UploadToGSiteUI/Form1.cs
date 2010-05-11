using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GSites;
using System.IO;
using Google.GData.Client;
using Google.GData.Documents.GSites;
using System.Diagnostics;


namespace UploadToGSiteUI {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                txtDir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e) {
            lstResult.Items.Clear();
            String domain = String.Empty;// = match.Groups[1].Value;
            String site_name = String.Empty;// = match.Groups[2].Value;
            if (!Helper.IsValidGSiteUrl(txtUrl.Text, ref domain, ref site_name)) {
                lstResult.Items.Add("Google Sites Url Error");
                return;
            }

            if (!Directory.Exists(txtDir.Text)) {
                lstResult.Items.Add("Path Error");
                return;
            }

            String id = txtId.Text;
            String pwd = txtPwd.Text;
            String src_path = txtDir.Text;
            try {
                var service = new GSiteService(Helper.APP_NAME, domain, site_name);
                service.setUserCredentials(id, pwd);
                var p_feed = service.GetContentFeedPath("/" + Helper.STORE_PATH);
                AtomEntry parent_node = null;
                if (p_feed.Entries.Count == 0) {
                    parent_node = service.CreateWebPage(Helper.STORE_PATH, "x", Helper.STORE_PATH);
                } else {
                    parent_node = p_feed.Entries[0];
                }
                backgroundWorker1.WorkerReportsProgress = true;
                backgroundWorker1.RunWorkerAsync(new WorkItem() {
                    Path = src_path,
                    Serv = service,
                    Parent = parent_node
                });
            } catch (InvalidCredentialsException ex) {
                lstResult.Items.Add("帳密錯誤");
                return;
            } catch (CaptchaRequiredException ex) {
                lstResult.Items.Add("帳密錯誤");
                return;
            }
            btnStart.Enabled = false;
            btnExport.Enabled = false;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {
            WorkItem args = (WorkItem)e.Argument;
            BackgroundWorker bk = (BackgroundWorker)sender;
            var file_list = from f in Directory.EnumerateFiles(args.Path)
                            select f;

            foreach (var filename in file_list) {
                var fi = new FileInfo(filename);
                var fn = Guid.NewGuid().ToString().Replace("-", "") + fi.Extension;
                var mime_type = Helper.GetMimeType(filename);
                var result = args.Serv.UpdloadAttachment(filename, mime_type, args.Parent, fn, "");
                if (result == null) {
                    bk.ReportProgress(0, String.Format("{0} fail", filename));
                }else{
                    bk.ReportProgress(2, String.Format("{0}=>{1}", filename, result.Content.Src));
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            lstResult.Items.Add(e.UserState.ToString());
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            btnStart.Enabled = true;
            btnExport.Enabled = true;
        }

        class WorkItem {
            public String Path { get; set; }
            public GSiteService Serv { get; set; }
            public AtomEntry Parent { get; set; }
        }

        private void btnExport_Click(object sender, EventArgs e) {
            if (lstResult.Items.Count == 0) {
                return;
            }
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".html";
            dlg.CheckPathExists = true;
            dlg.AddExtension = true;
            dlg.Filter = "HTML|*.html";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                var fn = dlg.FileName;
                var sb = new StringBuilder();
                foreach (var item in lstResult.Items) {
                    var parts=item.ToString().Split(new String[]{"=>"},  StringSplitOptions.RemoveEmptyEntries );
                    if (parts.Length == 2) {
                        sb.AppendFormat("<div><div>{0}</div><img src='{1}'></div><br/>\n", parts[0], parts[1]);
                    }
                }

                File.WriteAllText(fn, File.ReadAllText( Path.Combine(Application.StartupPath,"template.htm")).Replace("{0}", sb.ToString()));
                Process.Start(fn);
            }
        }
    }
}
