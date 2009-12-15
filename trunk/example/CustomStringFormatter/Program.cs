using System;
using System.Text;
namespace ConsoleApplication1 {
    public class CCFormatProvider : IFormatProvider {
        public Object GetFormat(Type formatType) {
            // 型別檢驗
            if (formatType == typeof(CC)) {
                return new CustomerFormatProvider(); 
            }

            return null;
        }

        public class CustomerFormatProvider : ICustomFormatter {
            public CustomerFormatProvider() {
            }

            public string Format(string format, object arg, IFormatProvider formatProvider){
                CC a = arg as CC;
                if (a == null) {
                    return a.ToString(); 
                } else {
                    return string.Format("{0}/{1}", a.Length, a.CurrentLength ); 
                }
            }
        }
    }

    public class CC : IFormattable {
        private int length = 100;

        public int Length {
            get { return length; }
            set { length = value; }
        }
        private int currentLength = 200;

        public int CurrentLength {
            get { return currentLength; }
            set { currentLength = value; }
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider) {
            if (formatProvider != null) {
                ICustomFormatter fmt = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;

                
                if (fmt != null) {
                    return fmt.Format(format, this, formatProvider);  
                }
            }

            switch (format) {
                case "l":
                    return length.ToString();  

                case "c":
                    return currentLength.ToString();   

                default:
                    return string.Format("Length: {0}, Current Length: {1}", length, currentLength); 
            }
            
        }

        public static void Test() {
            IFormattable cf = new CC();
            Console.WriteLine(cf.ToString("l", null));      // 100
            Console.WriteLine(cf.ToString("c", null));      // 200
            Console.WriteLine(cf.ToString(null, null));     // Length: 100, Current Length: 200
            Console.WriteLine(cf.ToString("", null));       // Length: 100, Current Length: 200
            Console.WriteLine(cf.ToString("null", null));   // Length: 100, Current Length: 200

            Console.WriteLine( string.Format( new CCFormatProvider(), "", cf));    
            //Console.WriteLine( cf.ToString("", new CCFormatProvider()));    
        }        
    }

    class CD {
        public static void Test(){
            StringBuilder s1 = new StringBuilder("AAA");            
            StringBuilder s2 = s1;
            s2.Length = 0;
            s2.Append("BBB");
            Console.WriteLine(s1.ToString());  
        }
    }
    class Program {
        static void Main(string[] args) {
            //CC.Test(); 
            CD.Test();
        }
    }
}
