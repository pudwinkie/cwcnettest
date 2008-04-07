#region using
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;
#endregion

namespace SharpZibTest {
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class JSharpTest {
        #region private data
        #endregion

        #region constructor
        #endregion
        private JZip jz;        
        private const string output = "c:/test/outputAll";     

        [SetUp]
        public void BeforeProcess() {
            jz = new JZip();
            jz.ProcessFile += delegate(object sender, ProcessFileEventArgs e){
                Console.WriteLine(e.Filename);
            };

            jz.ProcessFolder += delegate(object sender, ProcessFolderEventArgs e){
                Console.WriteLine(e.Folder);
            };
            if (Directory.Exists(output)) {
                Directory.Delete(output, true);
            }
        }

        [TearDown]
        public void AfterProcess() {
            jz = null;            
            if (Directory.Exists(output)) {
                Directory.Delete(output, true);
            }
        }

        [Test]
        public void ExtractCourse() {

            
            jz.ExtractZip("c:/test/WM3ContentPackage_10000358_20060724(2).zip", output, "");

        }
        #region public method
        [Test]
        public void ExtractTxt() {            
            jz.ExtractZip("c:/test/test.zip", output, ".txt");

            DirectoryInfo di = new DirectoryInfo(output);
            Assert.AreEqual(di.GetFiles().Length, 2);
            Assert.IsTrue(File.Exists(output + "/a.txt"));
            Assert.IsTrue(File.Exists(output + "/b.txt"));            
        }

        [Test]
        public void ExtractAll() {            
            jz.ExtractZip("c:/test/test.zip", output);

            DirectoryInfo di = new DirectoryInfo(output);

            Assert.AreEqual(di.GetFiles().Length, 3);
            Assert.IsTrue(File.Exists(output + "/a.txt"));
            Assert.IsTrue(File.Exists(output + "/b.txt"));
            Assert.IsTrue(File.Exists(output + "/c.gif"));
        }

        [Test]
        public void ExtractZeroByteFile() {            
            jz.ExtractZip("c:/test/0byte.zip", output, "");
            DirectoryInfo di = new DirectoryInfo(output);

            Assert.AreEqual(di.GetFiles().Length, 1);
            Assert.IsTrue(File.Exists(output + "/0byte.txt"));
        }
        /// <summary>
        /// 取出所有的目錄和檔案
        /// </summary>
        [Test]
        public void ExtractFolderAndFile() {            
            jz.ExtractZip("c:/test/test2.zip", output);
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
        #endregion

        #region properties
        #endregion
    }
}
