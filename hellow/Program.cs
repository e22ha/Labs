using System;

namespace hellow
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Enter A:\n");
            int A = Convert.ToInt32(Console.ReadLine());            
            Console.WriteLine("Enter B: \n");
            int B = Convert.ToInt32(Console.ReadLine());  

            Console.WriteLine(plus(A,B));
            Console.WriteLine(minus(A,B));
            Console.WriteLine(multi(A,B));
            Console.WriteLine(div(A,B));

          
        }


        static int div(int A, int B)
        {
            return A / B;
        }

        static int plus(int A, int B)
        {
            return A + B;
        }
      
        static int multi(int A, int B)
        {
            return A * B;
        }
      
        static int minus(int A, int B)
        { 
            return A-B;
        }
    }
}
