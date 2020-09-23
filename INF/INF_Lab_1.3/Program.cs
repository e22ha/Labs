using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_Lab_1._3
{
    class Program
    {
//Преобразование целого числа(int), в диапазоне от -128 до 127, в массив, содержащего двоичный
//код этого числа(int[]). Сигнатура функции может выглядеть следующим образом:
//static int[] intToBin(int n)
//где возвращаемое значение имеет тип массив, а входной параметр является целым числом.
//Для реализации представления отрицательных чисел, рекомендуется использовать две
//вспомогательные функции:
//static int[] invers(int[] n) //функция инвертирования значений
//static int[] addOne(int[] n) //функция добавления единицы к числу
//первая из которых инвертирует число, представленное в двоичной форме, а вторая добавляет к
//младшему разряду числа, представленного в двоичной форме, единицу.
        static void Main(string[] args)
        {
            Console.Write("Введите число от 128 до -127: ");
            int a = int.Parse(Console.ReadLine());
            printM(intToBin(a));
            Console.ReadKey();
        }
        static int[] intToBin(int n)
        {
            int[] m = new int[8];      
            m = SByteToBin((sbyte)(n));
            return m;
        }
        static void printM(int[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                Console.Write(m[i].ToString() + "");
            }
            Console.WriteLine();
        }
        static int[] SByteToBin(sbyte n)
        {
            int[] m = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 7; i >= 0; i--)
            {
                sbyte a = 1;//00000001
                m[i] = (n & a);
                n = (sbyte)(n >> 1);
            }
            return m;
        }


    }
}
