using System;
using System.IO;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{
    
    /// <summary>
    /// Helper extensions
    /// </summary>
    public static class Extensions
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Reads all the lines in a file
        /// </summary>
        [NotNull]
        public static string[] ReadAllLines([NotNull] this string fileName)
        {
            if (fileName is null) { throw new ArgumentNullException(nameof(fileName)); }
            
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            return File.Exists(path)
                ? File.ReadAllLines(path)
                : new string[0];
        }
        
    }
    
}
