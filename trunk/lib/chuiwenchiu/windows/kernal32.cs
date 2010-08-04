using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ChuiWenChiu.Win32 {
    public class kernal32 {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern int GetShortPathName(
         string lpszLongPath,
         string shortFile,
         int cchBuffer
        );

        [DllImport("kernel32.dll")]
        public static extern ushort GlobalAddAtom(IntPtr Name);
        [DllImport("kernel32.dll")]
        public static extern ushort GlobalDeleteAtom(ushort atom);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalLock(IntPtr hMem);
        [DllImport("kernel32.dll")]
        public static extern bool GlobalUnlock(IntPtr hMem);

        /// ＜summary>
        /// 添加GetPrivateProfileInt等6個API函數的聲明和FILE_NAME常量的聲明
        /// ＜/summary>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileInt(
            string lpApplicationName,
            string lpKeyName,
            int nDefault,
            string lpFileName
        );

        [DllImport("kernel32")]
        public static extern bool GetPrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
        );

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpString,
            string lpFileName
        );

        [DllImport("kernel32")]
        public static extern bool GetPrivateProfileSection(
            string lpAppName,
            StringBuilder lpReturnedString,
            int nSize,
            string lpFileName
        );

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileSection(
            string lpAppName,
            string lpString,
            string lpFileName
        );

        #region memory 
        [DllImport("kernal32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int size);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);


        [DllImport("kernel32.dll")]
        public static extern IntPtr GlobalFree(IntPtr hMem);

        [DllImport("kernel32.dll")]
        public static extern UIntPtr GlobalSize(IntPtr hMem);

        public const uint GMEM_DDESHARE = 0x2000;
        public const uint GMEM_MOVEABLE = 0x2;
        #endregion
    }
}
