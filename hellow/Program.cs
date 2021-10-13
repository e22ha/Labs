using System;

namespace hellow
{
    class Program
    {
        static void Main(string[] args)
        {   
            Console.WriteLine("Enter A:/n");
            
            Console.WriteLine("Enter B: /n");
          
            Console.ReadKey();
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
