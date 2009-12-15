using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ConsoleApplication1 {
    class Program {        
        static void Main(string[] args) {
            Tester t = new Tester();                    
        }
    }


    public enum EmbedImageSource {
        File,
        Http
    }

    public enum EmbedImageType {
        Jpeg,
        Gif,
        Png
    }

    [AttributeUsage( AttributeTargets.Field )]
    public class EmbedImageAttribute : Attribute {
        private String m_url;
        private EmbedImageSource m_source;
        private EmbedImageType m_type;
        public EmbedImageAttribute(String url, EmbedImageSource source, EmbedImageType type) {
            m_url = url;
            m_source = source;
            m_type = type;
        }

        public String URL {
            get {
                return m_url;
            }
        }

        public EmbedImageType Type {
            get {
                return m_type;
            }
        }

        public EmbedImageSource Source {
            get {
                return m_source;
            }
        }
    }

    public class EmbedImageGenerator {
        public static void InitEmbedImage(object obj) {            
            Type t = obj.GetType();            
            FieldInfo[] fis = t.GetFields();
            MemberInfo[] mis = t.GetMembers();
            PropertyInfo[] pis = t.GetProperties();
            foreach (FieldInfo fi in fis) {
                object[] attrs = fi.GetCustomAttributes(false);
                foreach (Attribute attr in attrs) {
                    if (attr is EmbedImageAttribute) {
                        EmbedImageAttribute ma = attr as EmbedImageAttribute;
                        
                        if (ma.Source == EmbedImageSource.File) {                                ;
                            fi.SetValue(obj, System.Drawing.Image.FromFile(ma.URL));
                        } else if (ma.Source == EmbedImageSource.Http) {
                            System.Net.WebClient client = new System.Net.WebClient();
                            byte[] bs = client.DownloadData(ma.URL);
                            using (MemoryStream ms = new MemoryStream(bs)) {
                                
                                fi.SetValue(obj, System.Drawing.Image.FromStream(ms));
                            }
                        } 
                    }
                }
            }            
        }
    }

    public class Tester {
        [EmbedImage("http://shared.live.com/HjKMzTS-xzcms40!CabizA/emoticons/smile_teeth.gif", EmbedImageSource.Http, EmbedImageType.Gif)]
        public System.Drawing.Image  imgHttp;

        [EmbedImage("d:\\logo.jpg", EmbedImageSource.File, EmbedImageType.Jpeg)]
        public System.Drawing.Image imgFile;

        public Tester() {
            // 
            EmbedImageGenerator.InitEmbedImage(this);

            Form f = new Form();            
            FlowLayoutPanel flp = new FlowLayoutPanel();
            PictureBox pic = new PictureBox();
            pic.Image = imgFile;
            pic.Size = imgFile.Size;
            flp.Controls.Add(pic);

            pic = new PictureBox();
            pic.Image = imgHttp;
            pic.Size = imgHttp.Size;
            flp.Controls.Add(pic);

            f.Controls.Add(flp);
            f.ShowDialog();
        }
    }
}
