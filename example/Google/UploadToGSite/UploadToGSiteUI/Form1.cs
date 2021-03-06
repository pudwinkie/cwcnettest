﻿using System;
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
            lstData.SimpleDrapDrop<string[]>(DataFormats.Text, x => {
                lstData.Items.AddRange(x);
            });
        }

        private void button1_Click(object sender, EventArgs e) {
            //if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                //txtDir.Text = folderBrowserDialog1.SelectedPath;
            //}
        }

        private void btnStart_Click(object sender, EventArgs e) {
            lstResult.Items.Clear();
            String domain = String.Empty;// = match.Groups[1].Value;
            String site_name = String.Empty;// = match.Groups[2].Value;
            if (!Helper.IsValidGSiteUrl(txtUrl.Text, ref domain, ref site_name)) {
                lstResult.Items.Add("Google Sites Url Error");
                return;
            }

            //if (!Directory.Exists(txtDir.Text)) {
              //  lstResult.Items.Add("Path Error");
                //return;
            //}

            String id = txtId.Text;
            String pwd = txtPwd.Text;
            //String src_path = txtDir.Text;
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

                object[] col = new Object[lstData.Items.Count];                
                lstData.Items.CopyTo(col, 0);
                backgroundWorker1.RunWorkerAsync(new WorkItem() {
                    Path = col,
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
            var file_list = from f in args.Path
                            select f.ToString();

            foreach (var filename in file_list) {
                var fi = new FileInfo(filename);
                var fn = Guid.NewGuid().ToString().Replace("-", "") + fi.Extension;
                var mime_type = Helper.GetMimeType(filename);
                var result = args.Serv.UpdloadAttachment(filename, mime_type, args.Parent, fn, "");
                if (result == null) {
                    bk.ReportProgress(0, new { fn = filename, state = false });
                }else{
                    bk.ReportProgress(2, new { fn = filename, state = true, url = result.Content.Src });
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            dynamic data = e.UserState;

            if (data.state) {
                lstData.Items.Remove(data.fn);
                lstResult.Items.Add(String.Format("{0}=>{1}", data.fn, data.url));
            } else {
                lstResult.Items.Add(String.Format("{0}=>fail", data.fn));
            }
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            btnStart.Enabled = true;
            btnExport.Enabled = true;
        }

        class WorkItem {
            public Object[] Path { get; set; }
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

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void button1_Click_1(object sender, EventArgs e) {
            String domain = String.Empty;// = match.Groups[1].Value;
            String site_name = String.Empty;// = match.Groups[2].Value;
            if (!Helper.IsValidGSiteUrl(txtUrl.Text, ref domain, ref site_name)) {
                MessageBox.Show("Google Sites Url Error");
                return;
            }

            String x = txtUrl.Text;
            if(!x.EndsWith("/")){
                x+="/";
            }
            x += Helper.STORE_PATH;
            Process.Start(x);
        }

        private void button2_Click(object sender, EventArgs e) {
            lstData.Items.Clear();
        }
    }

    static class DrapDropExtend {
        public static void SimpleDrapDrop<T>(this Control c, string dataformat, Action<T> hanlder) where T : class {
            c.AllowDrop = true;
            c.DragEnter += (s, e) => {
                    // 確定使用者抓進來的是檔案
                    if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true) {
                        // 允許拖拉動作繼續 (這時滑鼠游標應該會顯示 +)
                        e.Effect = DragDropEffects.All;
                    }
                };

            c.DragDrop += (s, e) => {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //foreach (var fn in files) {
                hanlder(files as T);
                //}
            };
        }

        public static void SimpleDrapDrop(this Control c, Action<DragEventArgs> enterHanlder, Action<DragEventArgs> dropHanlder) {
            c.AllowDrop = true;
            c.DragEnter += (s, e) => enterHanlder(e);
            c.DragDrop += (s, e) => enterHanlder(e);
        }

        public static void SimpleDrapDrop(this Control c, DragEventHandler enterHanlder, DragEventHandler dropHanlder) {
            c.AllowDrop = true;
            c.DragEnter += enterHanlder;
            c.DragDrop += dropHanlder;
        }
    }
}
