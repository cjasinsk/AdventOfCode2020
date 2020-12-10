using System;

using AdventOfCode.Common;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    internal static class Program
    {
     
        //--------------------------------------------------
        public static void Main([NotNull] string[] args)
            => Console.WriteLine(Result.From("AdventOfCode", async () => await Program.Run(args)));


        //--------------------------------------------------
        private static async Result<Day> Run([NotNull] string[] args)
        {
            if (args is null) { throw new ArgumentNullException(nameof(args)); }

            var day = await Result.From("Args", async () =>
            {
                var d = 0;
                await args.Validate(x => x.Length != 1, "Expecting at least 1 argument for the day; 1-25.");
                await args.Validate(x => !int.TryParse(x[0], out d) || (d < 1 || d > 25), "Expecting the first argument to be a number between 1 and 25.");
                return d;
            });

            return await Result.From($"Day {day}", async () =>
            {
                var input = await Result.From("ReadAllLines", async () => await
                    $"Input\\Day{day}.txt".ReadAllLines());
                
                return await (day switch
                {
                    1 => Day1.Run(input),
                    2 => Day2.Run(input),
                    3 => Day3.Run(input),
                    4 => Day4.Run(input),
                    5 => Day5.Run(input),
                    _ => await new Failure<Day>(message: $"Currently there is no solution for Day {day}.", value: day)
                });
            });
        }
    }
    
}
