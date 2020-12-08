using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day3Tests
    {

        [Test]
        public static void Day3Test()
            => Assert.AreEqual(
                new Day(
                    title: "--- Day 3: Toboggan Trajectory ---", 
                    partA: "7",
                    partB: "336"),
                Day3.Run("../../../Input/Day3.txt".ReadAllLines()));

    }
    
}
