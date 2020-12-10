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
            var parsed = await ("ParseInput", Day4.ParseInput(input));

            return new Day(
                "--- Day 4: Passport Processing ---",
                await Result.From(
                    ("PartA", Day4.RunPartA(parsed)),
                    ("PartB", Day4.RunPartB(parsed))));
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
                            await new Failure<IDictionary<string, string>>(message: $"Unable to parse '{pair}' into key/value pair on line {i}", value: input);
                            continue;
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

            return rawPassports.Aggregate(0, (total, rawPassport) =>
            {
                var result = rawPassport.Validate(
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

            return rawPassports.Aggregate(0, (total, rawPassport) 
                    => Passport.From(rawPassport) is Success<Passport> 
                        ? total + 1 
                        : total)
                .ToString();
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<int> ValidateBirthYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            var byr = 0;
            await passport.Validate(x => !x.ContainsKeyAndValue("byr"), "Expecting 'byr' (Birth Year) to exist.");
            await passport.Validate(x => x["byr"].Length != 4 || !int.TryParse(x["byr"], out byr), "Birth year must be a 4 digit year.");

            return await byr.Validate(
                (x => x < 1920, "Birth year must be at least 1920."),
                (x => x > 2002, "Birth year must be at most 2002."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<int> ValidateIssueYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            var iyr = 0;
            await passport.Validate(x => !x.ContainsKeyAndValue("iyr"), "Expecting 'iyr' (Issue Year) to exist.");
            await passport.Validate(x => x["iyr"].Length != 4 || !int.TryParse(x["iyr"], out iyr), "Issue year must be a 4 digit year.");
                
            return await iyr.Validate(
                (x => x < 2010, "Issue year must be at least 2010."),
                (x => x > 2020, "Issue year must be at most 2020."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<int> ValidateExpirationYear([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }

            var eyr = 0;
            await passport.Validate(x => !x.ContainsKeyAndValue("eyr"), "Expecting 'eyr' (Expiration Year) to exist.");
            await passport.Validate(x => x["eyr"].Length != 4 || !int.TryParse(x["eyr"], out eyr), "Expiration year must be a 4 digit year.");

            return await eyr.Validate(
                (x => x < 2020, "Expiration year must be at least 2020."),
                (x => x > 2030, "Expiration year must be at most 2030."));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<int> ValidateHeight([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            await passport.Validate(x => !x.ContainsKeyAndValue("hgt"), "Expecting 'hgt' (Height) to exist.");
            
            var matches = Regex.Match(passport["hgt"], "^([0-9]*)(in|cm)$", RegexOptions.IgnoreCase);
            await matches.Validate(x => x.Groups.Count != 3, "Expecting height to be a number and followed by either 'cm' or 'in'.");

            var hgt = int.Parse(matches.Groups[1].Value);
            var unit = matches.Groups[2].Value;

            return unit switch
            {
                "cm" => await hgt.Validate( 
                    (x => x < 150, "Height must be at least 150cm."),
                    (x => x > 193, "Height must be at most 193cm.")),
                "in" => await hgt.Validate( 
                    (x => x < 59, "Height must be at least 59in."),
                    (x => x > 76, "Height must be at most 76in.")),
                _ => await new Failure<int>(message: $"Unknown unit '{unit}'.", value: unit)
            };
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<string> ValidateHairColor([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            await passport.Validate(x => !x.ContainsKeyAndValue("hcl"), "Expecting 'hcl' (Hair Color) to exist.");
            
            var matches = Regex.Match(passport["hcl"], "^(#[0-9a-fA-F]{6})$", RegexOptions.IgnoreCase);
            await matches.Validate(x => x.Groups.Count != 2, "Expecting hair color to be a '#' followed by 6 characters 0-9 or a-f.");

            return matches.Groups[1].Value;
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<string> ValidateEyeColor([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            await passport.Validate(x => !x.ContainsKeyAndValue("ecl"), "Expecting 'ecl' (Eye Color) to exist.");

            var ecl = passport["ecl"];
            return await ecl.Validate(
                x => !(new[] {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"}.Contains(x)),
                $"Unknown eye color '{ecl}'.");
        }
        
        
        //--------------------------------------------------
        [NotNull]
        public static async Result<string> ValidatePassportId([NotNull] IDictionary<string, string> passport)
        {
            if (passport is null) { throw new ArgumentNullException(nameof(passport)); }
            await passport.Validate(x => !x.ContainsKeyAndValue("pid"), "Expecting 'pid' (Passport ID) to exist.");

            var matches = Regex.Match(passport["pid"], "^([0-9]{9})$", RegexOptions.IgnoreCase);
            await matches.Validate(m => m.Groups.Count != 2, "Expecting passport id a 9 digit long number.");

            return matches.Groups[1].Value;
        }



        private sealed class Passport
        {
            public static async Result<Passport> From([NotNull]IDictionary<string, string> rawPassport)
                => new Passport(
                    await Result.From(
                        ("byr", Day4.ValidateBirthYear(rawPassport)),
                        ("iyr", Day4.ValidateIssueYear(rawPassport)),
                        ("eyr", Day4.ValidateExpirationYear(rawPassport)),
                        ("hgt", Day4.ValidateHeight(rawPassport)),
                        ("hcl", Day4.ValidateHairColor(rawPassport)),
                        ("ecl", Day4.ValidateEyeColor(rawPassport)),
                        ("pid", Day4.ValidatePassportId(rawPassport))));

            private Passport((int byr, int iyr, int eyr, int hgt, string hcl, string ecl, string pid) _)
            { }
            
        }
        
    }
    
}
