using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace INF_Lab_1._4
{
    class Program
    {
        //        Преобразование массива целых чисел, содержащего двоичный код числа(int[]), в переменную
        //типа целое число(int), содержащую это число.Сигнатура функции может выглядеть следующим
        //образом: static int binToInt(int[] n)
        //9
        //где возвращаемое значение имеет тип целое число, а входной параметр является массивом.
        //Для преобразования отрицательных чисел следует использовать разработанные ранее функции
        //invers и addOne.
        //Для получения двоичного представления числа в виде массива, может быть использована функция
        //strToBin, разработанная ране.
        static void Main(string[] args)
        {
            Console.Write("Введите 8 бит числа: ");
            string str = Console.ReadLine();
            Console.WriteLine(binToInt(strToBin(str)));
            Console.ReadKey();
        }
        //Перевод int[] в int
        static int binToInt(int[] n)
        {
            int a = 0;
            if (n[0] == 0)//Для положительного
            {
                for (int i = 0; i < 8; i++)
                {
                    a = a + (n[i] * (int)Math.Pow(2, (7 - i)));
                }
            }
            else//Для отрицательного
            {
                n[7] = n[7] - 1;
                n = Invert(n);
                for (int i = 0; i < 8; i++)
                {
                    a = a - (((n[i])) * (int)Math.Pow(2, (7 - i)));
                }
            }
            return a;
        }
        //Перевод string в int[]
        static int[] strToBin(string n)
        {
            int[] m = new int[8]{0,0,0,0,0,0,0,0};
            int k = n.Length;
            for (int i = 8-k; i <= 7; i++)
            {
               int a = int.Parse( n[i - (8 - k)].ToString());
               m[i] = a;
            } 
            return m;
        }
        //Перевод int[] в sbyte
        static SByte BinToSByte(int[] n)
        {
            sbyte a = 0;
            for (int i = 7; i >= 0; i--)
            {
                a = (sbyte)(a << 1);
                sbyte b = (sbyte)n[7 - i];
                a = (sbyte)(a | b);
            }
            return a;
        }
        //Инвертирование массива
        static int[] Invert(int[] n) 
        { 
            for (int i = 0; i<8; i++)
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
    }
}
