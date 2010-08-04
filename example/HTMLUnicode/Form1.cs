using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HTMLUnicode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://chuiwenchiu.spaces.live.com");
        }

        private string HTML2Unicode(string htm)
        {
            StringBuilder sb = new StringBuilder();
            htm = htm.Replace(" ", "").Replace("&#", "");
            foreach (string s in htm.Split(";".ToCharArray()))
            {

                if ((s.Length > 0))
                {
                    try
                    {
                        if (s.StartsWith("x"))
                        {
                            sb.Append(((char)Convert.ToInt32(s.Substring(1), 16)).ToString());
                        }
                        else
                        {
                            sb.Append(Convert.ToChar(Convert.ToInt32(s)));
                        }
                    }
                    catch  {    }
                }
            }
            return sb.ToString();
        }

        private string Unicode2HTML(string unc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in unc.ToCharArray())
            {
                sb.Append(("&#" + (Int32)c + ";"));
            }
            return sb.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = HTML2Unicode(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = Unicode2HTML(textBox1.Text);
        }
    }
}
