using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day2Tests
    {

        [Test]
        public static void Day2Test()
            => Assert.AreEqual(
                new Day(
                    title: "--- Day 2: Password Philosophy ---",
                    partA: "2",
                    partB: "1"),
                Day2.Run("../../../Input/Day2.txt".ReadAllLines()));

    }
    
}
