using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InstallHelper {
    public partial class IPControl : UserControl {
        public IPControl() {
            InitializeComponent();
            txtA.Text = "0";
            txtB.Text = "0";
            txtC.Text = "0";
            txtD.Text = "0";
        }

        public String IPValue {
            get {
                return String.Format("{0}.{1}.{2}.{3}", txtA.Text, txtB.Text, txtC.Text, txtD.Text);
            }

            set {
                string[] tmp = value.Split('.');
                
                if (tmp.Length != 4) {
                    throw new FormatException();
                }

                txtA.Text = ExtractPart(tmp[0]);
                txtB.Text = ExtractPart(tmp[1]);
                txtC.Text = ExtractPart(tmp[2]);
                txtD.Text = ExtractPart(tmp[3]);                               
            }
        }

        private static string ExtractPart(string tmp) {
            if (String.IsNullOrEmpty(tmp)) {
                return String.Empty;
            }

            int v = Int32.Parse(tmp);
            if (v < 0 || v > 255) {
                throw new FormatException();
            }

            return v.ToString();
        }
        public override string ToString() {
            return IPValue;
        }
    }
}
