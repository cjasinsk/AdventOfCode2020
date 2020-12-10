using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public static async Result<string[]> ReadAllLines([NotNull] this string fileName)
        {
            if (fileName is null) { throw new ArgumentNullException(nameof(fileName)); }
            
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            return File.Exists(path)
                ? File.ReadAllLines(path)
                : await new Failure<string[]>(message: $"Unable to find file '{path}'", value: path);
        }

        
        //--------------------------------------------------
        /// <summary>
        /// Helper to easily create a successful value.
        /// </summary>
        [NotNull]
        public static Result<T> Success<T>([NotNull] this T value)
            => new Success<T>(value);
        
        
        //--------------------------------------------------
        /// <summary>
        /// Validate all rules separately, and collect all
        /// into a single result.
        /// </summary>
        [NotNull]
        public static Result<T> Validate<T>(
            [NotNull] this T value,
            [NotNull] Predicate<T> validator,
            [CanBeNull] string message = null)
            => validator(value)
                ? new Failure<T>(message: message, value: value)
                : value.Success();
        

        //--------------------------------------------------
        /// <summary>
        /// Validate all rules separately, and collect all
        /// into a single result.
        /// </summary>
        [NotNull]
        public static Result<T> Validate<T>(
            [NotNull] this T value,
            [NotNull] params (Predicate<T> Predicate, string Message)[] validators)
        {
            var errors = validators
                .Where(validator => validator.Predicate(value))
                .Select(validator => new Error(message: validator.Message))
                .ToArray();

            return errors.Length > 0
                ? new Failure<T>(nested: errors, value: value)
                : value.Success();
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Checks if a key exists and a value is not null
        /// in a dictionary.
        /// </summary>
        public static bool ContainsKeyAndValue<TKey, TValue>(
            [NotNull] this IDictionary<TKey, TValue> dictionary,
            [NotNull] TKey key)
            => dictionary.ContainsKey(key) && !(dictionary[key] is null);

    }
    
}
