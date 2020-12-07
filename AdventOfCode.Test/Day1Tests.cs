using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day1Tests
    {
        
        [Test]
        public static void Day1Test()
        {
            var day1 = Day1.Run("../../../Input/Day1.txt".ReadAllLines());
            Assert.AreEqual("--- Day 1: Report Repair ---", day1.Title);
            Assert.AreEqual("514579", day1.PartA);
            Assert.AreEqual("241861950", day1.PartB);
        }

    }
    
}
