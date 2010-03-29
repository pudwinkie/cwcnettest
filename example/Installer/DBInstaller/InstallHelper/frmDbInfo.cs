using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstallHelper {
    public partial class frmDbInfo : Form {
        public frmDbInfo() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        public String Id {
            get {
                return txtId.Text;                
            }

            set {
                txtId.Text = value;
            }
        }

        public String Password {
            get {
                return txtPwd.Text;
            }

            set {
                txtPwd.Text = value;
            }
        }

        public String Ip {
            get {
                //return txtIP.Text;
                //return ipControl1.ToString();
                return ipControl1.IPValue;
            }

            set {
                ipControl1.IPValue = value;
            }
        }

        public int Port {
            get {
                return 9999;
            }
        }

        public String DbName {
            get {
                return txtDb.Text;
            }

            set {
                txtDb.Text = value;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel; 
        }
    }
}
