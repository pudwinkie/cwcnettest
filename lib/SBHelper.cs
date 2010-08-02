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
            SND_SYNC = 0x0000,//�P�B
            SND_ASYNC = 0x0001,//���B
            SND_NODEFAULT = 0x0002,//��������q�{���R��
            SND_MEMORY = 0x0004,//�n�����Ӧۤ��s
            SND_LOOP = 0x0008, //�`������
            SND_NOSTOP = 0x0010,//������ثe������
            SND_NOWAIT = 0x00002000,//���񾹦��L�ɤ�����
            SND_ALIAS = 0x00010000, //���w���U���O�W��
            SND_ALIAS_ID = 0x00110000, //�O�W��ID
            SND_FILENAME = 0x00020000, //���W
            SND_RESOURCE = 0x00040004 //�귽�W
        }

        #region �ե�DLL���P�_�����O�_���n�d
        [DllImport("winmm.dll", EntryPoint = "waveOutGetNumDevs")]

        //waveOutGetNumdevs()��k
        //��������n�d�ɪ�^1
        //�S���n�d��^0
        public static extern int waveOutGetNumDevs();
        #endregion

        [DllImport("winmm.dll")]
        //SoundSource�n�����
        //�Ѽ�hmod�O���ε{�Ǫ���ҥy�`
        //psFlag����Ҧ�
        public static extern bool PlaySound(string SoundSource, IntPtr hmod,PlaySoundFlags psFlag);
    }
}