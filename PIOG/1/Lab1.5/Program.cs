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
                Console.WriteLine("Это простое число!");
            }
            else
            {
                Console.WriteLine("Это не простое число!");
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
            if (number <= 1) return false; //Отбрасываем 0 и 1
            if (number == 2) return true; // 2 - это простое число
            if (number % 2 == 0) return false; // Чётное = простое

            var a = (int)Math.Sqrt(number); //Находим корень из числа. Идти нужно до корня так как дальше мы будем получать второй множетель числа

            for (int i = 3; i <= a; i += 2)//Шаг через 2, нужны только нечётные
                if (number % i == 0)
                    return false; //Если делиться значит не простое

            return true;//все остальные простые
        }
    }
}
