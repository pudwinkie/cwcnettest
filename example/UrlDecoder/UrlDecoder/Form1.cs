using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web; 
namespace UrlDecoder {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void btnDecode_Click(object sender, EventArgs e) {
            txtDecode.Text = HttpUtility.UrlDecode(txtEncode.Text);   
        }
    }
}