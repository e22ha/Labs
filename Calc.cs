using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNa5
{
    class Calc
    {
        public static double Convr(double a)
        {
            return a / 2.54;
        }

        public static bool Even(double n)
        {
            return n % 2 == 0;
        }

        public static double Max(double[] mas)
        {
            double a = mas.Max();
            return a;
        }

        public static double Mod(double a, double b)
        {
            try
            {
                if (b <= 0) throw new ArgumentException("Делитель должен быть >= 0");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e}");
                throw new ArgumentException("Делитель должен быть >= 0");
            }

            return a % b;
        }
    }
}
