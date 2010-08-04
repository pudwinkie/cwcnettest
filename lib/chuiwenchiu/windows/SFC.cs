using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ChuiWenChiu.Win32 {

    public static class SFC {
        // 檢查檔案是不是受到Windows系統所保護
        [DllImport("sfc.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool SfcIsFileProtected(IntPtr handle /* must be NULL */, IntPtr path /* 檔案名稱 */);

        // 取得受保護的檔案清單
        [DllImport("sfc.dll", ExactSpelling = true, SetLastError = true)]
        static extern bool SfcGetNextProtectedFile(IntPtr RpcHandle /* must be NULL */, PPROTECTED_FILE_DATA ProtFileData);

        //[DllImport("Sfcfiles.dll", ExactSpelling = true, SetLastError = true)]
        //static extern bool SfcGetFiles(PPROTECT_FILE_ENTRY ProtFileData, out long FileCount);        

        public class ErrorCode : Exception {
            public ErrorCode(int code) {
                m_code = code;
            }

            private int m_code;
            private int Code {
                get {
                    return m_code;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        class PPROTECTED_FILE_DATA {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 18)]
            public String FileName = null;
            public int FileNumber = 0;
        }

        public static int GetProtectedFileNumber() {
            PPROTECTED_FILE_DATA pfd = new PPROTECTED_FILE_DATA();

            //IntPtr nativeBuffer = IntPtr.Zero;
            //Int32 elementSize = Marshal.SizeOf(typeof(PPROTECTED_FILE_DATA));
            //nativeBuffer = Marshal.AllocCoTaskMem(elementSize);
            //Byte* bytePtr = (Byte*)nativeBuffer.ToPointer();
            //IntPtr ptr = new IntPtr(bytePtr);
            //Marshal.StructureToPtr(pfd, ptr, false);            

            if (SfcGetNextProtectedFile(IntPtr.Zero, pfd)) {
                return 1;
            } else {
                int code = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                throw new Exception();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsFileProtected(String filename) {
            if (!System.IO.File.Exists(filename)) {
                return false;
            }

            IntPtr pathptr = Marshal.StringToHGlobalUni(filename);
            if (!SfcIsFileProtected(IntPtr.Zero, pathptr)) {
                int code = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                if (code == 2) {
                    return false;
                } else {
                    throw new ErrorCode(code);
                }
            }

            return true;
        }
    }
}
