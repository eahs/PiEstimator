using System;
using System.Diagnostics;
using System.Globalization;
using Spectre.Console;

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
    /// Entry point of the application. Prompts the user for the number of
    /// iterations, estimates Pi, and prints a report comparing the estimate
    /// to the true value of Pi.
    /// </summary>
    /// <param name="args">
    /// Command-line arguments. If the first argument is a positive whole number it is
    /// used as the number of iterations; otherwise the user is prompted for one.
    /// </param>
    static void Main(string[] args)
    {
        PrintBanner();

        if (args.Length == 0 || !long.TryParse(args[0], out long n) || n <= 0)
        {
            n = AnsiConsole.Prompt(
            new TextPrompt<long>("[yellow]Enter number of iterations (n):[/]")
                .ValidationErrorMessage("[red]Please enter a whole number.[/]")
                .Validate(value => value > 0
                    ? ValidationResult.Success()
                    : ValidationResult.Error("[red]The number must be greater than 0.[/]")));
        }
        else
        {
            AnsiConsole.MarkupLine($"[yellow]Number of iterations (n):[/] {n:N0}");
        }

        decimal pi = 0m;
        Stopwatch timer = new Stopwatch();

        AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .Start("Throwing random numbers at the problem...", _ =>
            {
                timer.Start();
                pi = EstimatePi(n);
                timer.Stop();
            });

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
    /// Prints the title banner at the top of the program.
    /// </summary>
    static void PrintBanner()
    {
        AnsiConsole.Write(new FigletText("Pi Estimator").Centered().Color(Color.Cyan1));
        AnsiConsole.Write(new Rule($"[darkcyan]{PiDigits}...[/]").RuleStyle("cyan"));
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Prints the results table, the digit-by-digit comparison, an accuracy meter,
    /// and a rating message for the estimate.
    /// </summary>
    /// <param name="n">The number of iterations used for the estimate.</param>
    /// <param name="pi">The estimated value of Pi.</param>
    /// <param name="elapsed">How long the estimate took to calculate.</param>
    static void PrintResults(long n, decimal pi, TimeSpan elapsed)
    {
        decimal diff = Decimal.Abs(pi - ActualPi);
        decimal percentError = diff / ActualPi * 100m;
        string estimateText = Math.Round(pi, 15).ToString(CultureInfo.InvariantCulture);
        int correctDigits = CountCorrectDigits(estimateText);

        Table table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Cyan1)
            .Title("[bold cyan]Results[/]")
            .AddColumn("[grey]Measure[/]")
            .AddColumn("[grey]Value[/]")
            .AddRow("Iterations", $"[white]{n:N0}[/]")
            .AddRow("Estimate", $"[yellow]{estimateText}[/]")
            .AddRow("Actual π", $"[green]{Math.Round(ActualPi, 15).ToString(CultureInfo.InvariantCulture)}[/]")
            .AddRow("Difference", $"[magenta]{Math.Round(diff, 15).ToString(CultureInfo.InvariantCulture)}[/]")
            .AddRow("Error", $"[magenta]{percentError.ToString("0.######", CultureInfo.InvariantCulture)} %[/]")
            .AddRow("Time", $"[white]{FormatTime(elapsed)}[/]");

        AnsiConsole.WriteLine();
        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();

        Grid grid = new Grid()
            .AddColumn(new GridColumn().PadRight(2))
            .AddColumn()
            .AddRow("[grey]Your digits:[/]", DigitComparisonMarkup(estimateText, correctDigits))
            .AddRow("[grey]Real digits:[/]", $"[darkgreen]{PiDigits.Substring(0, Math.Max(estimateText.Length, 2))}[/]")
            .AddEmptyRow()
            .AddRow("[grey]Accuracy:[/]", AccuracyMeterMarkup(correctDigits))
            .AddRow("[grey]Rating:[/]", RatingMarkup(pi, correctDigits));

        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Builds markup showing the estimate with correct digits in green, the first
    /// wrong digit in red, and the remaining digits dimmed.
    /// </summary>
    /// <param name="estimateText">The estimate formatted as text.</param>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    /// <returns>Spectre.Console markup for the colored digits.</returns>
    static string DigitComparisonMarkup(string estimateText, int correctDigits)
    {
        // Find where the correct part of the text ends (correct digits plus any decimal point).
        int correctLength = 0;
        int digitsSeen = 0;

        while (correctLength < estimateText.Length && digitsSeen < correctDigits)
        {
            if (char.IsDigit(estimateText[correctLength]))
            {
                digitsSeen++;
            }

            correctLength++;
        }

        if (correctLength < estimateText.Length && estimateText[correctLength] == '.')
        {
            correctLength++;
        }

        string correct = estimateText.Substring(0, correctLength);
        string wrong = correctLength < estimateText.Length ? estimateText.Substring(correctLength, 1) : "";
        string rest = correctLength + 1 < estimateText.Length ? estimateText.Substring(correctLength + 1) : "";

        return $"[green]{correct}[/][red]{wrong}[/][grey]{rest}[/]";
    }

    /// <summary>
    /// Builds markup for a bar showing how many digits of the estimate are correct.
    /// </summary>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    /// <returns>Spectre.Console markup for the accuracy bar.</returns>
    static string AccuracyMeterMarkup(int correctDigits)
    {
        const int meterSize = 10;
        int filled = Math.Min(correctDigits, meterSize);

        string barColor = correctDigits switch
        {
            >= 5 => "green",
            >= 3 => "yellow",
            _ => "red"
        };

        return $"[{barColor}]{new string('█', filled)}[/][grey]{new string('░', meterSize - filled)}[/]" +
               $"  [white]{correctDigits} correct digit{(correctDigits == 1 ? "" : "s")}[/]";
    }

    /// <summary>
    /// Builds markup for a short message rating how good the estimate is.
    /// </summary>
    /// <param name="pi">The estimated value of Pi.</param>
    /// <param name="correctDigits">How many leading digits of the estimate are correct.</param>
    /// <returns>Spectre.Console markup for the rating message.</returns>
    static string RatingMarkup(decimal pi, int correctDigits)
    {
        return (pi, correctDigits) switch
        {
            (0m, _) => "[red]EstimatePi returned 0 -- time to write some code![/]",
            (_, 0) => "[red]Not quite... check your math.[/]",
            (_, 1) => "[yellow]You got the 3! Getting warmer.[/]",
            (_, 2) => "[yellow]3.1 -- good start. Try more iterations?[/]",
            (_, 3) => "[green]3.14 -- nice work![/]",
            (_, 4) => "[green]Excellent! Archimedes would be proud.[/]",
            _ => "[cyan]Incredible! Are you sure you didn't cheat?[/]"
        };
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
}
