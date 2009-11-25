using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using IronPython.Hosting;
using IronPython.Compiler;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;
using System.IO;
namespace WindowsFormsApplication2 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            ScriptEngine pye = Python.CreateEngine(AppDomain.CurrentDomain);
            // 設定搜尋目錄
            List<string> searchPaths = new List<string>();
            searchPaths.Add(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            pye.SetSearchPaths(searchPaths);

            // 資料導入 Python 執行環境
            ScriptScope scope = pye.CreateScope();            
            Form frm = new Form();
            scope.SetVariable("frm", frm);

            ScriptRuntime runtime = pye.Runtime;
            using (MemoryStream stream = new MemoryStream(1024)) {

                    runtime.IO.SetOutput(stream, System.Text.UTF8Encoding.UTF8);
                    runtime.IO.SetErrorOutput(stream, System.Text.UTF8Encoding.UTF8);
                    pye.ExecuteFile("my.py", scope);
                    stream.Position = 0;
                    String txt; // 輸出結果
                    using (TextReader tr = new StreamReader(stream))
                        txt = tr.ReadToEnd();

                    object v;
                    pye.TryGetVariable(scope, "pyobj", out v);

                    String sv;
                    pye.TryGetVariable<String>(scope, "pyobj",out sv);
            }

            

        }
    }
}
