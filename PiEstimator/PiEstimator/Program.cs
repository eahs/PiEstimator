using System;

namespace PiEstimator;

/// <summary>
/// Console application that estimates the value of Pi using random sampling
/// and compares the estimate to <see cref="Math.PI"/>.
/// </summary>
class Program
{
    /// <summary>
    /// Entry point of the application. Prompts the user for the number of
    /// iterations, estimates Pi, and prints the estimate alongside the system
    /// value of Pi and the difference between them.
    /// </summary>
    /// <param name="args">Command-line arguments (not used).</param>
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

    /// <summary>
    /// Estimates the value of Pi using <paramref name="n"/> random samples.
    /// </summary>
    /// <param name="n">
    /// The number of iterations (random samples) to use. Larger values should
    /// generally produce a more accurate estimate.
    /// </param>
    /// <returns>The estimated value of Pi as a <see cref="decimal"/>.</returns>
    static decimal EstimatePi(long n)
    {
        Random rand = new Random(System.Environment.TickCount);
        decimal pi = 0.0m; // m means decimal number type

        // TODO: Calculate Pi

        return pi;
    }

    /// <summary>
    /// Repeatedly prompts the user until they enter a valid whole number.
    /// </summary>
    /// <param name="prompt">The message displayed to the user before each input attempt.</param>
    /// <returns>The number entered by the user, parsed as a <see cref="long"/>.</returns>
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
