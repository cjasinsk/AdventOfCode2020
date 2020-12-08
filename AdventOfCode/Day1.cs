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
            var parsed = await Day1.ParseInput(input);

            return new Day(
                "--- Day 1: Report Repair ---",
                await Validate.All("Day 1", "Parts error",
                    Day1.RunPartA(parsed),
                    Day1.RunPartB(parsed)));
        }


        //--------------------------------------------------
        /// <summary>
        /// Convert input strings to decimal
        /// </summary>
        [NotNull] 
        private static Result<decimal[]> ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.Select((x, i) => !decimal.TryParse(x, out var value)
                    ? new Error(i.ToString(), $"Unable to parse {x} into a number", x)
                    : value.Success())
                .Flatten("Day 1", "Unable to parse input into numbers.");
        }


        //--------------------------------------------------
        /// <summary>
        /// Find 2 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        [NotNull]
        private static Result<string> RunPartA([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 2) { return new Error("PartA", "Expecting at least 2 numbers of input.", input); }
            
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
            return new Error("PartA", "No two numbers add to 2020.", input);
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Find 3 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        [NotNull]
        private static Result<string> RunPartB([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 3) { return new Error("PartB", "Expecting at least 3 numbers of input.", input); }
            
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
            return new Error("PartB", "No three numbers add to 2020.", input);
        }
        
    }
    
}
