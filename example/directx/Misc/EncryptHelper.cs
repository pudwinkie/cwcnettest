/**************************************************
 * 版權所有: Mr_Sheng
 * 文 件 名: EncryptHelper.cs
 * 文件描述: 
 * 類型說明: EncryptHelper 加密幫助類
 * 授權聲明:
 *           本程序為自由軟件；
 *           您可依據自由軟件基金會所發表的GPL v3授權條款，對本程序再次發佈和/或修改；
 *           本程序是基於使用目的而加以發佈，然而不負任何擔保責任；
 *           亦無對適售性或特定目的適用性所為的默示性擔保。
 *           詳情請參照GNU通用公共授權 v3（參見license.txt文件）。
 * 版本歷史: 
 *           v2.0.0 Mr_Sheng   2009-09-09 修改
 *           
***************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Sheng.Common
{
    /// <summary>
    /// 加密幫助類
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5DecryptString(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] md5Source = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] md5Out = md5.ComputeHash(md5Source);
            return Convert.ToBase64String(md5Out);
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="sInputString">輸入字符</param>
        /// <param name="sKey">Key</param>
        /// <returns>加密結果</returns>
        public string DESEncryptString(string sInputString, string sKey)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(sInputString);
                byte[] result;
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);                       //密鑰
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);                        //初始化向量
                ICryptoTransform desencrypt = DES.CreateEncryptor();                //加密器對象
                result = desencrypt.TransformFinalBlock(data, 0, data.Length);      //轉換指定字節數組的指定區域
                return BitConverter.ToString(result);
            }
            catch (Exception ex)
            {
                //ex.Message = "DES加密異常";
                throw ex;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="sInputString">輸入字符</param>
        /// <param name="sKey">Key</param>
        /// <returns>解密結果</returns>
        public string DESDecryptString(string sInputString, string sKey)
        {
            try
            {
                //將字符串轉換為字節數組
                string[] sInput = sInputString.Split("-".ToCharArray());
                byte[] data = new byte[sInput.Length];
                byte[] result;
                for (int i = 0; i < sInput.Length; i++)
                {
                    data[i] = byte.Parse(sInput[i], System.Globalization.NumberStyles.HexNumber);
                }

                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                ICryptoTransform desencrypt = DES.CreateDecryptor();
                result = desencrypt.TransformFinalBlock(data, 0, data.Length);
                return Encoding.Default.GetString(result);
            }
            catch (Exception ex)
            {
                //ex.Message = "DES解密異常";
                throw ex;
            }
        }
    }
}