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
            Console.Write("Введите число от 127 до -128: ");
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
        static int[] Invert(int[] n)
        {
            for (int i = 0; i <= 7; i++)
            {
                if (n[i] == 0)
                {
                    n[i] = 1;
                }
                else
                {
                    n[i] = 0;
                }
            }
            return n;
        }
        static int[] SByteToBin(sbyte n)
        {
            int[] m = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            double z = (double)n;
            if (n >= 0) 
            {
                for (int i = 7; i >= 0; i--)
                {
                    double a = z % 2;
                    m[i] = (int)a;
                    z = (z / 2);
                }
            }
            else
            {
                for (int i = 7; i >= 0; i--)
                {
                    double a = z % 2;
                    m[i] = (int)a;
                    z = Math.Abs(z / 2);
                }
                Invert(m);
                m[7] = m[7] + 1;

                for (int i = 7; i > 0; i--)
                {
                    if (m[i] == 2)
                    {
                        m[i] = 0;
                        m[i - 1] = m[i - 1] + 1;
                    }
                }
                

            }
            return m;
        }


    }
}
