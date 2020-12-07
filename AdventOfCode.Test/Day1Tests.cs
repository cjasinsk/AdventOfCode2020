using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day1Tests
    {

        [Test]
        public static void Day1Test()
            => Assert.AreEqual(
                new Day(
                    title: "--- Day 1: Report Repair ---",
                    partA: "514579",
                    partB: "241861950"),
                Day1.Run("../../../Input/Day1.txt".ReadAllLines()));

    }
    
}
