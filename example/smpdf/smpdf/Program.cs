using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

namespace smpdf {
    class PlanFile {
        public PlanFile(String fn) {
            m_fn = fn;    
        }

        public bool Parse(){
            var lines = File.ReadAllLines(m_fn);
            if(lines.Length<=2){
                return false;
            }

            if (!File.Exists(lines[0])) {
                Console.WriteLine("{0} not found", lines[0]);
                return false;            
            }
            InputFilename = lines[0];
            OutputFilename = lines[1];
            Pages = new List<CutRange>();
            CutRange range;
            for (int i = 2; i < lines.Length; ++i) {
                var pn = lines[i];
                if (pn.Contains('-')) {
                    var parts = pn.Split('-');
                    if (parts.Length != 2) {
                        Console.WriteLine("page {0} incorrect", pn);
                        continue;
                    }
                    int v1;
                    int v2;
                    if (!int.TryParse(parts[0], out v1) || v1 <= 0 ||
                        !int.TryParse(parts[1], out v2) || v2 <= 0) {
                        Console.WriteLine("page {0} incorrect", pn);
                        continue;
                    }

                    if (v1 > v2) {
                        Console.WriteLine("page {0} incorrect", pn);
                        continue;
                    }

                    range = new CutRange(v1, v2);
                } else {
                    int v;
                    if (!int.TryParse(pn, out v) || v<=0) {
                        Console.WriteLine("page #{0} incorrect", pn);
                        continue;
                    }

                    range = new CutRange(v, v);
                }
                Pages.Add(range);                
            }

            return true;
        }

        private String m_fn;

        public String OutputFilename {
            get;
            set;
        }

        public String InputFilename {
            get;
            set;
        }

        public List<CutRange> Pages {
            get;
            set;
        }
    }

    struct CutRange {
        public CutRange(int begin, int end) {
            Begin = begin;
            End = end;
        }

        public int Begin;
        public int End;
    }


    class PDFSplit {
        private PlanFile m_pf;
        public PDFSplit(PlanFile pf) {
            m_pf = pf;
        }

        public bool DoIt() {
            Console.WriteLine("Input File: {0}", m_pf.InputFilename);
            Console.WriteLine("Output File: {0}", m_pf.OutputFilename);
            PdfReader reader = new PdfReader(m_pf.InputFilename);
            int total_pages = reader.NumberOfPages;            
            Document document1 = new Document(reader.GetPageSizeWithRotation(1));
            PdfWriter writer1 = PdfWriter.GetInstance(document1, new FileStream(m_pf.OutputFilename, FileMode.Create));
            document1.Open();
            PdfContentByte cb1 = writer1.DirectContent;
            PdfImportedPage page;
            int rotation;
            foreach (var range in m_pf.Pages) {
                for (int i = range.Begin; i <= range.End; ++i) {
                    
                    if (i <= 0 || i > total_pages) {
                        continue;
                    }

                    Console.WriteLine("Import page #{0}", i);
                    document1.SetPageSize(reader.GetPageSizeWithRotation(i));
                    document1.NewPage();
                    page = writer1.GetImportedPage(reader, i);

                    rotation = reader.GetPageRotation(i);
                    if (rotation == 90 || rotation == 270) {
                        cb1.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                    } else {
                        cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }
            }

            document1.Close();
            return true;
        }
    }

    class Program {

        static void Main(string[] args) {
            if (args.Length == 0){
                Console.WriteLine(@"
Syntax:
    smpdf.exe script_file

Example:
    smpdf.exe d:\plain.txt

script_file format:
input pdf
output pdf
pageN
pageN1-pageN

Example:
d:\MyCopy\1064.pdf
d:\test.pdf
2
7-8
4-6

");
                return;
            }

            if (!File.Exists(args[0])) {
                Console.WriteLine("{0} not found", args[0]);
                return;
            }

            var pf = new PlanFile(args[0]);
            if (!pf.Parse()) {
                Console.WriteLine("{0} format incorrect", args[0]);
                return;
            }

            var ps = new PDFSplit(pf);
            ps.DoIt();          
        }
    }
}
