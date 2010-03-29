using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CatchWndMsg {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private const Int32 WM_USER = 0x0400;
        private const Int32 WM_SEND_STRUCT = WM_USER + 1;
        private const Int32 WM_SEND_STRING = WM_USER + 2;
        private const Int32 WM_SEND_INT32  = WM_USER + 3;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// �T���B�z
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            // �S�w�T���B�z
            switch(m.Msg){
            case WM_SEND_STRUCT:
                     
                Point data = (Point)Marshal.PtrToStructure(m.LParam, typeof(Point));                                
                Text = "WM_SEND_STRUCT: " + data;
                break;
            case WM_SEND_STRING:            
                String sdata = Marshal.PtrToStringBSTR(m.LParam);
                Text = "WM_SEND_STRING: " + sdata;
            
                break;         
            case WM_SEND_INT32:
                Int32 idata = Marshal.ReadInt32(m.LParam);                      
                Text = "WM_SEND_INT32: " + idata.ToString();
                
                break;
            default:
                // �T���B�z���`���`�y�{
                base.WndProc(ref m);
                break;
            }
        }

        /// <summary>
        /// �ǰe�S�w���c
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendStruct_Click(object sender, EventArgs e) {
            Point data = new Point(100, 200);
            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(data));
            
            Marshal.StructureToPtr(data, buffer, true);
            SendMessage(Handle, WM_SEND_STRUCT, IntPtr.Zero, buffer );
            Marshal.FreeHGlobal(buffer); 
            
        }

        /// <summary>
        /// �ǰe String ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendString_Click(object sender, EventArgs e) {
            const String sdata = "Hello, Chui-Wen Chiu";
            SendMessage(Handle, WM_SEND_STRING, IntPtr.Zero, Marshal.StringToBSTR( sdata ));            
        }

        /// <summary>
        /// �ǰe Int32 �ƭ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendInt32_Click(object sender, EventArgs e) {
            Int32 idata = 99;
            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(idata));
            Marshal.WriteInt32(buffer, idata);
            //SendMessage(Handle, WM_SEND_INT32, buffer, IntPtr.Zero);
            SendMessage(Handle, WM_SEND_INT32, IntPtr.Zero, buffer );
            Marshal.FreeHGlobal(buffer); 
        }


    }
}