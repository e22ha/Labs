using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_2._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("First: ");
            string strA = Console.ReadLine();
            int[] a = strToBin(strA);
            Console.Write("Sub: ");
            string strB = Console.ReadLine();
            int[] b = strToBin(strB);
            int[] sum = masSub(a, b);
            printM(sum);


            Console.ReadKey();

        }
        static int[] strToBin(string n)
        {
            int[] m = new int[9] {0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int k = n.Length;



            for (int i = m.Length - k; i <= 8; i++)
            {
                int a = int.Parse(n[i - (9 - k)].ToString());
                m[i] = a;
            }

            return m;
        }
        static int[] masSub(int[] a, int[] b)
        {
            int[] s = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            b = Invert(b);
            addone(b);
            for (int i = 8; i >= 1; i--)
            {
                s[i] = a[i] + b[i] + s[i];
                if (s[i] == 3)
                {
                    s[i] = 1;
                    s[i - 1] = 1;
                }
                else if (s[i] > 1)
                {
                    s[i] = 0;
                    s[i - 1] = 1;
                }
            }

            return s;
        }

        static void printM(int[] m)
        {
            for (int i = 1; i < m.Length; i++)
            {
                Console.Write(m[i].ToString() + "");
            }
            Console.WriteLine();
        }

        static int[] Invert(int[] n)
        {
            for (int i = 0; i < 9; i++)
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
        static void addone(int[] m) {
            m[m.Length-1] = m[m.Length-1] + 1;

            for (int i = m.Length-1; i >= 0; i--)
            {
                if (m[i] == 2)
                {
                    m[i] = 0;
                    m[i - 1] =+ 1;
                }
                else break;
            }
        }
    }
}

