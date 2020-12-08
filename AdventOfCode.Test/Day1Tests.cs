using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day1Tests
    {

        [Test]
        public static async Task Day1Test()
            => Assert.AreEqual(
                new Day("--- Day 1: Report Repair ---", "514579", "241861950"),
                await Day1.Run(await "..\\..\\..\\Input\\Day1.txt".ReadAllLines()));

    }
    
}
