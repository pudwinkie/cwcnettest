using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UploadToGSite
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e) {

        }

        private void tsbLogin_Click(object sender, EventArgs e) {
            var frm = new frmLogin();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) {

            }
        }

        private void tsbExportHtml_Click(object sender, EventArgs e) {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".html";
            dlg.CheckPathExists = true;
            dlg.AddExtension = true;
            dlg.Filter = "HTML|*.html";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                var fn = dlg.FileName;
            }
        }

        private void tsbExportCsv_Click(object sender, EventArgs e) {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.CheckPathExists = true;
            dlg.AddExtension = true;
            dlg.Filter = "CSV|*.csv";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                var fn = dlg.FileName;
            }
        }


        private void tsbStart_Click(object sender, EventArgs e) {
            tsbStart.Image = UploadToGSite.Properties.Resources.imgStop;
        }
    }
}
