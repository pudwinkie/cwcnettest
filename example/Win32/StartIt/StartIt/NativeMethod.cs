namespace StartIt
{
    using System;
    using System.Runtime.InteropServices;

    public class NativeMethod
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0;
        public const uint SHGFI_SMALLICON = 1;

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst=80)]
            public string szTypeName;
        }
    }
}

