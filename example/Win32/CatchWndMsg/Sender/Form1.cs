using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Sender {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private const Int32 WM_USER = 0x0400;
        private const Int32 WM_SEND_STRUCT = WM_USER + 1;
        private const Int32 WM_SEND_STRING = WM_USER + 2;
        private const Int32 WM_SEND_INT32 = WM_USER + 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            IntPtr hWnd = FindWindow("WindowsForms10.Window.8.app.0.378734a", "Form1");

            const String sdata = "Hello, Chui-Wen Chiu";
            SendMessage(hWnd, WM_SEND_STRING, IntPtr.Zero, Marshal.StringToBSTR(sdata));            

            //Int32 idata = 99;
            //IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(idata));
            //Marshal.WriteInt32(buffer, idata);
            ////SendMessage(Handle, WM_SEND_INT32, buffer, IntPtr.Zero);
            //SendMessage(hWnd, WM_SEND_INT32, IntPtr.Zero, buffer);
            //Marshal.FreeHGlobal(buffer); 

        }
    }
}