using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace ChuiWenChiu.Win32 {
    public partial struct User32 {
        public const int GCL_HCURSOR = -12;
        public const int CF_ENHMETAFILE = 14;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo
            (int uAction, int uParam, string lpvParam, int fuWinIni);


        /// <summary>
        /// Win32 API Constants for ShowWindowAsync()
        /// </summary>
        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, int fAttach);

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        public static extern bool UnpackDDElParam(int msg, IntPtr lParam, out IntPtr pLo, out IntPtr pHi);

        [DllImport("user32.Dll")]
        public static extern IntPtr PackDDElParam(int msg, IntPtr pLo, IntPtr pHi);

        [DllImport("user32.Dll")]
        public static extern bool FreeDDElParam(int msg, IntPtr lParam);

        [DllImport("user32.Dll")]
        public static extern IntPtr ReuseDDElParam(IntPtr lParam, int msgIn, int msgOut, IntPtr pLo, IntPtr pHi);

        [DllImport("user32.Dll")]
        public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint SetClassLong(IntPtr hWnd, int index, int dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern uint MessageBeep(int uType);




        [DllImport("USER32", EntryPoint = "GetSystemMenu", SetLastError = true,
        CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr apiGetSystemMenu(IntPtr WindowHandle, int bReset);

        // 還需要AppendMenu()。 既然 .NET 使用Unicode,
        // 我們應該選取它的Unicode版本。

        [DllImport("USER32", EntryPoint = "AppendMenuW", SetLastError = true,
        CharSet = CharSet.Unicode, ExactSpelling = true,
        CallingConvention = CallingConvention.Winapi)]

        public static extern int apiAppendMenu(IntPtr MenuHandle, int Flags, int NewID, String Item);

        //還可能需要InsertMenu()

        [DllImport("USER32", EntryPoint = "InsertMenuW", SetLastError = true,
       CharSet = CharSet.Unicode, ExactSpelling = true,
       CallingConvention = CallingConvention.Winapi)]
        public static extern int apiInsertMenu(IntPtr hMenu, int Position, int Flags, int NewId, String Item); 


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 GetDC(Int32 hWnd);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hwnd,        // handle to destination window
            int msg,            // message
            IntPtr  wparam,     // first message parameter
            IntPtr lparam       // second message parameter
        );

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(
            string lpClassName,
            string lpWindowName
        );

        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter, 
            string lpszClass,
            string lpszWindow
        ); 

        //使用unmanged的API
        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(
            UInt32 Flag,
            UInt32 X,
            UInt32 Y,
            UInt32 Data,
            IntPtr ExtraInfo
        );

        /// <summary>
        /// 傳送視窗訊息
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int SendMessage(Message m) {
            return SendMessage(m.HWnd, m.Msg, m.LParam, m.WParam);             
        }

        //Win32 API calls necesary to raise an unowned processs main window
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain. 
        /// You would install a hook procedure to monitor the system for certain types of events. These events 
        /// are associated either with a specific thread or with all threads in the same desktop as the calling thread. 
        /// </summary>
        /// <param name="idHook">
        /// [in] Specifies the type of hook procedure to be installed. This parameter can be one of the following values.
        /// </param>
        /// <param name="lpfn">
        /// [in] Pointer to the hook procedure. If the dwThreadId parameter is zero or specifies the identifier of a 
        /// thread created by a different process, the lpfn parameter must point to a hook procedure in a dynamic-link 
        /// library (DLL). Otherwise, lpfn can point to a hook procedure in the code associated with the current process.
        /// </param>
        /// <param name="hMod">
        /// [in] Handle to the DLL containing the hook procedure pointed to by the lpfn parameter. 
        /// The hMod parameter must be set to NULL if the dwThreadId parameter specifies a thread created by 
        /// the current process and if the hook procedure is within the code associated with the current process. 
        /// </param>
        /// <param name="dwThreadId">
        /// [in] Specifies the identifier of the thread with which the hook procedure is to be associated. 
        /// If this parameter is zero, the hook procedure is associated with all existing threads running in the 
        /// same desktop as the calling thread. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
           CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(
            int idHook,
            HookProc lpfn,
            IntPtr hMod,
            int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
        /// </summary>
        /// <param name="idHook">
        /// [in] Handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain. 
        /// A hook procedure can call this function either before or after processing the hook information. 
        /// </summary>
        /// <param name="idHook">Ignored.</param>
        /// <param name="nCode">
        /// [in] Specifies the hook code passed to the current hook procedure. 
        /// The next hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies the wParam value passed to the current hook procedure. 
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
        /// </param>
        /// <param name="lParam">
        /// [in] Specifies the lParam value passed to the current hook procedure. 
        /// The meaning of this parameter depends on the type of hook associated with the current hook chain. 
        /// </param>
        /// <returns>
        /// This value is returned by the next hook procedure in the chain. 
        /// The current hook procedure must also return this value. The meaning of the return value depends on the hook type. 
        /// For more information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/setwindowshookex.asp
        /// </remarks>
        [DllImport("user32.dll", CharSet = CharSet.Auto,
             CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(
            int idHook,
            int nCode,
            int wParam,
            IntPtr lParam);



        /// <summary>
        /// The CallWndProc hook procedure is an application-defined or library-defined callback 
        /// function used with the SetWindowsHookEx function. The HOOKPROC type defines a pointer 
        /// to this callback function. CallWndProc is a placeholder for the application-defined 
        /// or library-defined function name.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/hooks/hookreference/hookfunctions/callwndproc.asp
        /// </remarks>
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// The ToAscii function translates the specified virtual-key code and keyboard 
        /// state to the corresponding character or characters. The function translates the code 
        /// using the input language and physical keyboard layout identified by the keyboard layout handle.
        /// </summary>
        /// <param name="uVirtKey">
        /// [in] Specifies the virtual-key code to be translated. 
        /// </param>
        /// <param name="uScanCode">
        /// [in] Specifies the hardware scan code of the key to be translated. 
        /// The high-order bit of this value is set if the key is up (not pressed). 
        /// </param>
        /// <param name="lpbKeyState">
        /// [in] Pointer to a 256-byte array that contains the current keyboard state. 
        /// Each element (byte) in the array contains the state of one key. 
        /// If the high-order bit of a byte is set, the key is down (pressed). 
        /// The low bit, if set, indicates that the key is toggled on. In this function, 
        /// only the toggle bit of the CAPS LOCK key is relevant. The toggle state 
        /// of the NUM LOCK and SCROLL LOCK keys is ignored.
        /// </param>
        /// <param name="lpwTransKey">
        /// [out] Pointer to the buffer that receives the translated character or characters. 
        /// </param>
        /// <param name="fuState">
        /// [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise. 
        /// </param>
        /// <returns>
        /// If the specified key is a dead key, the return value is negative. Otherwise, it is one of the following values. 
        /// Value Meaning 
        /// 0 The specified virtual key has no translation for the current state of the keyboard. 
        /// 1 One character was copied to the buffer. 
        /// 2 Two characters were copied to the buffer. This usually happens when a dead-key character 
        /// (accent or diacritic) stored in the keyboard layout cannot be composed with the specified 
        /// virtual key to form a single character. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
        /// </remarks>
        [DllImport("user32")]
        public static extern int ToAscii(
            int uVirtKey,
            int uScanCode,
            byte[] lpbKeyState,
            byte[] lpwTransKey,
            int fuState);

        /// <summary>
        /// The GetKeyboardState function copies the status of the 256 virtual keys to the 
        /// specified buffer. 
        /// </summary>
        /// <param name="pbKeyState">
        /// [in] Pointer to a 256-byte array that contains keyboard key states. 
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError. 
        /// </returns>
        /// <remarks>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/userinput/keyboardinput/keyboardinputreference/keyboardinputfunctions/toascii.asp
        /// </remarks>
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int vKey);

        #region Clipbaord
        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        public static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        public static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        public static extern uint EnumClipboardFormats(uint format);

        [DllImport("user32.dll")]
        public static extern int GetClipboardFormatName(uint format, [Out] StringBuilder lpszFormatName, int cchMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetClipboardData(int type, IntPtr h);

        #endregion
    }

    public partial struct User32 {
        public const int WM_USER = 0x0400;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_CLICK = 0x00F5;

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
    }
}
