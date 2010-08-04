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
        /// ����
        /// </summary>
        public static void Shutdown() {
            ExitWindowsEx(1, 0);
        }

        /// <summary>
        /// ���s�Ұ�
        /// </summary>
        public static void  Reboot() {
            ExitWindowsEx(2, 0);
        }

        /// <summary>
        /// �n�X
        /// </summary>
        public static void  Logoff() {
            ExitWindowsEx(0, 0);
        }

        /// <summary>
        /// ��w
        /// </summary>
        public static void LockStation() {
            LockWorkStation();
        }

        // �ٹq�Ҧ�
        // Application.SetSuspendState(PowerState.Hibernate, true, true);

        // �ݾ�
        // Application.SetSuspendState(PowerState.Suspend, true, true);
        #endregion
    }
}
