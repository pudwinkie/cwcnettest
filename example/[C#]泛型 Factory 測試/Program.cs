/// ªx«¬ Factory

using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplication2 {
    public interface ITest {
        void DoIt();
    }

    class Factory <T> where T: new() {
        public static T Create(){
            return new T();
        }
    }

    class parent : ITest {
        public void DoIt() {
            Console.WriteLine("DoIt"); 
        }

        public virtual void GetSomething(){
            Console.WriteLine("parent");  
        }

        public static parent GetObject<T>() {
            if (instance == null) {
                instance = System.Activator.CreateInstance<T>() as parent;
            }
            return instance;
        }

        private static parent instance;
    }

    class child :parent{
        public override void GetSomething() {
            Console.WriteLine("child");  
        }
    }

    class Program {
        static void Main(string[] args) {
            parent x = parent.GetObject<child>();
            DoIt(x);

            parent y = parent.GetObject<parent>();
            DoIt(y);

            parent z = Factory<parent>.Create();
            DoIt(z);

        }

        static void DoIt(parent x){
            x.GetSomething();
            x.DoIt();
        }
    }


}
