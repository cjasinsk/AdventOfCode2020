using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day6
    {
        
        //--------------------------------------------------
        public static Day Run([NotNull] IEnumerable<string> input)
        {
            var parsed = Day6.ParseInput(input);
            
            return new Day(
                "--- Day 6: Custom Customs ---",
                Day6.RunPart(parsed, false),
                Day6.RunPart(parsed, true));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static string[][] ParseInput([NotNull] IEnumerable<string> input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.Aggregate(new List<List<string>> {new List<string>()}, (total, x) =>
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        total.Add(new List<string>());
                        return total;
                    }

                    total[total.Count - 1].Add(x);
                    return total;
                })
                .Select(x => x.ToArray())
                .ToArray();
        }
        
        
        //--------------------------------------------------
        [NotNull]
        private static string RunPart([NotNull] string[][] groups, bool allAnswered)
        {
            // foreach group of lines
            return groups.Aggregate(0, (total, group) =>
            {
                // foreach line
                var answered = group.Aggregate(new int[26], (set, line) =>
                {
                    // foreach character, convert to number
                    foreach (var i in line.Select(c => c - 97))
                    {
                        set[i] += 1;
                    }

                    return set;
                });

                // count how many questions answered by all in group, and add to the total
                return total + Enumerable.Range(0, 26)
                    .Aggregate(0, (c, i) 
                        => c + (allAnswered 
                            ? answered[i] == group.Length ? 1 : 0
                            : answered[i] > 0 ? 1 : 0));
            }).ToString();
        }
        
    }
    
}
