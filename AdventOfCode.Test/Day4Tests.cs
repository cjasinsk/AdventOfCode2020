using System.Collections.Generic;
using System.Threading.Tasks;

using AdventOfCode.Common;

using NUnit.Framework;

namespace AdventOfCode.Test
{
    
    internal static class Day4Tests
    {

        [Test]
        public static async Task Day4Test()
            => Assert.AreEqual(
                new Day("--- Day 4: Passport Processing ---", "2", "2"),
                await Day4.Run(await "..\\..\\..\\Input\\Day4.txt".ReadAllLines()));

        [Test]
        public static void TestByr()
        {
            Assert.AreEqual(
                2002.Success(),
                Day4.ValidateBirthYear(new Dictionary<string, string> {["byr"] = "2002"}));
            
            Assert.AreEqual(
                new Failure<int>("byr", "Birth year must be at most 2002.", 2003),
                Day4.ValidateBirthYear(new Dictionary<string, string>{ ["byr"] = "2003" }));
        }
        
        
        [Test]
        public static void TestHgt()
        {
            Assert.AreEqual(
                60.Success(),
                Day4.ValidateHeight(new Dictionary<string, string> {["hgt"] = "60in"}));
            
            Assert.AreEqual(
                190.Success(),
                Day4.ValidateHeight(new Dictionary<string, string> {["hgt"] = "190cm"}));
            
            Assert.AreEqual(
                new Failure<int>("hgt", "Height must be at most 76in.", "190in"),
                Day4.ValidateHeight(new Dictionary<string, string>{ ["hgt"] = "190in" }));
            
            Assert.AreEqual(
                new Failure<int>("hgt", "Expecting height to be a number and followed by either 'cm' or 'in'.", "190"),
                Day4.ValidateHeight(new Dictionary<string, string>{ ["hgt"] = "190" }));
        }
        
        
        [Test]
        public static void TestHcl()
        {
            Assert.AreEqual(
                "#123abc".Success(),
                Day4.ValidateHairColor(new Dictionary<string, string> {["hcl"] = "#123abc"}));
            
            Assert.AreEqual(
                new Failure<string>("hcl", "Expecting hair color to be a '#' followed by 6 characters 0-9 or a-f.", "#123abz"),
                Day4.ValidateHairColor(new Dictionary<string, string>{ ["hcl"] = "#123abz" }));
            
            Assert.AreEqual(
                new Failure<string>("hcl", "Expecting hair color to be a '#' followed by 6 characters 0-9 or a-f.", "123abc"),
                Day4.ValidateHairColor(new Dictionary<string, string>{ ["hcl"] = "123abc" }));
        }
        
        
        [Test]
        public static void TestEcl()
        {
            Assert.AreEqual(
                "brn".Success(),
                Day4.ValidateEyeColor(new Dictionary<string, string> {["ecl"] = "brn"}));
            
            Assert.AreEqual(
                new Failure<string>("ecl", "Unknown eye color 'wat'.", "wat"),
                Day4.ValidateEyeColor(new Dictionary<string, string>{ ["ecl"] = "wat" }));
        }
        
        
        [Test]
        public static void TestPid()
        {
            Assert.AreEqual(
                "000000001".Success(),
                Day4.ValidatePassportId(new Dictionary<string, string> {["pid"] = "000000001"}));
            
            Assert.AreEqual(
                new Failure<string>("pid", "Expecting passport id a 9 digit long number.", "0123456789"),
                Day4.ValidatePassportId(new Dictionary<string, string>{ ["pid"] = "0123456789" }));
        }
        
        [Test]
        public static async Task TestPartBInvalid()
        {
            var parsed = await Day4.ParseInput(await "..\\..\\..\\Input\\Day4PartBInvalid.txt".ReadAllLines());
            Assert.AreEqual("0", await Day4.RunPartB(parsed));
        }
        
        
        [Test]
        public static async Task TestPartBValid()
        {
            var parsed = await Day4.ParseInput(await "..\\..\\..\\Input\\Day4PartBValid.txt".ReadAllLines());
            Assert.AreEqual("4", await Day4.RunPartB(parsed));
        }
        
    }
    
}
