using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace HardWaresOperation
{
    public class SpeakerBeeper
    {
        //#region �ե�DLL���P�_�����O�_���n�d
        //[DllImport("winmm.dll",EntryPoint = "waveOutGetNumdevs")]

        ////waveOutGetNumdevs()��k
        ////��������n�d�ɪ�^1
        ////�S���n�d��^0
        //public static extern int waveOutGetNumdevs();
        //#endregion

        //���귽
        private string SoundSource = @"C:\Documents and Settings\Administrator\�ୱ\gc22002a.wav";

        public SpeakerBeeper(string _SoundSource)
        {
            SoundSource = _SoundSource;
        }

        /// <summary>
        /// �ˬd�n�d�A�����n��
        /// </summary>
        /// <param name="_SoundSource">�n�����</param>
        /// <returns>���񦨥\�A��^true</returns>
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