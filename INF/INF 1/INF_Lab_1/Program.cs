using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_Lab_1
{
    class Program
    {
    //Преобразование переменной типа байт со знаком(sbyte) в массив, содержащий двоичный код
    //числа, лежащего в переменной.Сигнатура функции может выглядеть следующим образом:
    //static int[] sbyteToBin(sbyte n)
    //где возвращаемое значение имеет тип массив, а параметр имеет тип байт со знаком.
    //Преобразование массива целых чисел в строку.Сигнатура функции может выглядеть следующим
    //образом: static string binToStr(int[] n)
    //где возвращаемое значение имеет тип строка, а параметр имеет тип массив.

        static void Main(string[] args)
        {
            Console.Write("Введите число от 127 до -128: ");
            int a = int.Parse(Console.ReadLine());//Чтение числа из консоли
            sbyte r = (sbyte)(a);
            
            //printM(SByteToBin(r));
            Console.WriteLine(binToStr(SByteToBin(r)));
            Console.ReadKey();
        }
        
        //Функция перевода массива в строку
        static string binToStr(int[] n)
        {
            string str;

            str = String.Concat<int>(n);

            return str;
        }
        //Функция вывода массива в консоль
        static void printM(int[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                Console.Write(m[i].ToString() + "");
            }
            Console.WriteLine();
        }
        //Функция перевода sbyte в int[]
        static int[] SByteToBin(sbyte n)
        {
            int[] m = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 7; i >= 0; i--)
            {
                m[i] = n & 1; //операция & с 1 накладывает маску на число sbyte и записывает разряд еденицы числа
                n = (sbyte)(n >> 1); //Сдвиг вчисла на один разряд
            }
            return m;
        }

    }
}
