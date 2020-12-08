using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AdventOfCode.Common;

using JetBrains.Annotations;

#pragma warning disable 1998

namespace AdventOfCode
{
    
    public static class Day4
    {

        //--------------------------------------------------
        [NotNull]
        public static async Result<Day> Run([NotNull] string[] input)
        {
            var parsed = await Day4.ParseInput(input);
            
            return new Day(
                "--- Day 4: Passport Processing ---",
                await Validate.All("Day 4", "Parts error",
                    Day4.RunPartA(parsed),
                    Day4.RunPartB(parsed)));
        }


        //--------------------------------------------------
        [NotNull]
        public static async Result<IDictionary<string, string>[]> ParseInput([NotNull] string[] input)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            var rawPassports = new List<IDictionary<string, string>>();
            for (var i = 0; i < input.Length; i += 1)
            {
                var line = input[i];
                var pairs = line.Split(' ');
                
                if (i == 0 || string.IsNullOrEmpty(line)) { rawPassports.Add(new Dictionary<string, string>()); }
                var rawPassport = rawPassports[rawPassports.Count - 1];

                if (!string.IsNullOrEmpty(line))
                {
                    foreach (var pair in pairs)
                    {
                        var kvp = pair.Split(':');
                        if (kvp.Length != 2)
                        {
                            // throw is easy to short-circuit in the async Result
                            throw new Error("Day 4", $"Unable to parse '{pair}' into key/value pair on line {i}", input);
                        }

                        rawPassport[kvp[0]] = kvp[1];
                    }
                }
            }

            return rawPassports.ToArray();
        }


        //--------------------------------------------------
        [NotNull]
        public static Result<string> RunPartA([NotNull] IDictionary<string, string>[] rawPassports)
        {
            if (rawPassports is null) { throw new ArgumentNullException(nameof(rawPassports)); }

            var counter = 0;
            Console.WriteLine(rawPassports.Length);
            return rawPassports.Aggregate(0, (total, rawPassport) =>
            {
                var result = rawPassport.Validate($"PartA ({counter++})",
                    (x => !x.ContainsKeyAndValue("byr"), "Expecting 'byr' (Birth Year)"),
                    (x => !x.ContainsKeyAndValue("iyr"), "Expecting 'iyr' (Issue Year)"),
                    (x => !x.ContainsKeyAndValue("eyr"), "Expecting 'eyr' (Expiration Year)"),
                    (x => !x.ContainsKeyAndValue("hgt"), "Expecting 'hgt' (Height)"),
                    (x => !x.ContainsKeyAndValue("hcl"), "Expecting 'hcl' (Hair Color)"),
                    (x => !x.ContainsKeyAndValue("ecl"), "Expecting 'ecl' (Eye Color)"),
                    (x => !x.ContainsKeyAndValue("pid"), "Expecting 'pid' (Passport ID)"));

                return result is Success<IDictionary<string, string>>
                    ? total + 1
                    : total;
            }).ToString();
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<string> RunPartB([NotNull] IDictionary<string, string>[] rawPassports)
        {
            if (rawPassports is null) { throw new ArgumentNullException(nameof(rawPassports)); }

            var counter = 0;
            Console.WriteLine(rawPassports.Length);
            return rawPassports.Aggregate(0, (total, rawPassport) =>
            {
                var result = Validate.All(
                    $"PartB ({counter++})",
                    "Validate passport",
                    Day4.ValidateBirthYear(rawPassport),
                    Day4.ValidateIssueYear(rawPassport),
                    Day4.ValidateExpirationYear(rawPassport),
                    Day4.ValidateHeight(rawPassport),
                    Day4.ValidateHairColor(rawPassport),
                    Day4.ValidateEyeColor(rawPassport),
                    Day4.ValidatePassportId(rawPassport));

                return result is Success<(int, int, int, int, string, string, string)>
                    ? total + 1
                    : total;
            }).ToString();
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<int> ValidateBirthYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            
            // sequential
            if (!passport.ContainsKeyAndValue("byr")) { return new Error("byr", "Expecting 'byr' (Birth Year) to exist.", passport); }
            if (passport["byr"].Length != 4 || !int.TryParse(passport["byr"], out var byr)) { return new Error("byr", "Birth year must be a 4 digit year.", passport["byr"]); }
                
            // parallel
            return byr.Validate("byr",
                (x => x < 1920, "Birth year must be at least 1920."),
                (x => x > 2002, "Birth year must be at most 2002."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<int> ValidateIssueYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            // sequential
            if (!passport.ContainsKeyAndValue("iyr")) { return new Error("iyr", "Expecting 'iyr' (Issue Year) to exist.", passport); }
            if (passport["iyr"].Length != 4 || !int.TryParse(passport["iyr"], out var iyr)) { return new Error("iyr", "Issue year must be a 4 digit year.", passport["iyr"]); }
                
            // parallel
            return iyr.Validate("iyr",
                (x => x < 2010, "Issue year must be at least 2010."),
                (x => x > 2020, "Issue year must be at most 2020."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<int> ValidateExpirationYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            // sequential
            if (!passport.ContainsKeyAndValue("eyr")) { return new Error("eyr", "Expecting 'eyr' (Expiration Year) to exist.", passport); }
            if (passport["eyr"].Length != 4 || !int.TryParse(passport["eyr"], out var eyr)) { return new Error("eyr", "Expiration year must be a 4 digit year.", passport["eyr"]); }
                
            // parallel
            return eyr.Validate("eyr",
                (x => x < 2020, "Expiration year must be at least 2020."),
                (x => x > 2030, "Expiration year must be at most 2030."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<int> ValidateHeight([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            // sequential
            if (!passport.ContainsKeyAndValue("hgt")) { return new Error("hgt", "Expecting 'hgt' (Height) to exist.", passport); }
            
            var matches = Regex.Match(passport["hgt"], "^([0-9]*)(in|cm)$", RegexOptions.IgnoreCase);
            if (matches.Groups.Count != 3) { return new Error("hgt", "Expecting height to be a number and followed by either 'cm' or 'in'.", passport["hgt"]); }

            var hgt = int.Parse(matches.Groups[1].Value);
            var unit = matches.Groups[2].Value;

            // parallel
            return unit switch
            {
                "cm" => hgt.Validate("hgt", 
                    (x => x < 150, "Height must be at least 150cm."),
                    (x => x > 193, "Height must be at most 193cm.")),
                "in" => hgt.Validate("hgt", 
                    (x => x < 59, "Height must be at least 59in."),
                    (x => x > 76, "Height must be at most 76in.")),
                _ => new Error("hgt", $"Unknown unit '{unit}'.", unit)
            };
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<string> ValidateHairColor([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            
            if (!passport.ContainsKeyAndValue("hcl")) { return new Error("hcl", "Expecting 'hcl' (Hair Color) to exist.", passport); }
            
            var matches = Regex.Match(passport["hcl"], "^(#[0-9a-fA-F]{6})$", RegexOptions.IgnoreCase);
            if (matches.Groups.Count != 2) { return new Error("hcl", "Expecting hair color to be a '#' followed by 6 characters 0-9 or a-f.", passport["hcl"]); }

            return matches.Groups[1].Value;
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<string> ValidateEyeColor([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            if (!passport.ContainsKeyAndValue("ecl")) { return new Error("ecl", "Expecting 'ecl' (Eye Color) to exist.", passport); }

            var ecl = passport["ecl"];
            return ecl switch
            {
                "amb" => ecl,
                "blu" => ecl,
                "brn" => ecl,
                "gry" => ecl,
                "grn" => ecl,
                "hzl" => ecl,
                "oth" => ecl,
                _ => new Error("ecl", $"Unknown eye color '{ecl}'.", ecl)
            };
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<string> ValidatePassportId([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            if (!passport.ContainsKeyAndValue("pid")) { return new Error("pid", "Expecting 'pid' (Passport ID) to exist.", passport); }

            var matches = Regex.Match(passport["pid"], "^([0-9]{9})$", RegexOptions.IgnoreCase);
            if (matches.Groups.Count != 2) { return new Error("pid", "Expecting passport id a 9 digit long number.", passport["pid"]); }

            return matches.Groups[1].Value;
        }
    }
    
}
