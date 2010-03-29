using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        private Object oDocument; 

        public Form1() {
            InitializeComponent();

            this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
            //this.Load += new System.EventHandler(this.Form1_Load);
            //this.Closed += new System.EventHandler(this.Form1_Closed);

        }

        private void Form1_Load(object sender, EventArgs e) {
            button1.Text = "Browse";
            openFileDialog1.Filter = "Office Documents(*.doc, *.xls, *.ppt)|*.doc;*.xls;*.ppt";
            openFileDialog1.FilterIndex = 1;

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            oDocument = null;

        }

        private void axWebBrowser1_NavigateComplete2(object sender, AxSHDocVw.DWebBrowserEvents2_NavigateComplete2Event e) {
            //Note: You can use the reference to the document object to 
            //      automate the document server.

            Object o = e.pDisp;

            oDocument = o.GetType().InvokeMember("Document", BindingFlags.GetProperty, null, o, null);

            Object oApplication = o.GetType().InvokeMember("Application", BindingFlags.GetProperty, null, oDocument, null);

            Object oName = o.GetType().InvokeMember("Name", BindingFlags.GetProperty, null, oApplication, null);

            MessageBox.Show("File opened by: " + oName.ToString()); 

        }

        private void button1_Click(object sender, EventArgs e) {
            String strFileName;

            //Find the Office document.
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                strFileName = openFileDialog1.FileName;

                //If the user does not cancel, open the document.
                if (strFileName.Length != 0) {
                    Object refmissing = System.Reflection.Missing.Value;
                    oDocument = null;
                    axWebBrowser1.Navigate(strFileName, ref refmissing, ref refmissing, ref refmissing, ref refmissing);
                }
            }

        }
    }
}
