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
        public static Result<string[]> ReadAllLines([NotNull] this string fileName)
        {
            if (fileName is null) { throw new ArgumentNullException(nameof(fileName)); }
            
            var path = Path.Combine(Environment.CurrentDirectory, fileName);
            return File.Exists(path)
                ? File.ReadAllLines(path).Success()
                : new Error("ReadAllLines", $"Unable to find file '{path}'", path);
        }


        //--------------------------------------------------
        /// <summary>
        /// Flatter a collection of results, into a single
        /// result that contains a collection of values.
        /// </summary>
        [NotNull]
        public static Result<T[]> Flatten<T>([NotNull] this IEnumerable<Result<T>> results, [NotNull] string id, [NotNull] string message)
        {
            if (results is null) { throw new ArgumentNullException(nameof(results)); }

            return results.Aggregate(default(Result<T[]>), (total, result)
                => total switch 
                {
                    Success<T[]> success => result switch 
                    {
                        Success<T> s => success.Add(s),
                        Failure<T> f => new Error(id, message, results)[f.Error],
                        _ => throw new InvalidOperationException()
                    },
                    Failure<T[]> failure => result switch
                    {
                        Failure<T> f => failure[f.Error],
                        _ => failure
                    },
                    _ => result switch 
                    {
                        Success<T> s => new Success<T[]>(new [] { s.Value }),
                        Failure<T> f => new Error(id, message, results)[f.Error],
                        _ => throw new InvalidOperationException()
                    }
                });
        }


        //--------------------------------------------------
        /// <summary>
        /// Appends a value to a successful array of values.
        /// </summary>
        [NotNull]
        public static Success<T[]> Add<T>([NotNull] this Success<T[]> success, [NotNull] Success<T> s)
            => new Success<T[]>(success.Value.Concat(new[] {s.Value}).ToArray());
        
        
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
            [NotNull] string id,
            [NotNull] params (Predicate<T> Predicate, string Message)[] validators)
        {
            var messages = validators
                .Where(validator => validator.Predicate(value))
                .Select(validator => validator.Message)
                .ToArray();

            return messages.Length > 0
                ? new Error(id, messages, value)
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
