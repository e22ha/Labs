using System;

namespace TestNa5
{
    class Program
    {
        static void Main(string[] args)
        {
            int a = 0;
            string op;

            Console.WriteLine("\tConsole Calculator in C#\r");
            Console.WriteLine("\t------------------------\n");

            Console.WriteLine("\tSelect an operation:\n");
            Console.WriteLine("\t1.Convert centimeters to inches\n");
            Console.WriteLine("\t2.Checking a number for evenness\n");
            Console.WriteLine("\t3.The largest value from array\n");
            Console.WriteLine("\t4.Calculation of the remainder of division\n");
            op = Console.ReadLine();

            if (op == "1")
            {
                Console.WriteLine("\tEnter length in centimeters:\n");
                a = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(Calc.convr(a) + " inches");
            }
            else if (op == "2")
            {
                Console.WriteLine("\tEnter the number:\n");
                a = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine(Calc.Even(a));
            }
            else if (op == "3")
            {
                Console.WriteLine("\tEnter length of array:\n");
                int l = Convert.ToInt32(Console.ReadLine());
                int[] mas = new int[l];

                Console.WriteLine("\tEnter an array:\n");
                for (int i = 0; i < l; i++)
                {
                    mas[i] = Convert.ToInt32(Console.ReadLine());
                }

                Console.WriteLine("\tThe highest number is ");
                Console.WriteLine(Calc.max(mas, a));
            }
            else if (op == "4")
            {
                Console.WriteLine("\tEnter the number a:\n");
                a = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\tEnter the number b:\n");
                int b = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("\tThe remainder of dividing " + a + " by " + b);
                Console.WriteLine(Calc.mod(a, b));
            }

            Console.ReadKey();
        }
    }
}
