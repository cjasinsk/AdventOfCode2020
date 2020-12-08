using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day3Tests
    {

        [Test]
        public static async Task Day3Test()
            => Assert.AreEqual(
                new Day("--- Day 3: Toboggan Trajectory ---", "7", "336"),
                Day3.Run(await "..\\..\\..\\Input\\Day3.txt".ReadAllLines()));

    }
    
}
