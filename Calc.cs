using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNa5
{
    class Calc
    {
        public static double convr(int a)
        {
            return a / 2.54;
        }

        public static bool Even(int n)
        {
            return n % 2 == 0;
        }

        public static int max(int[] mas, int a)
        {
            a = mas.Max();
            return a;
        }

        public static int mod(int a, int b)
        {
            if (b <= 0)
            {
                Console.WriteLine("Enter a value greater than 0");
            }

            return a % b;
        }
    }
}
