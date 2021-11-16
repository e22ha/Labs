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
            if (a <= 0) throw new ArgumentException("The number must be > 0");
            return a / 2.54;
        }

        public static bool Even(double n)
        {
            return n % 2 == 0;
        }

        public static double Max(double[] mas)
        {
            
            if (mas.Count() <= 0) throw new ArgumentException("The count must be > 0");
            double a = mas.Max();
            return a;
        }

        public static double Mod(double a, double b)
        {
            if (b <= 0) throw new ArgumentException("The divisor must be > 0");

            return a % b;
        }
    }
}
