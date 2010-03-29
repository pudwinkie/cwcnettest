/**************************************************
 * ���v�Ҧ�: Mr_Sheng
 * �� �� �W: EncryptHelper.cs
 * ���y�z: 
 * ��������: EncryptHelper �[�K���U��
 * ���v�n��:
 *           ���{�Ǭ��ۥѳn��F
 *           �z�i�̾ڦۥѳn�����|�ҵo��GPL v3���v���ڡA�糧�{�ǦA���o�G�M/�έק�F
 *           ���{�ǬO���ϥΥت��ӥ[�H�o�G�A�M�Ӥ��t�����O�d���F
 *           ��L��A��ʩίS�w�ت��A�ΩʩҬ����q�ܩʾ�O�C
 *           �Ա��аѷ�GNU�q�Τ��@���v v3�]�Ѩ�license.txt���^�C
 * �������v: 
 *           v2.0.0 Mr_Sheng   2009-09-09 �ק�
 *           
***************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Sheng.Common
{
    /// <summary>
    /// �[�K���U��
    /// </summary>
    public class EncryptHelper
    {
        /// <summary>
        /// MD5�[�K
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
        /// DES�[�K�r�Ŧ�
        /// </summary>
        /// <param name="sInputString">��J�r��</param>
        /// <param name="sKey">Key</param>
        /// <returns>�[�K���G</returns>
        public string DESEncryptString(string sInputString, string sKey)
        {
            try
            {
                byte[] data = Encoding.Default.GetBytes(sInputString);
                byte[] result;
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);                       //�K�_
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);                        //��l�ƦV�q
                ICryptoTransform desencrypt = DES.CreateEncryptor();                //�[�K����H
                result = desencrypt.TransformFinalBlock(data, 0, data.Length);      //�ഫ���w�r�`�Ʋժ����w�ϰ�
                return BitConverter.ToString(result);
            }
            catch (Exception ex)
            {
                //ex.Message = "DES�[�K���`";
                throw ex;
            }
        }

        /// <summary>
        /// DES�ѱK�r�Ŧ�
        /// </summary>
        /// <param name="sInputString">��J�r��</param>
        /// <param name="sKey">Key</param>
        /// <returns>�ѱK���G</returns>
        public string DESDecryptString(string sInputString, string sKey)
        {
            try
            {
                //�N�r�Ŧ��ഫ���r�`�Ʋ�
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
                //ex.Message = "DES�ѱK���`";
                throw ex;
            }
        }
    }
}