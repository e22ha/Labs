using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1._3
{
    class Program
    {
        static void swap (ref double a, ref double b)
        {
            double c = a;
            a = b;
            b = c;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Введите первое число: ");
            double a = int.Parse(Console.ReadLine());
            Console.WriteLine("Введите второе число: ");
            double b = int.Parse(Console.ReadLine());
            swap(ref a,ref b);
            Console.WriteLine(a);
            Console.WriteLine(b);
            ConsoleKeyInfo key3 = Console.ReadKey();
        }

    }
}
