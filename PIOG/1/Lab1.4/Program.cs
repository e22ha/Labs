using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._4
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            Console.Write("Введите минимальное значение: ");
            int min = int.Parse(Console.ReadLine());
            Console.Write("Введите максимально значение: ");
            int max = int.Parse(Console.ReadLine());
            if (max <= min)
            {
                Console.WriteLine("Введите число побольше 😊");
                max = int.Parse(Console.ReadLine());
            }
            int[,] numbers = new int[5, 5];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    numbers[i, j] = rnd.Next(min, max);
                }
            }
            foreach (int element in numbers)
            {
                Console.WriteLine(element);
            }
            ConsoleKeyInfo key = Console.ReadKey();

        }
    }
}
