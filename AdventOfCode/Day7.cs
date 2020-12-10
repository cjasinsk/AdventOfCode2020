using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Day7
    {
        
        //--------------------------------------------------
        public static Day Run([NotNull] IEnumerable<string> input)
        {
            var parsed = Day7.ParseInput(input);
            
            return new Day(
                "--- Day 7: Handy Haversacks ---",
                Day7.RunPartA(parsed, "shiny gold"),
                Day7.RunPartB(parsed, "shiny gold"));
        }
        
        
        //--------------------------------------------------
        [NotNull]
        private static Dictionary<string, Bag> ParseInput([NotNull] IEnumerable<string> input)
        {
            var bags = new Dictionary<string, Bag>();
            foreach (var line in input)
            {
                var cleaned = Regex.Replace(line, "(bags|bag|\\.)", "");
                var split = cleaned.Split(new [] { "contain" }, StringSplitOptions.None);

                var key = split[0].Trim();
                var children = split[1].Split(',').Select(x =>
                {
                    var parts = x.Trim().Split(new[] {' '}, 2);
                    var count = parts[0].Trim() != "no"
                        ? int.Parse(parts[0].Trim())
                        : 0;
                    var name = parts[1].Trim();

                    return (count, name);
                }).Where(x => x.count > 0);

                if (!bags.ContainsKey(key)) { bags.Add(key, new Bag()); }
                var bag = bags[key];

                foreach (var (count, name) in children)
                {
                    if (!bags.ContainsKey(name)) { bags.Add(name, new Bag()); }
                    var child = bags[name];
                    
                    if (!bag.Children.ContainsKey(name)) { bag.Children.Add(name, count); }
                    if (!child.Parents.Contains(key)) { child.Parents.Add(key); }
                }
            }

            return bags;
        }
        
        
        //--------------------------------------------------
        [NotNull]
        private static string RunPartA([NotNull] Dictionary<string, Bag> bags, [NotNull] string start)
        {
            if (!bags.ContainsKey(start)) { throw new InvalidOperationException("Bag does not exist"); }
            
            static void Recurse(Dictionary<string, Bag> bags, HashSet<string> ancestors, string name)
            {
                var bag = bags[name];
                foreach (var parent in bag.Parents)
                {
                    if (!ancestors.Contains(parent)) { ancestors.Add(parent); }
                    Recurse(bags, ancestors, parent);
                }
            }
            
            var ancestors = new HashSet<string>();
            Recurse(bags, ancestors, start);

            return ancestors.Count.ToString();
        }
        
        
        //--------------------------------------------------
        [NotNull]
        private static string RunPartB([NotNull] Dictionary<string, Bag> bags, [NotNull] string start)
        {
            if (!bags.ContainsKey(start)) { throw new InvalidOperationException("Bag does not exist"); }
            
            static int Recurse(Dictionary<string, Bag> bags, string name)
                => bags[name].Children
                    .Sum(child => child.Value + (child.Value * Recurse(bags, child.Key)));
            
            return Recurse(bags, start).ToString();
        }


        //--------------------------------------------------
        private sealed class Bag
        {
            [NotNull] public readonly Dictionary<string, int> Children = new Dictionary<string, int>();
            [NotNull] public readonly HashSet<string> Parents = new HashSet<string>();
        }
        
    }
    
}
