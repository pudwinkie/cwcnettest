using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Office.Interop.Word;

namespace applyWordTemplate {
    class Program {
        static void Main(string[] args) {
            // 複製一份樣板
            String root = Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location);
            String template = Path.Combine(root, @"template\\template.doc");
            String target_folder = Path.Combine(root, "result");
            if (!Directory.Exists(target_folder)) {
                Directory.CreateDirectory(target_folder);
            }

            String target = Path.Combine(target_folder, Guid.NewGuid().ToString() + ".doc");
            File.Copy(template, target, true);

            // 打開文件
            object oMissing = System.Reflection.Missing.Value;
            Application oWord = null;
            Document oDoc = null;
            try {
                oWord = new ApplicationClass();
                oWord.Visible = false;

                //Document oDoc;
                //Word._Application oWord;
                //Word._Document oDoc;
                //oWord = new Word.Application();

                object fileName = target;
                oDoc = oWord.Documents.Open(ref fileName,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                // 套版
                IDictionary<String, String> StringTable = new Dictionary<String, String>();
                StringTable.Add("{NAME}", "Chui-Wen Chiu");
                StringTable.Add("{BIRTHDAY}", "2009/01/01");
                foreach (KeyValuePair<String, String> s in StringTable) {
                    ReplaceStr(oDoc, s.Key, s.Value);
                    //Replace(oDoc, "{NAME}", "Arick");
                }
                IDictionary<String, String> PicTable = new Dictionary<String, String>();
                PicTable.Add("{PIC}", "d:\\logo.jpg");
                
                ReplacePic(oWord, oDoc, "{PIC}", "d:\\logo.jpg");
            } finally {
                // 存檔
                //String fn = Path.GetTempFileName();
                oDoc.Save();
                oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            }
            //Guid.NewGuid().ToString(); 
            
            
        }

        public static bool ReplaceStr(Document oDoc, string strOldText, string strNewText) {
            oDoc.Content.Find.Text = strOldText;
            object FindText, ReplaceWith, Replace;// 
            object MissingValue = Type.Missing;
            FindText = strOldText;//要查找的文本
            ReplaceWith = strNewText;//替换文本
            Replace = WdReplace.wdReplaceAll;/*wdReplaceAll - 替换找到的所有项。
                                                      * wdReplaceNone - 不替换找到的任何项。
                                                    * wdReplaceOne - 替换找到的第一项。
                                                    * */
            oDoc.Content.Find.ClearFormatting();//移除Find的搜索文本和段落格式设置
            if (oDoc.Content.Find.Execute(
                ref FindText, ref MissingValue,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue, ref MissingValue,
                ref ReplaceWith, ref Replace,
                ref MissingValue, ref MissingValue,
                ref MissingValue, ref MissingValue)) {
                return true;
            }
            return false;
        }

        public static bool ReplacePic(Application oWord, Document oDoc, string strOldText, string strNewText) {
            Selection sel = oWord.Selection;
            Find f = sel.Find;

            f.ClearFormatting();
            f.Text = strOldText;
            f.Replacement.Text = "";
            f.Forward = true;
            f.Wrap = WdFindWrap.wdFindContinue;
            f.Format = false;
            f.MatchCase = true;
            f.MatchWholeWord = false;
            f.MatchByte = true;
            f.MatchWildcards = false;
            f.MatchSoundsLike = false;
            f.MatchAllWordForms = false;

            object mv = Type.Missing;
            object oFalse = false;
            object oTrue = true;
            if (f.Execute(ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv, ref mv)) {
                sel.InlineShapes.AddPicture(strNewText, ref oFalse, ref oTrue, ref mv);
                return true;
            }

            return false;           
        }

    }


}
