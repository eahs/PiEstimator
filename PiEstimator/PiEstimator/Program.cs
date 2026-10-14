using System;
using System.Diagnostics;
using System.Text;

namespace PiEstimator;

/// <summary>
/// Console application that estimates the value of Pi using random sampling
/// and compares the estimate to the true value of Pi.
/// </summary>
class Program
{
    /// <summary>
    /// Pi to the full precision a <see cref="decimal"/> can hold (more digits than <see cref="Math.PI"/>).
    /// </summary>
    const decimal ActualPi = 3.1415926535897932384626433833m;

    /// <summary>
    /// The digits of Pi used when checking how many digits of the estimate are correct.
    /// </summary>
    const string PiDigits = "3.14159265358979323846264338327950288";

    /// <summary>
    /// Inner width (in characters) of the boxes drawn on screen.
    /// </summary>
    const int BoxWidth = 50;

    /// <summary>
    /// Entry point of the application. Prompts the user for the number of
    /// iterations, estimates Pi, and prints a report comparing the estimate
    /// to the true value of Pi.
    /// </summary>
    /// <param name="args">Command-line arguments (not used).</param>
    static void Main(string[] args)
    {
        long n;

        Console.OutputEncoding = Encoding.UTF8;

        PrintBanner();

        n = GetNumber("  Enter number of iterations (n): ");

        Console.WriteLine();
        WriteColored("  Throwing random numbers at the problem...", ConsoleColor.DarkGray);
        Console.WriteLine();

        Stopwatch timer = Stopwatch.StartNew();
        decimal pi = EstimatePi(n);
        timer.Stop();

        PrintResults(n, pi, timer.Elapsed);
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
    /// Repeatedly prompts the user until they enter a valid positive whole number.
    /// </summary>
    /// <param name="prompt">The message displayed to the user before each input attempt.</param>
    /// <returns>The number entered by the user, parsed as a <see cref="long"/>.</returns>
    static long GetNumber(string prompt)
    {
        long output;

        while (true)
        {
            WriteColored(prompt, ConsoleColor.Yellow);
            string input = Console.ReadLine();

            if (Int64.TryParse(input?.Replace(",", ""), out output) && output > 0)
            {
                return output;
            }

            WriteColored("  Please enter a whole number greater than 0.", ConsoleColor.Red);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Prints the title banner at the top of the program.
    /// </summary>
    static void PrintBanner()
    {
        Console.WriteLine();
        WriteColored("  ╔" + new string('═', BoxWidth) + "╗", ConsoleColor.Cyan);
        Console.WriteLine();
        WriteColored("  ║" + Center("π   E S T I M A T O R", BoxWidth) + "║", ConsoleColor.Cyan);
        Console.WriteLine();
        WriteColored("  ║" + Center("3.14159265358979323846264338327950288...", BoxWidth) + "║", ConsoleColor.DarkCyan);
        Console.WriteLine();
        WriteColored("  ╚" + new string('═', BoxWidth) + "╝", ConsoleColor.Cyan);
        Console.WriteLine();
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the results box, the digit-by-digit comparison, an accuracy meter,
    /// and a rating message for the estimate.
    /// </summary>
    /// <param name="n">The number of iterations used for the estimate.</param>
    /// <param name="pi">The estimated value of Pi.</param>
    /// <param name="elapsed">How long the estimate took to calculate.</param>
    static void PrintResults(long n, decimal pi, TimeSpan elapsed)
    {
        decimal diff = Decimal.Abs(pi - ActualPi);
        decimal percentError = diff / ActualPi * 100m;
        string estimateText = Math.Round(pi, 15).ToString();
        int correctDigits = CountCorrectDigits(estimateText);

        Console.WriteLine();
        WriteColored("  ┌─ Results " + new string('─', BoxWidth - 10) + "┐", ConsoleColor.Cyan);
        Console.WriteLine();
        PrintRow("Iterations", n.ToString("N0"), ConsoleColor.White);
        PrintRow("Estimate", estimateText, ConsoleColor.Yellow);
        PrintRow("Actual π", Math.Round(ActualPi, 15).ToString(), ConsoleColor.Green);
        PrintRow("Difference", Math.Round(diff, 15).ToString(), ConsoleColor.Magenta);
        PrintRow("Error", $"{percentError:0.######} %", ConsoleColor.Magenta);
        PrintRow("Time", FormatTime(elapsed), ConsoleColor.White);
        WriteColored("  └" + new string('─', BoxWidth) + "┘", ConsoleColor.Cyan);
        Console.WriteLine();
        Console.WriteLine();

        PrintDigitComparison(estimateText, correctDigits);
        PrintAccuracyMeter(correctDigits);
        PrintRating(pi, correctDigits);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints one labeled row inside the results box.
    /// </summary>
    /// <param name="label">The name of the value shown on the left.</param>
    /// <param name="value">The value shown on the right.</param>
    /// <param name="valueColor">The color used to draw the value.</param>
    static void PrintRow(string label, string value, ConsoleColor valueColor)
    {
        string labelText = " " + label.PadRight(14);
        string valueText = value.PadRight(BoxWidth - labelText.Length);

        WriteColored("  │", ConsoleColor.Cyan);
        WriteColored(labelText, ConsoleColor.Gray);
        WriteColored(valueText, valueColor);
        WriteColored("│", ConsoleColor.Cyan);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints the estimate with correct digits in green, the first wrong digit
    /// in red, and the remaining digits dimmed, with the real digits of Pi underneath.
    /// </summary>
    /// <param name="estimateText">The estimate formatted as text.</param>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    static void PrintDigitComparison(string estimateText, int correctDigits)
    {
        WriteColored("  Your digits:  ", ConsoleColor.Gray);

        int digitsSeen = 0;
        bool mismatchShown = false;

        foreach (char c in estimateText)
        {
            if (c == '.')
            {
                WriteColored(".", digitsSeen <= correctDigits ? ConsoleColor.Green : ConsoleColor.DarkGray);
                continue;
            }

            digitsSeen++;

            if (digitsSeen <= correctDigits)
            {
                WriteColored(c.ToString(), ConsoleColor.Green);
            }
            else if (!mismatchShown)
            {
                WriteColored(c.ToString(), ConsoleColor.Red);
                mismatchShown = true;
            }
            else
            {
                WriteColored(c.ToString(), ConsoleColor.DarkGray);
            }
        }

        Console.WriteLine();
        WriteColored("  Real digits:  ", ConsoleColor.Gray);
        WriteColored(PiDigits.Substring(0, Math.Max(estimateText.Length, 2)), ConsoleColor.DarkGreen);
        Console.WriteLine();
        Console.WriteLine();
    }

    /// <summary>
    /// Prints a bar showing how many digits of the estimate are correct.
    /// </summary>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    static void PrintAccuracyMeter(int correctDigits)
    {
        const int meterSize = 10;
        int filled = Math.Min(correctDigits, meterSize);

        ConsoleColor barColor = correctDigits switch
        {
            >= 5 => ConsoleColor.Green,
            >= 3 => ConsoleColor.Yellow,
            _ => ConsoleColor.Red
        };

        WriteColored("  Accuracy:     ", ConsoleColor.Gray);
        WriteColored(new string('█', filled), barColor);
        WriteColored(new string('░', meterSize - filled), ConsoleColor.DarkGray);
        WriteColored($"  {correctDigits} correct digit{(correctDigits == 1 ? "" : "s")}", ConsoleColor.White);
        Console.WriteLine();
    }

    /// <summary>
    /// Prints a short message rating how good the estimate is.
    /// </summary>
    /// <param name="pi">The estimated value of Pi.</param>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    static void PrintRating(decimal pi, int correctDigits)
    {
        (string message, ConsoleColor color) = (pi, correctDigits) switch
        {
            (0m, _) => ("EstimatePi returned 0 -- time to write some code!", ConsoleColor.Red),
            (_, 0) => ("Not quite... check your math.", ConsoleColor.Red),
            (_, 1) => ("You got the 3! Getting warmer.", ConsoleColor.Yellow),
            (_, 2) => ("3.1 -- good start. Try more iterations?", ConsoleColor.Yellow),
            (_, 3) => ("3.14 -- nice work!", ConsoleColor.Green),
            (_, 4) => ("Excellent! Archimedes would be proud.", ConsoleColor.Green),
            _ => ("Incredible! Are you sure you didn't cheat?", ConsoleColor.Cyan)
        };

        WriteColored("  Rating:       ", ConsoleColor.Gray);
        WriteColored(message, color);
        Console.WriteLine();
    }

    /// <summary>
    /// Counts how many leading digits of the estimate match the real digits of Pi.
    /// </summary>
    /// <param name="estimateText">The estimate formatted as text.</param>
    /// <returns>The number of correct digits before the first wrong one.</returns>
    static int CountCorrectDigits(string estimateText)
    {
        int count = 0;

        for (int i = 0; i < estimateText.Length && i < PiDigits.Length; i++)
        {
            if (estimateText[i] != PiDigits[i])
            {
                break;
            }

            if (char.IsDigit(estimateText[i]))
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Formats an amount of time using the most readable unit.
    /// </summary>
    /// <param name="elapsed">The amount of time to format.</param>
    /// <returns>The time as text, such as "250 ms" or "1.75 s".</returns>
    static string FormatTime(TimeSpan elapsed)
    {
        if (elapsed.TotalSeconds >= 1)
        {
            return $"{elapsed.TotalSeconds:0.00} s";
        }

        return $"{elapsed.TotalMilliseconds:0.###} ms";
    }

    /// <summary>
    /// Centers text within a field of the given width by padding it with spaces.
    /// </summary>
    /// <param name="text">The text to center.</param>
    /// <param name="width">The total width of the field.</param>
    /// <returns>The padded text.</returns>
    static string Center(string text, int width)
    {
        int left = (width - text.Length) / 2;
        return text.PadLeft(left + text.Length).PadRight(width);
    }

    /// <summary>
    /// Writes text in the given color, then restores the previous console color.
    /// </summary>
    /// <param name="text">The text to write.</param>
    /// <param name="color">The color to write the text in.</param>
    static void WriteColored(string text, ConsoleColor color)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = previous;
    }
}
