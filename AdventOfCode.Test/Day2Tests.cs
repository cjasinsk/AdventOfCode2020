using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day2Tests
    {

        [Test]
        public static async Task Day2Test()
            => Assert.AreEqual(
                new Day("--- Day 2: Password Philosophy ---", "2", "1"),
                Day2.Run(await "..\\..\\..\\Input\\Day2.txt".ReadAllLines()));

    }
    
}
