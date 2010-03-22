using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace LU.Security
{
    public class Util
    {
        // String To Bytes
        public static byte[] StringToBytes(string s)
        {
            return System.Text.Encoding.ASCII.GetBytes(s);
        }

        // Bytes To String
        public static string BytesToString(byte[] b)
        {
            return System.Text.Encoding.ASCII.GetString(b);
        }

        // Bytes To Hex String
        public static string BytesToHexString(byte[] b)
        {
            if (b == null)
                return null;

            StringBuilder sb = new StringBuilder(b.Length * 2);

            for (int i = 0; i < b.Length; i++)
                sb.AppendFormat("{0:x2}", b[i]);

            return sb.ToString();
        }

        // Hex String To Bytes
        public static byte[] HexStringToBytes(string s)
        {
            if (s == null) return null;

            int nLen = s.Length;
            if ((nLen % 2) != 0) // 如果长度为奇数，则忽略最后一个十六位字符
                nLen--;

            byte[] buffer = new byte[nLen / 2];

            for (int i = 0; i < nLen; i += 2)
            {
                buffer[i / 2] = Convert.ToByte(s.Substring(i, 2), 16);
            }

            return buffer;
        }
    }

 public class Hash
 {
  // MD5(32bit)
  public static string MD5(byte[] b)
  {
   MD5 md5 = new MD5CryptoServiceProvider();
   return Util.BytesToHexString( md5.ComputeHash(b) );
  }
  
  public static string MD5(string szInput)
  {
   return MD5( Util.StringToBytes(szInput) );
  }
  
  // SHA1(160bit)
  public static string SHA1(byte[] b)
  {
   SHA1 sha1 = new SHA1Managed();
   return Util.BytesToHexString( sha1.ComputeHash(b) );
  }
  
  public static string SHA1(string szInput)
  {
   return SHA1( Util.StringToBytes(szInput) );
  }
  
  // SHA256(256bit)
  public static string SHA256(byte[] b)
  {
   SHA256 sha256 = new SHA256Managed();
   return Util.BytesToHexString( sha256.ComputeHash(b) );
  }
  
  public static string SHA256(string szInput)
  {
   return SHA256( Util.StringToBytes(szInput) );
  }
  
  // SHA384(384bit)
  public static string SHA384(byte[] b)
  {
   SHA384 sha384 = new SHA384Managed();
   return Util.BytesToHexString( sha384.ComputeHash(b) );
  }
  
  public static string SHA384(string szInput)
  {
   return SHA384( Util.StringToBytes(szInput) );
  }
  
  // SHA512(512bit)
  public static string SHA512(byte[] b)
  {
   SHA512 sha512 = new SHA512Managed();
   return Util.BytesToHexString( sha512.ComputeHash(b) );
  }
  
  public static string SHA512(string szInput)
  {
   return SHA512( Util.StringToBytes(szInput) );
  }
 }

    public class DES
    {
        private DESCryptoServiceProvider _des;

        public DES()
        {
            _des = new DESCryptoServiceProvider();
        }

        public DES(byte[] key, byte[] iv)
        {
            _des = new DESCryptoServiceProvider();
            _des.Key = key;
            _des.IV = iv;
        }

        public DES(string SerialNumber)
        {
            SHA1 sha1 = new SHA1Managed();
            byte[] buffer = sha1.ComputeHash(Util.StringToBytes(SerialNumber));
            byte[] key = new byte[8];
            byte[] iv = new byte[8];

            Array.Copy(buffer, 0, key, 0, 8);
            Array.Copy(buffer, 8, iv, 0, 8);

            _des = new DESCryptoServiceProvider();
            _des.Key = key;
            _des.IV = iv;
        }

        public byte[] Key
        {
            get { return _des.Key; }
            set { _des.Key = value; }
        }

        public byte[] IV
        {
            get { return _des.IV; }
            set { _des.IV = value; }
        }

        public byte[] Encrypt(byte[] plainText)
        {
            MemoryStream ms = new MemoryStream();

            CryptoStream encStream = new CryptoStream(ms, _des.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(plainText, 0, plainText.Length);
            encStream.Close();

            byte[] buffer = ms.ToArray();
            ms.Close();

            return buffer;
        }

        public string Encrypt(string plainText)
        {
            byte[] cypherText = this.Encrypt(Util.StringToBytes(plainText));
            return Util.BytesToHexString(cypherText);
        }

        public byte[] Decrypt(byte[] cypherText)
        {
            MemoryStream ms = new MemoryStream(cypherText);
            CryptoStream encStream = new CryptoStream(ms, _des.CreateDecryptor(), CryptoStreamMode.Read);

            int b;
            MemoryStream tmpStream = new MemoryStream();
            while ((b = encStream.ReadByte()) != -1)
            {
                tmpStream.WriteByte((byte)b);
            }

            encStream.Close();
            ms.Close();

            byte[] buffer = tmpStream.ToArray();
            tmpStream.Close();

            return buffer;
        }

        public string Decrypt(string cypherText)
        {
            byte[] plainText = this.Decrypt(Util.HexStringToBytes(cypherText));
            return Util.BytesToString(plainText);
        }
    }
}