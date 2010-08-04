using System;
using System.Runtime.InteropServices;

namespace ChuiWenChiu.Windows {
    /// <summary>
    /// OS Operator
    /// </summary>
    public class OSUtility {
        #region Native
        [DllImport("user32.dll")]
        private static extern void LockWorkStation();
        [DllImport("user32.dll")]
        private static extern int ExitWindowsEx(int uFlags, int dwReason);
        #endregion

        #region method
        /// <summary>
        /// 關機
        /// </summary>
        public static void Shutdown() {
            ExitWindowsEx(1, 0);
        }

        /// <summary>
        /// 重新啟動
        /// </summary>
        public static void  Reboot() {
            ExitWindowsEx(2, 0);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public static void  Logoff() {
            ExitWindowsEx(0, 0);
        }

        /// <summary>
        /// 鎖定
        /// </summary>
        public static void LockStation() {
            LockWorkStation();
        }

        // 省電模式
        // Application.SetSuspendState(PowerState.Hibernate, true, true);

        // 待機
        // Application.SetSuspendState(PowerState.Suspend, true, true);
        #endregion
    }
}
