using System;
using System.Text;
namespace ChuiWenChiu {
    /// <summary>
    /// Summary description for StrHelper.
    /// ㏑罽糶
    /// Str: unicode string
    /// Arr: unicode array
    /// Hex: 秈计沮
    /// Hexbin: 秈计沮ノASCII才ボ ㄒ 才''''1''''hex琌0x31ボhexbin琌 ''''3''''''''1''''
    /// Asc: ASCII
    /// Uni: UNICODE
    /// </summary>
    public sealed class StrHelper {
        #region Hex籔Hexbin锣传
        public static void Hexbin2Hex(byte[] bHexbin, byte[] bHex, int nLen) {
            for (int i = 0; i < nLen / 2; i++) {
                if (bHexbin[2 * i] < 0x41) {
                    bHex[i] = Convert.ToByte(((bHexbin[2 * i] - 0x30) << 4) & 0xf0);
                } else {
                    bHex[i] = Convert.ToByte(((bHexbin[2 * i] - 0x37) << 4) & 0xf0);
                }

                if (bHexbin[2 * i + 1] < 0x41) {
                    bHex[i] |= Convert.ToByte((bHexbin[2 * i + 1] - 0x30) & 0x0f);
                } else {
                    bHex[i] |= Convert.ToByte((bHexbin[2 * i + 1] - 0x37) & 0x0f);
                }
            }
        }
        public static byte[] Hexbin2Hex(byte[] bHexbin, int nLen) {
            if (nLen % 2 != 0)
                return null;
            byte[] bHex = new byte[nLen / 2];
            Hexbin2Hex(bHexbin, bHex, nLen);
            return bHex;
        }
        public static void Hex2Hexbin(byte[] bHex, byte[] bHexbin, int nLen) {
            byte c;
            for (int i = 0; i < nLen; i++) {
                c = Convert.ToByte((bHex[i] >> 4) & 0x0f);
                if (c < 0x0a) {
                    bHexbin[2 * i] = Convert.ToByte(c + 0x30);
                } else {
                    bHexbin[2 * i] = Convert.ToByte(c + 0x37);
                }
                c = Convert.ToByte(bHex[i] & 0x0f);
                if (c < 0x0a) {
                    bHexbin[2 * i + 1] = Convert.ToByte(c + 0x30);
                } else {
                    bHexbin[2 * i + 1] = Convert.ToByte(c + 0x37);
                }
            }
        }
        public static byte[] Hex2Hexbin(byte[] bHex, int nLen) {
            byte[] bHexbin = new byte[nLen * 2];
            Hex2Hexbin(bHex, bHexbin, nLen);
            return bHexbin;
        }
        #endregion

        #region 计舱㎝才﹃ぇ丁锣て
        public static byte[] Str2Arr(String s) {
            return (new UnicodeEncoding()).GetBytes(s);
        }
        public static string Arr2Str(byte[] buffer) {
            return (new UnicodeEncoding()).GetString(buffer, 0, buffer.Length);
        }

        public static byte[] Str2AscArr(String s) {
            return System.Text.UnicodeEncoding.Convert(System.Text.Encoding.Unicode,
             System.Text.Encoding.ASCII,
             Str2Arr(s));
        }

        public static byte[] Str2HexAscArr(String s) {
            byte[] hex = Str2AscArr(s);
            byte[] hexbin = Hex2Hexbin(hex, hex.Length);
            return hexbin;
        }
        public static string AscArr2Str(byte[] b) {
            return System.Text.UnicodeEncoding.Unicode.GetString(
             System.Text.ASCIIEncoding.Convert(System.Text.Encoding.ASCII,
             System.Text.Encoding.Unicode,
             b)
             );
        }

        public static string HexAscArr2Str(byte[] buffer) {
            byte[] b = Hex2Hexbin(buffer, buffer.Length);
            return AscArr2Str(b);
        }
        #endregion
    }
}