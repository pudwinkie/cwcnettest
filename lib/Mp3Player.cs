using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

/************************************************************************************************************************************
 * * ���W   : 
 * * �n��     : 
 * * �Ыت�   : ���o
 * * �Ыؤ�� : 2009.10.8
 * * �ק��   : ���o
 * * �̷s�ק��� : 2009.10.8
 ************************************************************************************************************************************/
namespace Tool
{
    /********************************************************************************************************************************
     * * ���W     : 
     * * �n��     : 
     * * �Ыت�   : ���o
     * * �Ыؤ�� : 2009.7.15
     * * �ק��   : ���o
     * * �̷s�ק��� : 2009.7.15
     ********************************************************************************************************************************/
    public class Mp3Player
    {
        #region - �ݩ� -
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String command,
          StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

        /// <summary>
        /// �{�ɭ��֤��s��B
        /// </summary>
        private string m_musicPath = "";

        /// <summary>
        /// ������y�`
        /// </summary>
        private IntPtr m_Handle;
        #endregion

        #region - �c�y��� -
        /// <summary>
        /// �Ы�Mp3������
        /// </summary>
        /// <param name="music">�O�J�����֤��</param>
        /// <param name="path">�{�ɭ��֤��O�s���|</param>
        /// <param name="Handle">������y�`</param>
        public Mp3Player(Byte[] music, string path, IntPtr Handle)
        {
            try
            {
                m_Handle = Handle;
                m_musicPath = Path.Combine(path, "temp.mp3");
                FileStream fs = new FileStream(m_musicPath, FileMode.Create);
                fs.Write(music, 0, music.Length);
                fs.Close();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// �Ы�Mp3������
        /// </summary>
        /// <param name="musicPath">�n����mp3�����|</param>
        /// <param name="Handle">������y�`</param>
        public Mp3Player(string musicPath, IntPtr Handle)
        {
            m_musicPath = musicPath;
            m_Handle = Handle;
        }

        public Mp3Player(Byte[] music, IntPtr Handle):
            this(music, @"C:\Windows\", Handle)
        {

        }

        #endregion

        #region - ���񭵼� -
        public void Open(string path)
        {
            if (path != "")
            {
                try
                {
                    mciSendString("open " + path + " alias media", null, 0, m_Handle);
                    mciSendString("play media", null, 0, m_Handle);
                }
                catch (Exception)
                {

                }
            }
        }

        public void Open()
        {
            Open(m_musicPath);
        }
        #endregion

        #region - ����ּ��� -
        void CloseMedia()
        {
            try
            {
                mciSendString("close all", null, 0, m_Handle);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}