using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UploadToGSite {
    

    public partial class frmLogin : Form {
        public frmLogin() {
            InitializeComponent();           
            
        }

        private void bnCancel_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Id = txtId.Text;
            Password = txtPwd.Text;
        }

        public String Id { get; set; }
        public String Password { get; set; }

        private void frmLogin_Load(object sender, EventArgs e) {

        }
    }
}
