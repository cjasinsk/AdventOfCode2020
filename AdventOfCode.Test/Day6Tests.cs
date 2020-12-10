using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    public static class Day6Tests
    {
        [Test]
        public static async Task Day6Test()
            => Assert.AreEqual(
                new Day("--- Day 6: Custom Customs ---", "11", "6"),
                Day6.Run(await "..\\..\\..\\Input\\Day6.txt".ReadAllLines()));
    
    }
    
}
