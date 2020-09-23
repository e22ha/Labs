using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._5
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Write("Введите число: ");
            int a = int.Parse(Console.ReadLine());
            if (IsPrime(a) == true)
            {
                Console.WriteLine("Это простое число");
            }
            else
            {
                Console.WriteLine("Это не простое число");
            }
            for (int i = 0; i <= a; i++)
            {
                if (IsPrime(i) == true)
                {
                    Console.WriteLine(i);
                }
            }
            ConsoleKeyInfo key = Console.ReadKey();

        }

        public static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var a = (int)Math.Sqrt(number);

            for (int i = 3; i <= a; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }
    }
}
