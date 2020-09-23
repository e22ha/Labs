using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите число от 1 до 12: ");
            string[] months = new string[] { "Зима", "Зима", "Весна", "Весна", "Весна", "Лето", "Лето", "Лето", "Осень", "Осень", "Осень", "Зима" };
            int a = int.Parse(Console.ReadLine());
            Console.WriteLine(months[a - 1].ToString());
            ConsoleKeyInfo key = Console.ReadKey();

        }
    }
}
