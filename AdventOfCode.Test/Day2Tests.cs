using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day2Tests
    {

        [Test]
        public static void Day2Test()
        {
            var day2 = Day2.Run("../../../Input/Day2.txt".ReadAllLines());
            Assert.AreEqual("--- Day 2: Password Philosophy ---", day2.Title);
            Assert.AreEqual("2", day2.PartA);
            Assert.AreEqual("1", day2.PartB);
        } 
    }
    
}
