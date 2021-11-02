using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNa5
{
    class Calc
    {
        public double convr(double a)
        {
            return a / 2.54;
        }

        public bool Even(double n)
        {
            return n % 2 == 0;
        }

        public double max(double[] mas)
        {
            double a = mas.Max();
            return a;
        }

        public double mod(double a, double b)
        {

            if (b <= 0) throw new ArgumentException("Делитель должен быть >= 0");
            Console.WriteLine("Enter a value greater than 0");


            return a % b;
        }
    }
}
