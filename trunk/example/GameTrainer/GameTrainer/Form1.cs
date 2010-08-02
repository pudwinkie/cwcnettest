/* GameTrainer 1.0
 Copyright (C) 2007 Luca Tagliaferri

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

 Copyright (C) 2007 Luca Tagliaferri
 e-mai luca.tagliaferri@gmail.com
*/

 
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime;

namespace GameTrainer
{
    unsafe public partial class Form1 : Form
    {
      
        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindow(IntPtr hWnd, int wCmd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool IsWindow(IntPtr hWnd);

        private System.Collections.Generic.List<GameTrainerProcess> processList = new System.Collections.Generic.List<GameTrainerProcess>(10);

        public Form1()
        {
            InitializeComponent();
        }

        private Form2 varForm;

        private void button1_Click(object sender, EventArgs e)
        {
            varForm= new Form2();
            varForm.ProgramName.Text = this.comboBox1.SelectedItem.ToString();
            varForm.Process = this.comboBox1.SelectedItem as GameTrainerProcess;
            varForm.Show();
        
        }

        private void FillCombo()
        {
            this.comboBox1.Items.Clear();
            IntPtr desk = GetDesktopWindow();
            desk = GetWindow(desk, 5);
            IntPtr next = desk;
            System.Collections.Generic.List<string> myList = new List<string>(100);

            while (next != IntPtr.Zero)
            {
                int textLength = GetWindowTextLength(next);
                StringBuilder sb2 = new StringBuilder(textLength + 1);
                GetWindowText(next, sb2, 200);

                if (IsWindowVisible(next) && sb2.Length > 0)
                {
                    uint pid;
                    GetWindowThreadProcessId(next, out pid);
                    this.comboBox1.Items.Add(new GameTrainerProcess(pid,next, sb2.ToString()));

                }
                next = GetWindow(desk, 2);
                desk = next;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FillCombo();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FillCombo();
        }
    }

    


}