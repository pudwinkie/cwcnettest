using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using System.IO; 
namespace SharpZibTest {
    [TestFixture]
    public class FastZipTest {
        private FastZip fz;
        private FastZipEvents fze;
        private const string output = "c:/test/outputAll";     

        [SetUp]
        public void BeforeProcess() {
            fze = new FastZipEvents();
            fze.ProcessFile = new ICSharpCode.SharpZipLib.Core.ProcessFileDelegate(ProcessFile);
            fze.ProcessDirectory = new ICSharpCode.SharpZipLib.Core.ProcessDirectoryDelegate(ProcessDirectory);
            fze.FileFailure = new ICSharpCode.SharpZipLib.Core.FileFailureDelegate(FailFailure);
            fze.DirectoryFailure = new ICSharpCode.SharpZipLib.Core.DirectoryFailureDelegate(DirectoryFailure);

            fz = new FastZip(fze);
            if (Directory.Exists(output)) {
                Directory.Delete(output, true);
            }
        }

        [TearDown]
        public void AfterProcess() {
            fz = null;
            fze = null;
            if (Directory.Exists(output)) {
                Directory.Delete(output, true);
            }
        }

        /// <summary>
        /// 直接將 zip 壓縮檔的資料全部解到指定路徑
        /// </summary>
        [Test]
        public void ExtractAll() {
             
            fz.ExtractZip("c:/test/test.zip", output, "");
            DirectoryInfo di = new DirectoryInfo(output);
            
            Assert.AreEqual(di.GetFiles().Length, 3);
            Assert.IsTrue(File.Exists(output + "/a.txt"));
            Assert.IsTrue(File.Exists(output + "/b.txt"));
            Assert.IsTrue(File.Exists(output + "/c.gif"));
        }

        /// <summary>
        /// 直接將 zip 壓縮檔的資料全部解到指定路徑
        /// </summary>
        [Test]
        public void ExtractTxt() {            
            fz.ExtractZip("c:/test/test.zip", output, ".txt");
            DirectoryInfo di = new DirectoryInfo(output);
            Assert.AreEqual(di.GetFiles().Length, 2);
            Assert.IsTrue(File.Exists(output + "/a.txt"));
            Assert.IsTrue(File.Exists(output + "/b.txt"));            
        }

        /// <summary>
        /// 取出所有的目錄和檔案
        /// </summary>
        [Test]
        public void ExtractFolderAndFile() {          
            
            fz.CreateEmptyDirectories = true;
            fz.ExtractZip("c:/test/test2.zip", output, "");
            DirectoryInfo di = new DirectoryInfo(output);

            Assert.AreEqual(di.GetFiles().Length, 3);
            Assert.AreEqual(di.GetDirectories().Length, 2);
            Assert.IsTrue(File.Exists(output + "/a.txt"));
            Assert.IsTrue(File.Exists(output + "/b.txt"));
            Assert.IsTrue(File.Exists(output + "/c.gif"));
            Assert.IsTrue(File.Exists(output + "/f2/c.txt"));
            Assert.IsTrue(Directory.Exists(output + "/f1"));
            Assert.IsTrue(Directory.Exists(output + "/f2"));            
        }

        [Test]
        public void ExtractZeroByteFile() {                     
            fz.CreateEmptyDirectories = true;
            fz.ExtractZip("c:/test/0byte.zip", output, "");
            DirectoryInfo di = new DirectoryInfo(output);

            Assert.AreEqual(di.GetFiles().Length, 1);
            Assert.IsTrue(File.Exists(output + "/0byte.txt"));
        }

        [Test]
        public void ExtractCourse() {           

            fz.CreateEmptyDirectories = true;
            fz.ExtractZip("c:/test/WM3ContentPackage_10000358_20060724(2).zip", output, "");            

        }


        public void ProcessFile(object sender, ICSharpCode.SharpZipLib.Core.ScanEventArgs e) {
            
            e.ContinueRunning = true;
        }

        public void ProcessDirectory(object sender, ICSharpCode.SharpZipLib.Core.DirectoryEventArgs e) {
            e.ContinueRunning = true;
        }

        public void DirectoryFailure(object sender, ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs  e) {
            e.ContinueRunning = true;
        }

        public void FailFailure(object sender, ICSharpCode.SharpZipLib.Core.ScanFailureEventArgs e) {            
            e.ContinueRunning = true;
        }
    }
}

