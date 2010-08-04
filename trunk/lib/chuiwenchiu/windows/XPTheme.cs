using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;   
namespace ChuiWenChiu.Win32.Theme{
	/// <summary>
    /// 判斷 XP 的佈景主題
    /// </summary>
    /// <seealso cref="http://www.codeproject.com/csharp/xptheme.asp"/>
    public class XPTheme {
        [DllImport("UxTheme")]
        public static extern bool IsThemeActive();

        [DllImport("UxTheme")]
        public static extern bool IsAppThemed();

        public enum Theme {
            WindowsClassic,
            XPBlue,
            XPGreen,
            XPSilver

        }

        /// <summary>
        /// 目前的佈景主題
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ThemeManager
        /// ThemeActive is "1" if Windows XP, "0" if Windows Classic.
        /// If ThemeActive is "1", ColorName will be "NormalColor" for blue, "HomeStead" for olive green, or "Metallic" for silver. (All of these data values are type REG_SZ, or strings, by the way.)
        /// </remarks>
        public static Theme CurrentTheme() {
            RegistryKey key =
                Registry.CurrentUser.OpenSubKey(
                    @"Software\Microsoft\Windows\CurrentVersion\ThemeManager");
            if (key != null) {
                if ("1" == (string)key.GetValue("ThemeActive")) {
                    string s = (string)key.GetValue("ColorName");
                    if (s != null) {
                        if (String.Compare(s, "NormalColor", true) == 0)
                            return Theme.XPBlue;
                        if (String.Compare(s, "HomeStead", true) == 0)
                            return Theme.XPGreen;
                        if (String.Compare(s, "Metallic", true) == 0)
                            return Theme.XPSilver;
                    }
                }
            }
            return Theme.WindowsClassic;

        }
    }
}