using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_2._3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("First: ");
            string strA = Console.ReadLine();
            int[] a = strToBin(strA);
            Console.Write("Sec: ");
            string strB = Console.ReadLine();
            int[] b = strToBin(strB);
            int[] rec = masMul(a, b);
            printM(rec);


            Console.ReadKey();

        }

        private static int[] masMul(int[] a, int[] b)
        {
            int[] s = new int[] { };
            
            return s;
        }

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
        static void printM(int[] m)
        {
            for (int i = 0; i < m.Length; i++)
            {
                Console.Write(m[i].ToString() + "");
            }
            Console.WriteLine();
        }
        static int[] masAdd(int[] a, int[] b)
        {
            int[] s = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 7; i >= 0; i--)
            {
                s[i] = a[i] + b[i] + s[i];
                if (s[i] == 3)
                {
                    s[i] = 1;
                    s[i - 1] = 1;
                }
                if (s[i] > 1)
                {
                    s[i] = 0;
                    s[i - 1] = 1;
                }
            }

            return s;
        }
    }
}
