using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChuiWenChiu.Win32 {

    /// <summary>
    /// 幫助實現操作系統菜單的類的定義
    /// </summary>
    public class SystemMenu {
        //注意：用P/Invoke調用動態鏈接庫中非託管函數時，應執行如下步驟：
        //1，定位包含該函數的DLL。
        //2，把該DLL庫裝載入內存。
        //3，找到即將調用的函數地址，並將所有的現場壓入堆棧。
        //4，調用函數。
        //
        
        // 我們需要GetSystemMenu() 函數
        // 注意這個函數沒有Unicode 版本

        private IntPtr m_SysMenu = IntPtr.Zero; // 系統菜單句柄

        private SystemMenu() {
        }
        
        /// <summary>
        /// 從一個Form對象檢索一個新對象
        /// </summary>
        /// <param name="Frm"></param>
        /// <returns></returns>
        public static SystemMenu FromForm(Form Frm) {
            SystemMenu cSysMenu = new SystemMenu();
            cSysMenu.m_SysMenu = User32.apiGetSystemMenu(Frm.Handle, 0);
            if (cSysMenu.m_SysMenu == IntPtr.Zero) { // 一旦失敗，引發一個異常
                throw new NoSystemMenuException();
            }
            return cSysMenu;
        }
        
        /// <summary>
        /// 當前窗口菜單還原  
        /// </summary>
        /// <param name="Frm"></param>
        public static void ResetSystemMenu(Form Frm) {
            User32.apiGetSystemMenu(Frm.Handle, 1);
        }

        /// <summary>
        /// 在給定的位置（以0為索引開始值）插入一個分隔條 
        /// </summary>
        /// <param name="Pos"></param>
        /// <returns></returns>
        public bool InsertSeparator(int Pos) {
            return (InsertMenu(Pos, ItemFlags.mfSeparator | ItemFlags.mfByPosition, 0, ""));
        }

        #region InsertMenu
        /// <summary>
        /// 簡化的InsertMenu()，前提──Pos參數是一個0開頭的相對索引位置 
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="ID"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool InsertMenu(int Pos, int ID, String Item) {
            return (InsertMenu(Pos, ItemFlags.mfByPosition | ItemFlags.mfString, ID, Item));
        }

        
        /// <summary>
        /// 在給定位置插入一個菜單項。具體插入的位置取決於Flags 
        /// </summary>
        /// <param name="Pos"></param>
        /// <param name="Flags"></param>
        /// <param name="ID"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool InsertMenu(int Pos, ItemFlags Flags, int ID, String Item) {
            return (User32.apiInsertMenu(m_SysMenu, Pos, (Int32)Flags, ID, Item) == 0);
        }
        #endregion


        /// <summary>
        /// 添加一個分隔條
        /// </summary>
        /// <returns></returns>
        public bool AppendSeparator() {
            return AppendMenu(0, String.Empty, ItemFlags.mfSeparator);
        }

        #region AppendMenu
        /// <summary>
        /// 附加一個新的選單
        /// 使用ItemFlags.mfString 作為缺省值
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public bool AppendMenu(int ID, String Item) {
            return AppendMenu(ID, Item, ItemFlags.mfString);
        }
        
        /// <summary>
        /// 附加一個新的選單
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Item"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        public bool AppendMenu(int ID, String Item, ItemFlags Flags) {
            return (User32.apiAppendMenu(m_SysMenu, (int)Flags, ID, Item) == 0);
        }
        #endregion

                
        /// <summary>
        /// 檢查是否一個給定的ID在系統菜單ID範圍之內 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static bool VerifyItemID(int ID) {
            return (bool)(ID < 0xF000 && ID > 0);
        }
    }

    /// <summary>
    /// 不具有系統選單
    /// </summary>
    public class NoSystemMenuException : System.Exception {
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ItemFlags {
        // The item ...
        mfUnchecked = 0x00000000, // ... is not checked
        mfString = 0x00000000, // ... contains a string as label
        mfDisabled = 0x00000002, // ... is disabled
        mfGrayed = 0x00000001, // ... is grayed
        mfChecked = 0x00000008, // ... is checked
        mfPopup = 0x00000010, // ... Is a popup menu. Pass the

        // menu handle of the popup
        // menu into the ID parameter.

        mfBarBreak = 0x00000020, // ... is a bar break
        mfBreak = 0x00000040, // ... is a break
        mfByPosition = 0x00000400, // ... is identified by the position
        mfByCommand = 0x00000000, // ... is identified by its ID
        mfSeparator = 0x00000800 // ... is a seperator (String and

        // ID parameters are ignored).
    }

    /// <summary>
    /// 視窗訊息
    /// </summary>
    public enum WindowMessages {
        wmSysCommand = 0x0112
    }

}
