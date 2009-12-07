using System;
using System.Collections.Generic;
using System.Text;
using Antlr.StringTemplate;
using System.IO;

namespace ConsoleApplication1 {
    class User {
        public int Age {
            get;
            set;
        }

        public String Name {
            get;
            set;
        }
    }

    class Program {
        static void Main(string[] args) {
        }

        static void loop_test(){
            String content = File.ReadAllText("loop-template.txt");


            StringTemplate st = new StringTemplate(content);
            
            st.SetAttribute("names", new String[]{"A", "B", "C"});
            Console.WriteLine(st.ToString());  
        }

        static void if_test(){
            String content = File.ReadAllText("if-template.txt");


            StringTemplate st = new StringTemplate(content);
            st.SetAttribute("sex", false); // true
            st.SetAttribute("name", "Chui-Wen Chiu");
            Console.WriteLine(st.ToString());  
        }

        static void obj_test(){
            String content = File.ReadAllText("obj-template.txt");
            User u = new User();
            u.Age = 30;
            u.Name = "Chui-Wen Chiu";

            StringTemplate st = new StringTemplate(content);            
            st.SetAttribute("user", u);
            Console.WriteLine(st.ToString());            
        }

        /// <summary>
        /// 簡易的字串代換
        /// </summary>
        static void simple_test() {
            String content = File.ReadAllText("str-template.txt");
            StringTemplate st = new StringTemplate(content);
            st.SetAttribute("name", "Chui-Wen Chiu");
            Console.WriteLine(st.ToString());
        }
    }
}
