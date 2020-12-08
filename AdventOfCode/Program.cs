using System;

using AdventOfCode.Common;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    internal static class Program
    {
     
        //--------------------------------------------------
        public static void Main([NotNull] string[] args)
        {
            var output = Program.Run(args) switch
            {
                Success<Day> success => success.ToString(),
                Failure<Day> failure => new Error("AdventOfCode", "An error occurred")[failure.Error].ToString(),
                _ => throw new InvalidOperationException()
            };
            
            Console.WriteLine(output);
        }


        //--------------------------------------------------
        private static async Result<Day> Run([NotNull] string[] args)
        {
            if (args is null) { throw new ArgumentNullException(nameof(args)); }

            await args.Validate("Args", (x => x.Length != 1, "Expecting at least 1 argument for the day; 1-25."));
            await args.Validate("Args", (x => !int.TryParse(x[0], out var d) || (d < 1 || d > 25), "Expecting the first argument to be a number between 1 and 25."));

            var day = int.Parse(args[0]);
            return await (day switch
            {
                1 => Day1.Run(await $"Input\\Day{day}.txt".ReadAllLines()),
                2 => Day2.Run(await $"Input\\Day{day}.txt".ReadAllLines()),
                3 => Day3.Run(await $"Input\\Day{day}.txt".ReadAllLines()),
                4 => Day4.Run(await $"Input\\Day{day}.txt".ReadAllLines()),
                _ => throw new Error($"Day {day}", $"Currently there is no solution for Day {day}.")
            });
        }
    }
    
}
