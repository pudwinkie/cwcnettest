using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using WinShell;

namespace Sample1 {


    public partial class Sample1 : Form {
        public Sample1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            using (CShellFolder sf = new CShellFolder()) {
                sf.FetchFileEventHandler += new EventHandler<CShellFolder.FetchFileEventArgs>(sf_FetchFileEventHandler);
                sf.FetchFolderEventHandler += new EventHandler<CShellFolder.FetchFolderEventArgs>(sf_FetchFolderEventHandler);
                sf.DoFatch(@"d:\");
            }
        }

        void sf_FetchFolderEventHandler(object sender, CShellFolder.FetchFolderEventArgs e) {
            lvFile.Items.Add(e.Folder, 1);
        }

        void sf_FetchFileEventHandler(object sender, CShellFolder.FetchFileEventArgs e) {
            lvFile.Items.Add(e.Filename, 0);
        }
    }

}