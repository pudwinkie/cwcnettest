using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace ChuiWenChiu.Win32 {
    public struct  GDI32 {


        [DllImport("gdi32")]
        public static extern Int32 CreateDC(String lpDriverName, String lpDeviceName, String lpOutput, String lpInitData);

        [DllImport("gdi32")]
        public static extern Int32 CreateCompatibleDC(Int32 hDC);

        [DllImport("gdi32")]
        public static extern Int32 CreateCompatibleBitmap(Int32 hDC, Int32 nWidth, Int32 nHeight);

        [DllImport("gdi32")]
        public static extern Int32 GetDeviceCaps(Int32 hdc, Int32 nIndex);

        [DllImport("gdi32")]
        public static extern Int32 SelectObject(Int32 hDC, Int32 hObject);

        [DllImport("gdi32")]
        public static extern Int32 BitBlt(Int32 srchDC, Int32 srcX, Int32 srcY, Int32 srcW, Int32 srcH, Int32 desthDC, Int32 destX, Int32 destY, Int32 op);

        [DllImport("gdi32")]
        public static extern Int32 DeleteDC(Int32 hDC);

        [DllImport("gdi32")]
        public static extern Int32 DeleteObject(Int32 hObj);
    }
}
