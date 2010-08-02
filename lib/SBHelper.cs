using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HardWaresOperation
{
    class SBHelper
    {
        public enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,//同步
            SND_ASYNC = 0x0001,//異步
            SND_NODEFAULT = 0x0002,//未找到文件默認為靜音
            SND_MEMORY = 0x0004,//聲音文件來自內存
            SND_LOOP = 0x0008, //循環播放
            SND_NOSTOP = 0x0010,//不停止目前的播放
            SND_NOWAIT = 0x00002000,//當播放器忙碌時不等待
            SND_ALIAS = 0x00010000, //為已註冊的別名時
            SND_ALIAS_ID = 0x00110000, //別名為ID
            SND_FILENAME = 0x00020000, //文件名
            SND_RESOURCE = 0x00040004 //資源名
        }

        #region 調用DLL文件判斷機器是否有聲卡
        [DllImport("winmm.dll", EntryPoint = "waveOutGetNumDevs")]

        //waveOutGetNumdevs()方法
        //當機器有聲卡時返回1
        //沒有聲卡返回0
        public static extern int waveOutGetNumDevs();
        #endregion

        [DllImport("winmm.dll")]
        //SoundSource聲音文件
        //參數hmod是應用程序的實例句柄
        //psFlag播放模式
        public static extern bool PlaySound(string SoundSource, IntPtr hmod,PlaySoundFlags psFlag);
    }
}