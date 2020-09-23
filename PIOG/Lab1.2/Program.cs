using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите символ: ");
            ConsoleKeyInfo key = Console.ReadKey();
            char x = key.KeyChar;
            Console.WriteLine();
            if (Char.IsDigit(x) == true)
            {
                Console.WriteLine("Да");
            }
            else
            {
                Console.WriteLine("Нет");
            }
            ConsoleKeyInfo key1 = Console.ReadKey();
        }
    }

}
