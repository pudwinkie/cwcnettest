using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 這行程式碼會將資料載入 'wM_10001DataSet.WM_user_account' 資料表。您可以視需要進行移動或移除。
            this.wM_user_accountTableAdapter.Fill(this.wM_10001DataSet.WM_user_account);

        }
    }
}
