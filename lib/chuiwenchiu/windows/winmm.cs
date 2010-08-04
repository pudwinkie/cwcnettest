using System;
using System.Runtime.InteropServices;

namespace ChuiWenChiu.Win32 {
    public class winmm {

        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
         string lpstrCommand,
         string lpstrReturnString,
         int uReturnLength,
         int hwndCallback
        );
    }
}
