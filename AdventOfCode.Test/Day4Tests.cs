using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day4Tests
    {

        [Test]
        public static void Day4Test()
            => Assert.AreEqual(
                new Day(
                    title: "--- Day 4: Passport Processing ---", 
                    partA: "N/A",
                    partB: "N/A"),
                Day3.Run("../../../Input/Day4.txt".ReadAllLines()));
        
    }
    
}
