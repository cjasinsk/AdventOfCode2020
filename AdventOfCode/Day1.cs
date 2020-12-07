using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day1
    {

        //--------------------------------------------------
        [NotNull]
        public static Day Run([NotNull] IEnumerable<string> input)
        {
            var parsed = Day1.ParseInput(input);

            return new Day(
                title: "--- Day 1: Report Repair ---",
                partA: Day1.RunPartA(parsed).ToString(),
                partB: Day1.RunPartB(parsed).ToString());
        }


        //--------------------------------------------------
        /// <summary>
        /// Convert input strings to decimal
        /// </summary>
        [NotNull] 
        private static decimal[] ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            return input.Select(x =>
            {
                if (!decimal.TryParse(x, out var i)) { throw new InvalidOperationException($"Unable to parse {x} into a number."); }
                return i;
            }).ToArray();
        }


        //--------------------------------------------------
        /// <summary>
        /// Find 2 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        private static decimal RunPartA([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 2) { throw new ArgumentException("Expecting at least 2 numbers of input."); }
            
            foreach (var x in input)
            {
                foreach (var y in input)
                {
                    if (x + y == 2020)
                    {
                        return x * y;
                    }
                }
            }
            throw new InvalidOperationException("No two numbers add to 2020.");
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Find 3 numbers that add to 2020, then multiply
        /// them together.
        /// </summary>
        private static decimal RunPartB([NotNull] decimal[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (input.Length < 3) { throw new ArgumentException("Expecting at least 3 numbers of input."); }
            
            foreach (var x in input)
            {
                foreach (var y in input)
                {
                    foreach (var z in input)
                    {
                        if (x + y + z == 2020)
                        {
                            return x * y * z;
                        }
                    }
                }
            }
            throw new InvalidOperationException("No three numbers add to 2020.");
        }
        
    }
    
}
