using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GridViewTest {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {           

            int idx = dataGridView1.Rows.Add();
            //int idx = dataGridView1.Rows.Count - 1;
            DataGridViewRow row = dataGridView1.Rows[idx];
            row.Cells["colTextbox"].Value = idx.ToString();
            row.Cells["colLink"].Value = "http://www.sunnet.com.tw";

            DataGridViewButtonCell bc = row.Cells["colButton"] as DataGridViewButtonCell;
            //bc.UseColumnTextForButtonValue = true;
            bc.Value = "Button";
            bc.Tag = "";
            //dataGridView1.Rows[idx].Cells["verNew"].Value = aiNew.Version;
            //dataGridView1.Rows[idx].Cells["verOld"].Value = aiOld.Version;
            //dataGridView1.Rows[idx].Cells["logUrl"].Value = aiNew.ChangeLogUrl;
            //dataGridView1.Rows[idx].Cells["size"].Value = String.Empty;
            //dataGridView1.Rows[idx].Cells["progress"].Value = "©|¥¼¤U¸ü";
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) {
                return;
            }

            DataGridViewColumn col = dataGridView1.Columns[e.ColumnIndex];
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (col is DataGridViewLinkColumn) {                
                //MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells["colLink"].Value.ToString());
                System.Diagnostics.Process.Start(dataGridView1.Rows[e.RowIndex].Cells["colLink"].Value.ToString());
            } else if (col is DataGridViewButtonColumn) {
                if (col.Name == "colButton") {
                    if (dataGridView1.Rows[e.RowIndex].Cells["colButton"].Tag == null) {
                        dataGridView1.Rows[e.RowIndex].Cells["colButton"].Tag = String.Empty;
                        MessageBox.Show("Click Button");
                    }
                } else if (col.Name == "colNewButton") {
                    MessageBox.Show("New Button");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (!dataGridView1.Columns.Contains("colNewButton")) {
                DataGridViewButtonColumn colButton = new DataGridViewButtonColumn();
                colButton.Text = "New";
                colButton.Name = "colNewButton";
                this.dataGridView1.Columns.Add(colButton);
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            if (dataGridView1.Columns.Contains("colNewButton")){
                dataGridView1.Columns.Remove("colNewButton");            
            }

        }
    }
}