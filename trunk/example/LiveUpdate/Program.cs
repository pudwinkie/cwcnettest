using System;
using System.Collections.Generic;
using System.Windows.Forms;
//using ChuiWenChiu.Utility;

namespace ChuiWenChiu.LiveUpdate {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            string applicationPath = null;
            if (args.Length <= 0) {
                System.IO.FileInfo fi = new System.IO.FileInfo(Application.ExecutablePath);
                applicationPath = fi.DirectoryName;

                if (applicationPath.EndsWith("update") == true) {
                    applicationPath = applicationPath.Replace("update", "");
                }
                if (DialogResult.No == MessageBox.Show("Is '" + applicationPath + "' Agent's install path?", "Question?", MessageBoxButtons.YesNo)) {
                    return;
                }                
            } else {
                applicationPath = args[0];
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            string executeFile = applicationPath + "update/setup.exe";
            string unrar = applicationPath + "update/unrar.exe";
            FDonwload frmEntry = new FDonwload();            
            frmEntry.AddTask("http://61.221.176.159:2527/agent/update/agent.exe", executeFile);
            frmEntry.Shown += delegate {
                frmEntry.Start();  
            };

            frmEntry.DownloadComplete += delegate {
                frmEntry.Update(unrar, String.Format("x -o+ {0} \"{1}\"", executeFile, applicationPath));  
            };

            frmEntry.UpdateComplete += delegate {
                MessageBox.Show("Update Finish");
                frmEntry.Close(); 
            };
            Application.Run(frmEntry);
        }
    }
}