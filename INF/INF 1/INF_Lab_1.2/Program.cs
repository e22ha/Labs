using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_Lab_1._2
{
    class Program
    {
        //Преобразование строки, длиною не более 8 символов, содержащей двоичное представление числа
        //(string), в массив, содержащий двоичное представление числа(int[]). Сигнатура функции может
        //выглядеть следующим образом: static int[] strToBin(string n)
        //где возвращаемое значение имеет тип массив, а входной параметр является строкой.
        //Преобразование массива, содержащего двоичный код числа (int[]), в переменную типа целое байт
        //со знаком (sbyte), содержащую это число.Сигнатура функции может выглядеть следующим
        //образом: static sbyte binToSByte(int[] n)
        //где возвращаемое значение имеет тип байт со знаком, а входной параметр является массивом.
        static void Main(string[] args)
        {
            Console.Write("Введите 8 бит числа: ");
            string str = Console.ReadLine();
            Console.WriteLine(BinToSByte(strToBin(str)));
            Console.ReadKey();
        }
        //Функция перевода string в int[]
        static int[] strToBin(string n)
        {
            int[] m = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            int k = n.Length;
            for (int i = 8 - k; i <= 7; i++)
            {
                int a = int.Parse(n[i - (8 - k)].ToString());
                m[i] = a;
            }
            return m;
        }
        //Процедура вывода массива в строку
        static void printM(int[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                Console.Write(m[i].ToString() + "");
            }
            Console.WriteLine();
        }
        //Функция перевода int[] (массива) в sbyte
        static SByte BinToSByte(int[] n)
        {
            sbyte a = 0;
            for(int i = 7; i >= 0; i--)
            {
                a = (sbyte)(a << 1);
                sbyte b = (sbyte)n[7-i]; 
                a = (sbyte)(a | b);
            }
            return a;
        }
    }
}
