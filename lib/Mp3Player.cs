using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

/************************************************************************************************************************************
 * * 文件名   : 
 * * 聲明     : 
 * * 創建者   : 黃聰
 * * 創建日期 : 2009.10.8
 * * 修改者   : 黃聰
 * * 最新修改日期 : 2009.10.8
 ************************************************************************************************************************************/
namespace Tool
{
    /********************************************************************************************************************************
     * * 類名     : 
     * * 聲明     : 
     * * 創建者   : 黃聰
     * * 創建日期 : 2009.7.15
     * * 修改者   : 黃聰
     * * 最新修改日期 : 2009.7.15
     ********************************************************************************************************************************/
    public class Mp3Player
    {
        #region - 屬性 -
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String command,
          StringBuilder buffer, Int32 bufferSize, IntPtr hwndCallback);

        /// <summary>
        /// 臨時音樂文件存放處
        /// </summary>
        private string m_musicPath = "";

        /// <summary>
        /// 父窗體句柄
        /// </summary>
        private IntPtr m_Handle;
        #endregion

        #region - 構造函數 -
        /// <summary>
        /// 創建Mp3播放類
        /// </summary>
        /// <param name="music">嵌入的音樂文件</param>
        /// <param name="path">臨時音樂文件保存路徑</param>
        /// <param name="Handle">父窗體句柄</param>
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
        /// 創建Mp3播放類
        /// </summary>
        /// <param name="musicPath">要播放的mp3文件路徑</param>
        /// <param name="Handle">父窗體句柄</param>
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

        #region - 播放音樂 -
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

        #region - 停止音樂播放 -
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