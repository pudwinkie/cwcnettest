namespace cwc
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// HotKey.RegisterHotKey(this.Handle, 100, HotKey.KeyModifiers.Alt, Keys.A);
	/// HotKey.UnregisterHotKey(this.Handle, 100);
    /// protected override void WndProc(ref Message m)
    /// {
    ///    if (m.get_Msg() == 0x312)
    ///    {
    ///        switch (m.get_WParam().ToInt32())
    ///        {
    ///            case 100:
    ///                this.button1.PerformClick();
    ///                break;
    ///
    ///        }
    ///    }
    ///    base.WndProc(ref m);
    /// }        
    /// </example>
    internal class HotKey
    {
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [Flags]
        public enum KeyModifiers
        {
            Alt = 1,
            Ctrl = 2,
            None = 0,
            Shift = 4,
            WindowsKey = 8
        }
    }
}

