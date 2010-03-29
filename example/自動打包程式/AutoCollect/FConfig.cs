using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ChuiWenChiu.Utility;
namespace ChuiWenChiu {
    /// <summary>
    /// 
    /// </summary>
    public partial class FConfig : Form {
        private CXMLConfig _conf;
        /// <summary>
        /// 
        /// </summary>
        public FConfig() {
            InitializeComponent();
            _conf = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conf"></param>
        public void setConfig(object conf) {

            _conf = (CXMLConfig)conf;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            //_conf.SourceServer = txtSourceServer.Text;
            //_conf.SourceFolder = txtSourceFolder.Text;
            //_conf.SMTPServer = txtSMTP.Text;  
            _conf.EncoderServer = txtEncoderServer.Text;
            _conf.EncoderFolder = txtEncoderFolder.Text;
            _conf.EncoderUsername = txtUsername.Text;
            _conf.EncoderPassword = txtPassword.Text;
            _conf.EncoderPort = txtEncoderPort.Text;
            _conf.EncoderShellScript = txtMainShell.Text;  

            _conf.save();
            Close();
        }

        private void frmConfig_Shown(object sender, EventArgs e) {
            txtEncoderFolder.Text   = _conf.EncoderFolder;
            txtEncoderServer.Text   = _conf.EncoderServer;
            //txtProjectTag.Text = _conf.ProjectTag;
            //txtSourceFolder.Text    = _conf.SourceFolder;
            //txtSourceServer.Text    = _conf.SourceServer;
            txtEncoderPort.Text     = _conf.EncoderPort;
            txtMainShell.Text       = _conf.EncoderShellScript;
            //txtSMTP.Text            = _conf.SMTPServer;
            txtUsername.Text        = _conf.EncoderUsername;
            txtPassword.Text        = _conf.EncoderPassword; 
        }


        private void btnCancel_Click(object sender, EventArgs e) {
            Close(); 
        }

        private void FConfig_Load(object sender, EventArgs e) {

        }



    }
}