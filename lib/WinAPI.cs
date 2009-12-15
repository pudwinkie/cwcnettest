namespace cwc
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public static class WinAPI
    {
        private const int SRCCOPY = 0xcc0020;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_MOUSEMOVE = 0x200;

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        public static Bitmap CreateScreenshot()
        {
            IntPtr desktopWindow = GetDesktopWindow();
            IntPtr windowDC = GetWindowDC(desktopWindow);
            RECT rect = new RECT();
            GetWindowRect(desktopWindow, ref rect);
            int nWidth = rect.right - rect.left;
            int nHeight = rect.bottom - rect.top;
            IntPtr hDC = CreateCompatibleDC(windowDC);
            IntPtr hObject = CreateCompatibleBitmap(windowDC, nWidth, nHeight);
            IntPtr ptr5 = SelectObject(hDC, hObject);
            BitBlt(hDC, 0, 0, nWidth, nHeight, windowDC, 0, 0, 0xcc0020);
            SelectObject(hDC, ptr5);
            DeleteDC(hDC);
            ReleaseDC(desktopWindow, windowDC);
            Bitmap bitmap = Image.FromHbitmap(hObject);
            DeleteObject(hObject);
            return bitmap;
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.Dll")]
        public static extern bool EnumChildWindows(IntPtr parentHandle, EnumWindowProc callback, IntPtr lParam);
        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            List<IntPtr> list = GCHandle.FromIntPtr(pointer).get_Target() as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            list.Add(handle);
            return true;
        }

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        public static int fMakeLong(Point p)
        {
            return ((p.get_Y() << 0x10) | (p.get_X() & 0xffff));
        }

        public static List<IntPtr> GetChildWindows(IntPtr parent)
        {
            List<IntPtr> list = new List<IntPtr>();
            GCHandle handle = GCHandle.Alloc(list);
            try
            {
                EnumWindowProc callback = new EnumWindowProc(WinAPI.EnumWindow);
                EnumChildWindows(parent, callback, GCHandle.ToIntPtr(handle));
            }
            finally
            {
                if (handle.get_IsAllocated())
                {
                    handle.Free();
                }
            }
            return list;
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        public static POINT GetClientPoint(IntPtr windowHandler, Point point)
        {
            POINT lpPoint = new POINT(point.get_X(), point.get_Y());
            ScreenToClient(windowHandler, ref lpPoint);
            return lpPoint;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        public static List<IntPtr> GetFlashObj(IntPtr parent)
        {
            List<IntPtr> list = new List<IntPtr>();
            GCHandle handle = GCHandle.Alloc(list);
            try
            {
                EnumWindowProc callback = new EnumWindowProc(WinAPI.IsMacromediaFlashPlayerActiveXWindow);
                EnumChildWindows(parent, callback, GCHandle.ToIntPtr(handle));
            }
            finally
            {
                if (handle.get_IsAllocated())
                {
                    handle.Free();
                }
            }
            return list;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        public static List<IntPtr> GetInternetExplorer(IntPtr parent)
        {
            List<IntPtr> list = new List<IntPtr>();
            GCHandle handle = GCHandle.Alloc(list);
            try
            {
                EnumWindowProc callback = new EnumWindowProc(WinAPI.IsIEServerWindow);
                EnumChildWindows(parent, callback, GCHandle.ToIntPtr(handle));
            }
            finally
            {
                if (handle.get_IsAllocated())
                {
                    handle.Free();
                }
            }
            return list;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        private static bool IsIEServerWindow(IntPtr handle, IntPtr pointer)
        {
            List<IntPtr> list = GCHandle.FromIntPtr(pointer).get_Target() as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            StringBuilder lpClassName = new StringBuilder(100);
            if ((GetClassName(handle, lpClassName, lpClassName.get_Capacity()) != 0) && (string.Compare(lpClassName.ToString(), "Internet Explorer_Server", true, CultureInfo.get_InvariantCulture()) == 0))
            {
                list.Add(handle);
            }
            return true;
        }

        private static bool IsMacromediaFlashPlayerActiveXWindow(IntPtr handle, IntPtr pointer)
        {
            List<IntPtr> list = GCHandle.FromIntPtr(pointer).get_Target() as List<IntPtr>;
            if (list == null)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }
            StringBuilder lpClassName = new StringBuilder(100);
            if ((GetClassName(handle, lpClassName, lpClassName.get_Capacity()) != 0) && (string.Compare(lpClassName.ToString(), "MacromediaFlashPlayerActiveX", true, CultureInfo.get_InvariantCulture()) == 0))
            {
                list.Add(handle);
            }
            return true;
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);
        public static void KeyboardEvent(Keys key, IntPtr windowHandler)
        {
            keybd_event((byte) key, 0x45, 1, (UIntPtr) 0);
            keybd_event((byte) key, 0x45, 3, (UIntPtr) 0);
        }

        public static void KeyboardEvent(Keys key, IntPtr windowHandler, int delay)
        {
            keybd_event((byte) key, 0x45, 1, (UIntPtr) 0);
            Thread.Sleep(delay);
            keybd_event((byte) key, 0x45, 3, (UIntPtr) 0);
        }

        public static void KeyboardEvent(Keys key, IntPtr windowHandler, string state)
        {
            byte dwFlags = 0x1c;
            if (state == "up")
            {
                dwFlags = 3;
            }
            else
            {
                dwFlags = 1;
            }
            keybd_event((byte) key, 0x45, dwFlags, (UIntPtr) 0);
        }

        //public static uint MakeDWORD(ushort hiWord, ushort loWord)
        //{
        //    return ((hiWord << 0x10) | loWord);
        //}

        public static void ManagedMouseMove(int x, int y)
        {
            Cursor.set_Position(new Point(x, y));
        }

        public static void ManagedSendKeys(string keys)
        {
            SendKeys.SendWait(keys);
        }

        public static void ManagedSendKeys(string keys, IntPtr handle)
        {
            if (WindowActive(handle))
            {
                ManagedSendKeys(keys);
            }
        }

        public static void ManagedSendKeys(string keys, string windowName)
        {
            if (WindowActive(windowName))
            {
                ManagedSendKeys(keys);
            }
        }

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);
        public static void MouseClick()
        {
            MouseClick("left");
        }

        public static void MouseClick(string button)
        {
            string str = button;
            if (str != null)
            {
                if (str != "left")
                {
                    if (str != "right")
                    {
                        if (str == "middle")
                        {
                            mouse_event(0x20, 0, 0, 0, 0);
                            mouse_event(0x40, 0, 0, 0, 0);
                        }
                        return;
                    }
                }
                else
                {
                    mouse_event(2, 0, 0, 0, 0);
                    mouse_event(4, 0, 0, 0, 0);
                    return;
                }
                mouse_event(8, 0, 0, 0, 0);
                mouse_event(0x10, 0, 0, 0, 0);
            }
        }

        public static void MouseClick(string button, int state)
        {
            string str = button.ToLower();
            if (str != null)
            {
                if (str != "left")
                {
                    if (str != "right")
                    {
                        if (str == "middle")
                        {
                            switch (state)
                            {
                                case 0:
                                    mouse_event(0x20, 0, 0, 0, 0);
                                    return;

                                case 1:
                                    mouse_event(0x40, 0, 0, 0, 0);
                                    return;
                            }
                        }
                        return;
                    }
                }
                else
                {
                    switch (state)
                    {
                        case 0:
                            mouse_event(2, 0, 0, 0, 0);
                            return;

                        case 1:
                            mouse_event(4, 0, 0, 0, 0);
                            return;
                    }
                    return;
                }
                switch (state)
                {
                    case 0:
                        mouse_event(8, 0, 0, 0, 0);
                        return;

                    case 1:
                        mouse_event(0x10, 0, 0, 0, 0);
                        return;

                    default:
                        return;
                }
            }
        }

        public static void MouseClick(string button, string windowName)
        {
            if (WindowActive(windowName))
            {
                MouseClick(button);
            }
        }


        public static void MouseClickMessage(IntPtr windowHandler)
        {
            SendMessage(windowHandler, 0x201, IntPtr.Zero, IntPtr.Zero);
            SendMessage(windowHandler, 0x202, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowHandler"></param>
        /// <param name="point"></param>
        /// <example>
        /// WinAPI.MouseClickMessage(this.flashs.get_Item(0), new Point(30, 300));
        /// </example>
        public static void MouseClickMessage(IntPtr windowHandler, Point point)
        {
            SendMessage(windowHandler, 0x201, IntPtr.Zero, (IntPtr) fMakeLong(point));
            SendMessage(windowHandler, 0x202, IntPtr.Zero, (IntPtr) fMakeLong(point));
        }

        public static void MouseClickMessage(IntPtr windowHandler, Point point, bool move)
        {
            if (move)
            {
                MouseMoveMessage(windowHandler, point);
            }
            SendMessage(windowHandler, 0x201, IntPtr.Zero, (IntPtr) fMakeLong(point));
            SendMessage(windowHandler, 0x202, IntPtr.Zero, (IntPtr) fMakeLong(point));
        }

        public static void MouseClickMessage(IntPtr windowHandler, Point point, int state)
        {
            switch (state)
            {
                case 0:
                    SendMessage(windowHandler, 2, IntPtr.Zero, (IntPtr) fMakeLong(point));
                    return;

                case 1:
                    SendMessage(windowHandler, 4, IntPtr.Zero, (IntPtr) fMakeLong(point));
                    return;
            }
        }

        public static void MouseClickMessage(IntPtr windowHandler, string button, int state)
        {
            string str = button.ToLower();
            if (str != null)
            {
                if (str != "left")
                {
                    if (str != "right")
                    {
                        if (str == "middle")
                        {
                            switch (state)
                            {
                                case 0:
                                    SendMessage(windowHandler, 0x20, IntPtr.Zero, IntPtr.Zero);
                                    return;

                                case 1:
                                    SendMessage(windowHandler, 0x40, IntPtr.Zero, IntPtr.Zero);
                                    return;
                            }
                        }
                        return;
                    }
                }
                else
                {
                    switch (state)
                    {
                        case 0:
                            SendMessage(windowHandler, 2, IntPtr.Zero, IntPtr.Zero);
                            return;

                        case 1:
                            SendMessage(windowHandler, 4, IntPtr.Zero, IntPtr.Zero);
                            return;
                    }
                    return;
                }
                switch (state)
                {
                    case 0:
                        SendMessage(windowHandler, 8, IntPtr.Zero, IntPtr.Zero);
                        return;

                    case 1:
                        SendMessage(windowHandler, 0x10, IntPtr.Zero, IntPtr.Zero);
                        return;

                    default:
                        return;
                }
            }
        }

        public static void MouseMove(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void MouseMoveMessage(IntPtr windowHandler, Point point)
        {
            SendMessage(windowHandler, 0x200, IntPtr.Zero, (IntPtr) fMakeLong(point));
        }

        public static void MouseMoveMessage(IntPtr windowHandler, int x, int y)
        {
            MouseMoveMessage(windowHandler, new Point(x, y));
        }

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        public static byte[] ReadMemory(IntPtr pID, IntPtr mAddress, byte Size)
        {
            byte[] lpBuffer = new byte[Size];
            IntPtr zero = IntPtr.Zero;
            ReadProcessMemory(pID, mAddress, lpBuffer, (UIntPtr) Size, out zero);
            return lpBuffer;
        }

        public static void ReadMemory(IntPtr pID, IntPtr mAddress, ref byte[] buffer, byte size)
        {
            buffer = new byte[size];
            IntPtr zero = IntPtr.Zero;
            ReadProcessMemory(pID, mAddress, buffer, (UIntPtr) size, out zero);
        }

        public static byte[] ReadPointer(IntPtr pID, IntPtr baseAddress, byte[] offsets)
        {
            byte[] buffer = new byte[4];
            ReadMemory(pID, baseAddress, ref buffer, 4);
            for (byte i = 0; i < offsets.Length; i++)
            {
                int num2 = BitConverter.ToInt32(buffer, 0) + offsets[i];
                ReadMemory(pID, new IntPtr(num2), ref buffer, 4);
            }
            return buffer;
        }

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);
        [DllImport("user32.dll")]
        private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        private static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowHandler"></param>
        /// <param name="key"></param>
        /// <example>
        /// WinAPI.SendKeyMessage(this.flashs.get_Item(0), 40);
        /// </example>
        public static void SendKeyMessage(IntPtr windowHandler, Keys key)
        {
            SendMessage(windowHandler, 0x100, (IntPtr) key, IntPtr.Zero);
            SendMessage(windowHandler, 0x101, (IntPtr) key, IntPtr.Zero);
        }

        public static void SendKeyMessage2(IntPtr windowHandler, Keys key)
        {
            PostMessage(windowHandler, 0x100, (IntPtr) key, IntPtr.Zero);
            PostMessage(windowHandler, 0x101, (IntPtr) key, IntPtr.Zero);
        }

        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32")]
        public static extern int SetCursorPos(int x, int y);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public static void WindowActivate(IntPtr handle)
        {
            SetForegroundWindow(handle);
        }

        public static void WindowActivate(string windowName)
        {
            SetForegroundWindow(FindWindow(null, windowName));
        }

        public static bool WindowActive(IntPtr myHandle)
        {
            IntPtr foregroundWindow = GetForegroundWindow();
            if (myHandle != foregroundWindow)
            {
                return false;
            }
            return true;
        }

        public static bool WindowActive(string windowName)
        {
            IntPtr ptr = FindWindow(null, windowName);
            IntPtr foregroundWindow = GetForegroundWindow();
            if (ptr != foregroundWindow)
            {
                return false;
            }
            return true;
        }

        public static void WindowMove(int x, int y, string windowName)
        {
            WindowMove(x, y, windowName, 800, 600);
        }

        public static void WindowMove(int x, int y, string windowName, int width, int height)
        {
            IntPtr hWnd = FindWindow(null, windowName);
            if (hWnd != IntPtr.Zero)
            {
                MoveWindow(hWnd, x, y, width, height, true);
            }
        }

        public static void WriteMemory(IntPtr pID, IntPtr mAddress, byte[] buffer)
        {
            IntPtr zero = IntPtr.Zero;
            WriteProcessMemory(pID, mAddress, buffer, (UIntPtr) buffer.Length, out zero);
        }

        public static void WriteMemory(string caption, IntPtr mAddress, byte[] buffer)
        {
            IntPtr zero = IntPtr.Zero;
            IntPtr hProcess = FindWindow(null, caption);
            if (hProcess != IntPtr.Zero)
            {
                WriteProcessMemory(hProcess, mAddress, buffer, (UIntPtr) buffer.Length, out zero);
            }
        }

        public static void WriteNOPs(IntPtr pID, IntPtr mAddress, byte NOPCount)
        {
            byte[] buffer = new byte[NOPCount];
            for (byte i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0x90;
            }
            WriteMemory(pID, mAddress, buffer);
        }

        public static void WriteNOPs(string caption, IntPtr mAddress, byte NOPCount)
        {
            byte[] buffer = new byte[NOPCount];
            for (byte i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0x90;
            }
            WriteMemory(caption, mAddress, buffer);
        }

        [DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, out IntPtr lpNumberOfBytesWritten);

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        [Flags]
        public enum MouseEventFlags
        {
            ABSOLUTE = 0x8000,
            LEFTDOWN = 2,
            LEFTUP = 4,
            MIDDLEDOWN = 0x20,
            MIDDLEUP = 0x40,
            MOVE = 1,
            RIGHTDOWN = 8,
            RIGHTUP = 0x10
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator Point(WinAPI.POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator WinAPI.POINT(Point p)
            {
                return new WinAPI.POINT(p.get_X(), p.get_Y());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
    }
}

