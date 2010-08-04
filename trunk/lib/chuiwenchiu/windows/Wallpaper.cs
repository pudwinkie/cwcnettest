using System;
using System.Collections.Generic;
using System.Text;

namespace ChuiWenChiu.Win32 {
    public class Wallpaper {
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_SENDCHANGE = 0x2;

        public static void ChangeTo(String filename){
            User32.SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, filename, SPIF_SENDCHANGE);
        }
    }
}
