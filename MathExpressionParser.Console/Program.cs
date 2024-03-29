namespace MathExpressionParser.Console;

using System;
using MathExpressionParser;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Please, type in an arithmetic expression:");

        try
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("The input string is empty!");
                return;
            }

            decimal result = MathProcessor.Calculate(input);
            Console.WriteLine($"Expression result: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return;
        }
    }
}
