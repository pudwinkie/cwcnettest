#region
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endregion

namespace ChuiWenChiu.Win32 {    
    /// <summary>
    /// 動態載入 DLL 並將 Function 轉成 Delegate
    /// </summary>
    /// <example>
    /// DynamicNative dn = new DynamicNative("user32.dll");
    /// MsgBox mb = (MsgBox)dn.GetDelegate("MessageBoxA", typeof(MsgBox));
    /// mb.Invoke(0, "xyz", "title", 64);
    /// mb = (MsgBox)dn.GetDelegate("MessageBoxA", typeof(MsgBox));
    /// mb.Invoke(0, "xyz", "title", 64); 
    /// dn.Dispose(); 
    /// </example> 
    public class DynamicNative : IDisposable {
        #region Native Api
        [DllImport("Kernel32")]
        public static extern int GetProcAddress(int handle, String funcname);
        [DllImport("Kernel32")]
        public static extern int LoadLibrary(String funcname);
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(int handle);
        #endregion

        #region field
        private int moduleHandle = 0;
        private Dictionary<string, Delegate> functionTable;  // catch 機制
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="moduleName"></param>
        public DynamicNative(string moduleName) {
            moduleHandle = LoadLibrary(moduleName);
            functionTable = new Dictionary<string, Delegate>();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            FreeLibrary(moduleHandle);
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="functionName">函數名稱</param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Delegate GetDelegate(string functionName, Type t) {
            if (functionTable.ContainsKey(functionName)) {
                return functionTable[functionName];
            }

            int addr = GetProcAddress(moduleHandle, functionName);
            if (addr == 0) {
                return null;
            } else {
                Delegate d = Marshal.GetDelegateForFunctionPointer(new IntPtr(addr), t);
                functionTable.Add(functionName, d);
                return d;
            }
        }
    }

    
}
