using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TestDataGridView
{
    public partial class Form1 : Form
    {
        ManageDgv Dgv;
        public Form1()
        {
            InitializeComponent();
            InitCol();
        }

        private void InitCol()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");
            dt.Columns.Add("col3");
            for (int i = 1; i < 10; i++)
            {
                dt.Rows.Add("colaa" + i, "colbb" + i, "colcc" + i);
            }
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = dt;
            Dgv = new ManageDgv(this.dataGridView1);
            Dgv.AddMergeColumns(2, 2, "test", -1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if(this.dataGridView1.Columns[e.ColumnIndex].HeaderText.Equals("Column4"))
                {
                    MessageBox.Show ("Hello World");
                }
            }
        }
    }
}