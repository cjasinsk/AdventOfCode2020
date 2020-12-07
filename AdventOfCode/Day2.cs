using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day2
    {

        //--------------------------------------------------
        [NotNull]
        public static Day Run([NotNull] IEnumerable<string> input)
        {
            var parsed = Day2.ParseInput(input);
            
            return new Day(
                title: "--- Day 2: Password Philosophy ---",
                partA: Day2.RunPartA(parsed).ToString(),
                partB: Day2.RunPartB(parsed).ToString());
        }


        //--------------------------------------------------
        /// <summary>
        /// Parse input strings into separate policies and
        /// passwords.
        /// </summary>
        [NotNull]
        private static (Policy Policy, string Password)[] ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            
            //ex: "1-3 a: abcde"
            return input.Select(x =>
            {
                // [0]:"1-3 a:", [1]:"abcde"
                var split = x.Split(new[] {": "}, StringSplitOptions.None);

                // [0]:"1-3", [1]:"a"
                var policyStr = split[0].Split(new[] {" "}, StringSplitOptions.None);

                // [0]:"1", [1]:"3"
                var minMax = policyStr[0].Split(new[] {"-"}, StringSplitOptions.None);

                return (
                    new Policy(
                        int.Parse(minMax[0]),
                        int.Parse(minMax[1]),
                        policyStr[1][0]),
                    split[1]);
            }).ToArray();
        }


        //--------------------------------------------------
        /// <summary>
        /// Find how many passwords are valid, given the policy
        /// expects a certain character to occur at least a minimum
        /// specified amount, and at most a maximum specified amount.
        /// </summary>
        /// <remarks>
        /// Example: "1-3 a: abcde" expects 'a' to occur 1 &lt;= a &gt;= 3
        /// </remarks>
        private static int RunPartA([NotNull] (Policy Policy, string Password)[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.Aggregate(0, (total, x) =>
            {
                var count = x.Password.Sum(c => (c == x.Policy.Letter) ? 1 : 0);
                return count >= x.Policy.Min && count <= x.Policy.Max
                    ? total + 1
                    : total;
            });
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Find how many passwords are valid, given the policy
        /// expects a certain character to occur only once in either
        /// the first or second location specified.
        /// </summary>
        /// <remarks>
        /// Example: "1-3 a: abcde" expects 'a' to occur in either the
        /// first or third position, but not both.
        /// </remarks>
        private static int RunPartB([NotNull] (Policy Policy, string Password)[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.Aggregate(0, (total, x) =>
                (x.Password.Length >= x.Policy.Max)
                && ((x.Password[x.Policy.Min - 1] == x.Policy.Letter)
                    ^ (x.Password[x.Policy.Max - 1] == x.Policy.Letter))
                    ? total + 1
                    : total);
        }
        
        
        //--------------------------------------------------
        private sealed class Policy
        {
            
            public Policy(int min, int max, char letter)
            {
                this.Letter = letter;
                this.Max = max;
                this.Min = min;
            }

            public readonly char Letter;
            public readonly int Max;
            public readonly int Min;
            
        }
        
    }
    
}
