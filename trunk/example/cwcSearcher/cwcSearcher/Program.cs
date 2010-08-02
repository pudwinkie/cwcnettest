using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace cwcSearcher {
    public class FolderFilter {
        private DirectoryInfo di;

        public FolderFilter(string path) {
            di = new DirectoryInfo(path);            
        }

        private bool bRunning;
        public void Run() {
            bRunning = true;
            if (!Filter(di.GetDirectories())) {
                Console.WriteLine("User STOP success!!"); 
            }
            bRunning = false;
        }

        public void Stop() {
            bRunning = false;
        }

        public bool Filter(DirectoryInfo[] ds) {
            foreach (DirectoryInfo di in ds) {
                if (!bRunning) {
                    return false;
                }

                if (di.Name == Pattern) {
                    di.Delete(true);  
                    //Console.WriteLine(di.FullName);
                } else {
                    Filter(di.GetDirectories()); 
                }
            }

            return true;
        }

        private String pattern;
        public String Pattern {
            get {
                return pattern;
            }

            set {
                pattern = value;
            }
        }
    }

    class Program {
        static void Main(string[] args) {
            if (args.Length <=0 ){
                Console.WriteLine("cwcSearcher.exe path");
                return;
            }

            if (Directory.Exists(args[0])) {
                FolderFilter s = new FolderFilter(args[0]);
                s.Pattern = "CVS";
                s.Run(); 
            } else {
                Console.WriteLine("Folder Not Found"); 
            }
        }       

    }
}
