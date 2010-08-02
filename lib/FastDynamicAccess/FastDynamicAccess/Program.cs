using System;
using System.Collections.Generic;
using System.Text;

namespace FastDynamicAccess {
    class Person {
        public Person(String name, int age) {
            _name = name;
            _age = age;
        }

        public int Age{
            get {
                return _age;
            }
        }
        private int _age;

        public String Name {
            get {
                return _name;
            }
        }
        private String _name;
    }
    class Program {
        static void Main(string[] args) {
            Person p = new Person("Arick", 26);
            Reflection.TypeUtility<Person>.MemberGetDelegate<int> getAge = Reflection.TypeUtility<Person>.GetMemberGetDelegate<int>("Age");
            int age = getAge(p);
            Console.WriteLine("Age " + age.ToString());   
        }
    }
}
