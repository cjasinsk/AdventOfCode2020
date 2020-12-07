using System;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    internal static class Program
    {
        
        public static void Main([NotNull] string[] args)
        {
            if (args is null) { throw new ArgumentNullException(nameof(args)); }
            if (args.Length != 1) { throw new ArgumentException("Expecting a parameter for the day; 1-25.", nameof(args)); }

            if (!int.TryParse(args[0], out var day)) { throw new ArgumentException("Expecting the first parameter to be a number.", nameof(args)); }
            if (day < 1 || day > 25) { throw new ArgumentException("The day must be between 1 and 25.", nameof(args)); }

            var result = day switch
            {
                1 => Day1.Run("Input/Day1.txt".ReadAllLines()),
                _ => throw new InvalidOperationException($"Currently there is no solution for Day {day}.")
            };

            Console.WriteLine(result.Title);
            Console.WriteLine($"PartA:\t{result.PartA}");
            Console.WriteLine($"PartB:\t{result.PartB}");
        }
        
    }
    
}
