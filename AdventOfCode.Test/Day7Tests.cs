using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    public static class Day7Tests
    {
    
        [Test]
        public static async Task Day7Test()
        {
            Assert.AreEqual(
                new Day("--- Day 7: Handy Haversacks ---", "4", "32"),
                Day7.Run(await "..\\..\\..\\Input\\Day7.txt".ReadAllLines()));
            
            Assert.AreEqual(
                new Day("--- Day 7: Handy Haversacks ---", "0", "126"),
                Day7.Run(await "..\\..\\..\\Input\\Day7_2.txt".ReadAllLines()));
        }

    }
    
}
