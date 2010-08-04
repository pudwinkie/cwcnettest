using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ChuiWenChiu.Win32 {
    public class shell32 {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;
        public const uint SHGFI_SMALLICON = 0x1;
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [DllImport("Shell32", CharSet = CharSet.Auto)]
        public extern static int ExtractIconEx(
            [MarshalAs(UnmanagedType.LPTStr)] 
            string lpszFile,       //size of the icon
            int nIconIndex,        //index of the icon 
            //(in case we have more 
            //then 1 icon in the file
            IntPtr[] phIconLarge,  //32x32 icon
            IntPtr[] phIconSmall,  //16x16 icon
            int nIcons);           //how many to get

        //we need this function to release the unmanaged resource,
        //the unmanaged resource will be 
        //copies to a managed one and it will be returned.
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static bool DestroyIcon(IntPtr handle);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static int ExecuteCommand(string Command, int Timeout) {
            int ExitCode;
            ProcessStartInfo ProcessInfo;
            Process Process;

            ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            Process = Process.Start(ProcessInfo);
            Process.WaitForExit(Timeout);
            ExitCode = Process.ExitCode;
            Process.Close();

            return ExitCode;
        }

    }
}
