using System;

namespace PiEstimator;

class Program
{
    static void Main(string[] args)
    {
        long n;
        
        Console.WriteLine("Pi Estimator");
        Console.WriteLine("================================================");

        n = GetNumber("Enter number of iterations (n): ");

        decimal pi = EstimatePi(n);
        decimal diff = Decimal.Abs(pi - (decimal)Math.PI);
        
        Console.WriteLine();
        Console.WriteLine($"Pi (estimate): {pi}, Pi (system): {Math.PI}, Difference: {diff}");
    }

    static decimal EstimatePi(long n)
    {
        Random rand = new Random(System.Environment.TickCount);
        decimal pi = 0.0m; // m means decimal number type

        // TODO: Calculate Pi

        return pi;
    }

    static long GetNumber(string prompt)
    {
        long output;

        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (Int64.TryParse(input, out output))
            {
                return output;
            }
        }
    }
}
