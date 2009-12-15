using System;
using System.IO;
using System.IO.Compression;

namespace gzipTest {
    class Program {
        static void Main(string[] args) {
            GZipTest.Compress("c:\\vcredist_x86.exe", "c:\\vcredist_x86.exe.zip");
            GZipTest.Decompress("c:\\vcredist_x86.exe.zip", "c:\\vcredist_x86.exe.zip.exe");
        }
    }

    public class GZipTest{
        const int SIZE = 4096;
        public static void Compress(String from, String to){
            
            using (FileStream fs = new FileStream(from, FileMode.Open)) {
                using (FileStream fsOutput = new FileStream(to, FileMode.Create, FileAccess.Write)) {                                       
                    using (GZipStream zip = new GZipStream(fsOutput, CompressionMode.Compress)) {
                        byte[] input = new byte[SIZE];
                        int bytesRead;
                        while ((bytesRead = fs.Read(input, 0, SIZE)) > 0) {                            
                            zip.Write(input, 0, bytesRead);
                                                    
                        }
                    }
                }
            }
        }

        public static void Decompress(String from, String to) {
            using (FileStream fs = new FileStream(from, FileMode.Open)) {
                using (FileStream fsOutput = new FileStream(to, FileMode.Create, FileAccess.Write)) {
                    using (GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, true)) {
                        byte[] buffer = new byte[SIZE];
                        int bytesRead;
                        while ((bytesRead = zip.Read(buffer, 0, buffer.Length)) > 0) {
                            fsOutput.Write(buffer, 0, bytesRead);
                        }
                    }
                }  
            }
        }
    }
}
