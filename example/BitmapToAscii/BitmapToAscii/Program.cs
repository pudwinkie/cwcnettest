using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
 
namespace BitmapToAscii {
    class Program {
        static void Main(string[] args) {
            BitmapToAscii bta = new BitmapToAscii(); 
            using (StreamWriter sw = new StreamWriter(@"c:\dd.htm")) {
                sw.Write(bta.ConvertToAscii(new Bitmap(@"c:\ASCII-art.JPG")));
            }
        }
    }

    public class BitmapToAscii {
        public string ConvertToAscii(Bitmap image) {
            Boolean toggle = false;
            StringBuilder sb = new StringBuilder();

            for (int h = 0; h < image.Height; h++) {

                for (int w = 0; w < image.Width; w++) {

                    Color pixelColor = image.GetPixel(w, h);

                    //Average out the RGB components to find the Gray Color

                    int red = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    int green = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    int blue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    Color grayColor = Color.FromArgb(red, green, blue);



                    //Use the toggle flag to minimize height-wise stretch

                    if (!toggle) {

                        int index = (grayColor.R * 10) / 255;

                        sb.Append(_AsciiChars[index]);

                    }

                }

                if (!toggle) {

                    sb.Append("<BR>");

                    toggle = true;

                } else {

                    toggle = false;

                }

            }

            return sb.ToString();

        }

        private string[] _AsciiChars = { "#", "#", "@", "%", "=", "+", "*", ":", "-", ".", "&nbsp;" };
    }
}
