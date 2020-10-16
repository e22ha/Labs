using System;
using System.Collections;
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
            Console.Write("Sub: ");
            string strB = Console.ReadLine();
            int[] b = strToBin(strB);
            int[] result = multip(a, b);
            printM(result);

            Console.ReadKey();

        }

        private static void printM(int[] result)
        {
            for(int i =0; i < result.Length; i++)
            {
                Console.Write(result[i]);
            }
            Console.WriteLine();
        }

        private static int[] multip(int[] a, int[] b)
        {
            int[] mul = new int[16];

            for (int i =b.Length-1; i >= 0; i--)
            {
                if (b[i]==1) 
                    mul = sum(a, mul);
                    
                a = sdvig(a);
                
            }

            return mul;
        }
        private static int[] strToBin(string n)
        {
            int[] m = new int[16];
            int k = n.Length;



            for (int i = m.Length - k; i < m.Length; i++)
            {
                int a = int.Parse(n[i - (m.Length - k)].ToString());
                m[i] = a;
            }

            return m;
        }

        private static int[] sdvig(int[] n)
        {
            for (int i = 1; i < n.Length; i++)
            {
                n[i - 1] = n[i];
            }
            n[n.Length - 1] = 0;
            return n;
        }


        private static int[] addOne(int[] n, int start)
        {
            int k = 1;

            for (int i = start; i >=0; i--)
            {
                n[i] += k;
                k = 0;
                if (n[i]>1)
                {
                    n[i] = 0;
                    k = 1;
                }

                if (k == 0) break;
            }
            return n;
        }

        private static int[] sum(int[] a, int[] b)
        {
            for (int i = 15; i >=0; i--)
            {
                if (b[i] ==1)
                {
                    a = addOne(a, i);
                }
            }

            return a;
        }

    }
}

