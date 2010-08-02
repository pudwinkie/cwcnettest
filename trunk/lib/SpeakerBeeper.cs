using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HardWaresOperation
{
    public class SpeakerBeeper
    {
        //#region 調用DLL文件判斷機器是否有聲卡
        //[DllImport("winmm.dll",EntryPoint = "waveOutGetNumdevs")]

        ////waveOutGetNumdevs()方法
        ////當機器有聲卡時返回1
        ////沒有聲卡返回0
        //public static extern int waveOutGetNumdevs();
        //#endregion

        //文件資源
        private string SoundSource = @"C:\Documents and Settings\Administrator\桌面\gc22002a.wav";

        public SpeakerBeeper(string _SoundSource)
        {
            SoundSource = _SoundSource;
        }

        /// <summary>
        /// 檢查聲卡，播放聲音
        /// </summary>
        /// <param name="_SoundSource">聲音文件</param>
        /// <returns>播放成功，返回true</returns>
        public bool SpeakerBeep()
        {
            if (SBHelper.waveOutGetNumDevs()!= 0)
            {
                SBHelper.PlaySound(SoundSource, IntPtr.Zero, SBHelper.PlaySoundFlags.SND_FILENAME | SBHelper.PlaySoundFlags.SND_ASYNC);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}