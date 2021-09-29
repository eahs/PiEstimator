using System;

namespace PiEstimator
{
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            
            Console.WriteLine("Pi Estimator");
            Console.WriteLine("================================================");

            n = GetNumber("Enter number of iterations (n): ");

            double pi = EstimatePi(n);
            double diff = Math.Abs(pi - Math.PI);

            Console.WriteLine();
            Console.WriteLine($"Pi (estimate): {pi}, Pi (system): {Math.PI}, Difference: {diff}");
        }

        static double EstimatePi(int n)
        {
            Random rand = new Random(System.Environment.TickCount);
            double pi = 0.0;
            
            

            return pi;
        }

        static int GetNumber(string prompt)
        {
            int output;

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (Int32.TryParse(input, out output))
                {
                    return output;
                }
            }
        }
    }
}