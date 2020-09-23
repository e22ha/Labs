using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._7
{
    struct student
    {
        public string name;
        public string lastName;
        public string o;
        public byte math;
        public student( string Name, string LastName, string O, byte Math)
        {
            name = Name;
            lastName = LastName;
            o = O;
            math = Math;
        }
        public void WriteUserInfo()   // метод для вывода
        {
            Console.WriteLine("ФИО: {0} {1} {2}, Math: {3}", name, lastName, o, math);
        }
        public static student Reg()
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine();
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите отчество: ");
            string o = Console.ReadLine();
            Console.Write("Введите отметку: ");
            byte math = byte.Parse(Console.ReadLine());
            return new student (name,lastName,o,math);
        }
    }
    class Program
    {
        static void Main()
        {
            List<student> myList = new List<student>();
           
            for (int i = 1; i <= 10; i++)
            {
                Console.WriteLine("Добавить студента?");
                Console.WriteLine("(y)es or (n)o?");
                char ans = char.Parse(Console.ReadLine());
                if (ans == 'y')
                {
                    myList.Add(student.Reg());
                }
                else{ i =i + 11; }
            }
            for (int i = 0; i < myList.Count; i++)
            {
                var item = myList[i];
                item.WriteUserInfo();
            }

            Console.ReadKey();
        }
    }
}