using System;
using System.IO;

using JetBrains.Annotations;

namespace AdventOfCode
{
    
    public static class Extensions
    {
        
        [NotNull]
        public static string[] ReadAllLines([NotNull] this string fileName)
        {
            if (fileName is null) { throw new ArgumentNullException(nameof(fileName)); }
            
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            Console.WriteLine(path);
            return File.Exists(path)
                ? File.ReadAllLines(path)
                : new string[0];
        }
        
    }
    
}
