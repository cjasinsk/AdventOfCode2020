using System;
using System.Collections.Generic;
using System.Linq;

using AdventOfCode.Common;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day1
    {

        //--------------------------------------------------
        public static async Result<Day> Run([NotNull] IEnumerable<string> input)
        {
            var parsed = await ("ParseInput", Day1.ParseInput(input));

            return new Day(
                "--- Day 1: Report Repair ---",
                await Result.From(
                    ("PartA", Day1.RunPartA(parsed)),
                    ("PartB", Day1.RunPartB(parsed))));
        }


        //--------------------------------------------------
        /// <summary>
        /// Convert input strings to decimal
        /// </summary>
        [NotNull] 
        private static async Result<decimal[]> ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return (await input.Select<string, Result<decimal>>(async (x, i) =>
                !decimal.TryParse(x, out var d)
                    ? await new Failure<decimal>($"{i}", $"Unable to parse {x} into a number")
                    : d
            )).ToArray();
        }


        //--------------------------------------------------
        /// <summary>
        /// Find 2 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        [NotNull]
        private static async Result<string> RunPartA([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 2) { await new Failure<string>(message: "Expecting at least 2 numbers of input.", value: input); }
            
            foreach (var x in input)
            {
                foreach (var y in input)
                {
                    if (x + y == 2020)
                    {
                        return (x * y).ToString();
                    }
                }
            }
            return await new Failure<string>(message: "No two numbers add to 2020.", value: input);
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Find 3 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        [NotNull]
        private static async Result<string> RunPartB([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 3) { await new Failure<string>(message: "Expecting at least 3 numbers of input.", value: input); }
            
            foreach (var x in input)
            {
                foreach (var y in input)
                {
                    foreach (var z in input)
                    {
                        if (x + y + z == 2020)
                        {
                            return (x * y * z).ToString();
                        }
                    }
                }
            }
            return await new Failure<string>(message: "No three numbers add to 2020.", value: input);
        }
        
    }
    
}
