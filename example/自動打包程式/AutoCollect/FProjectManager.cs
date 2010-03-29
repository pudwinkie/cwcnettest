using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ChuiWenChiu.Utility;
namespace AutoCollect {
    public partial class FProjectManager : Form {
        private CWMVersions _wmv = null;

        public FProjectManager() {
            InitializeComponent();
        }

        private void FProjectManager_Load(object sender, EventArgs e) {        
        
        }

        public void SetWM(CWMVersions wmv) {
            _wmv = wmv;
        }

        private void FProjectManager_Shown(object sender, EventArgs e) {
            for (int i = 0; i < _wmv.Count; ++i) {
                lstWM.Items.Add(_wmv[i].ID);     
            }            
        }

        private void DisplayProjects(CWMProjects wmp) {
            lstProject.Items.Clear();   
            for (int j = 0; j < wmp.Count; ++j) {
                lstProject.Items.Add( wmp[j].tag);            
            }            
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e) {
            Close();
        }

        private void lstWM_Click(object sender, EventArgs e) {

        }

        private void lstWM_SelectedIndexChanged(object sender, EventArgs e) {            
            if (lstWM.SelectedIndex != -1) {
                txtWMName.Text  = _wmv[lstWM.SelectedIndex].ID;
                txtWMIP.Text =  _wmv[lstWM.SelectedIndex].IP ;
                txtWMDir.Text =  _wmv[lstWM.SelectedIndex].WorkDir;
                DisplayProjects(_wmv[lstWM.SelectedIndex].Projects);   
            }
        }

        private void lstProject_SelectedIndexChanged(object sender, EventArgs e) {
            
            if (lstProject.SelectedIndex != -1) {
                CWMProject wmp = _wmv[lstWM.SelectedIndex].Projects[lstProject.SelectedIndex];  
                txtProjectName.Text =  wmp.tag;
                txtProjectDesc.Text = wmp.name;
            }
        }
    }
}