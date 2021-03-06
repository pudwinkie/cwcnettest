//
// bittorrent File Creator
// Author: http://www.cnblogs.com/thomasnet
// Email: cnblogreply@gmail.com
// License: GPL v3, http://www.gnu.org/copyleft/gpl.html
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BenCode
{
    class Utility
    {
        static public byte[] GetByteFromInt64(UInt64 value)
        {
            byte[] int64Byte = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                int64Byte[i] = (byte)(value % 0x100);
                value /= 0x100;
            }
            return int64Byte;
        }

        static public byte[] GetByteFromInt16(UInt16 value)
        {
            byte[] int16Byte = new byte[2];
            for (int i = 0; i < 2; i++)
            {
                int16Byte[i] = (byte)(value % 0x100);
                value /= 0x100;
            }
            return int16Byte;
        }

        static public string[] OrdinalSortStringArray(string[] fileName)
        {
            List<string> fileNameList = new List<string>(fileName);
            fileNameList.Sort(UTF8StringComparison);
            return fileNameList.ToArray();
        }

        static private int UTF8StringComparison(string x, string y)
        {
            return string.CompareOrdinal(x, y);
        }

        static public long GetFileLength(string fileName)
        {
            using (FileStream fileStream =
                new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                return fileStream.Length;
            }
        }

        static public string GetClosestDirectoryName(string PathName)
        {
            string[] pathSegment = PathName.Split('\\');
            for (int i = pathSegment.Length - 1; i >= 0; --i)
            {
                string directoryName = pathSegment[i];
                if (!string.IsNullOrEmpty(directoryName) ||
                    directoryName != "\\")
                {
                    return directoryName;
                }
            }
            throw new ApplicationException(
                string.Format("Invalid directory name: {0}", PathName));
        }
    }
}

