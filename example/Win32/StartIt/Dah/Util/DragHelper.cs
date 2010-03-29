namespace Dah.Util
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DragHelper
    {
        private Control _dragHandler;
        private Control _dragTarget;
        private bool _isDragging;
        private int _x;
        private int _y;
        public const int HT_CAPTION = 2;
        public const int WM_NCLBUTTONDOWN = 0xa1;

        public DragHelper(Control dragHandler, Control dragTarget, bool usePInvoke)
        {
            this._dragHandler = dragHandler;
            this._dragTarget = dragTarget;
            if (!usePInvoke)
            {
                this._dragHandler.MouseDown += new MouseEventHandler(this._dragHandler_MouseDown);
                this._dragHandler.MouseMove += new MouseEventHandler(this._dragHandler_MouseMove);
                this._dragHandler.MouseUp += new MouseEventHandler(this._dragHandler_MouseUp);
            }
            else
            {
                ((Form) this._dragTarget).MouseMove += new MouseEventHandler(this._dragTarget_MouseMove2);
            }
        }

        private void _dragHandler_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this._x = e.X;
                this._y = e.Y;
                this._isDragging = true;
            }
        }

        private void _dragHandler_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._isDragging)
            {
                this._dragTarget.Left = (this._dragTarget.Left - this._x) + e.X;
                this._dragTarget.Top = (this._dragTarget.Top - this._y) + e.Y;
            }
        }

        private void _dragHandler_MouseUp(object sender, MouseEventArgs e)
        {
            this._isDragging = false;
        }

        private void _dragTarget_MouseMove2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(((Form) this._dragTarget).Handle, 0xa1, 2, 0);
            }
        }

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}

