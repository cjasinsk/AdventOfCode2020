using System;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{

    /// <summary>
    /// Stores information regarding an error
    /// </summary>
    public sealed class Error : Exception
    {
        
        /**************************************************
         * Public
         ***************************************************/
        
        //--------------------------------------------------
        /// <summary>
        /// Construct a new Error.
        /// </summary>
        /// <param name="id">The id of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="value">The value that caused the error</param>
        /// <param name="nested">Any additional nested errors</param>
        public Error(
            [CanBeNull] string id = null,
            [CanBeNull] string message = null,
            [CanBeNull] object value = null,
            [CanBeNull] Error[] nested = null)
        {
            this.Id = id;
            this._message = message;
            this.Nested = (nested ?? new Error[0])
                .Where(x => !(x is null))
                .ToArray();
            this.Value = value;
        }

        
        //--------------------------------------------------
        public static Error operator |([CanBeNull] Error e1, [CanBeNull] Error e2)
            => !(e1 is null)
                ? !(e2 is null)
                    ? new Error(nested: new [] { e1, e2 })
                    : e1
                : !(e2 is null)
                    ? e2
                    : null;
        
        
        //--------------------------------------------------
        public static bool operator true([CanBeNull] Error error)
            => !(error is null);
        
        
        //--------------------------------------------------
        public static bool operator false([CanBeNull] Error error)
            => error is null;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override bool Equals(object obj)
            => !object.ReferenceEquals(obj, null)
                && (object.ReferenceEquals(this, obj)
                    || (obj is Error error && this.ToString().Equals(error.ToString())));
        

        //--------------------------------------------------
        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (this._message?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ this.Nested.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Value?.GetHashCode() ?? 0);
                return hashCode;
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// The Id of the error.
        /// </summary>
        [CanBeNull] public readonly string Id;
        

        //--------------------------------------------------
        /// <summary>
        /// The error messages.
        /// </summary>
        [NotNull]
        public override string Message => this.ToString();
        
        
        //--------------------------------------------------
        /// <summary>
        /// Additional nested errors.
        /// </summary>
        [NotNull] public readonly Error[] Nested;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString() 
            => Error.ToString(this.Id, this._message, this.Nested);

        
        //--------------------------------------------------
        /// <summary>
        /// The value that caused the error.
        /// </summary>
        [CanBeNull] public readonly object Value;
        
        
        
        /**************************************************
         * Private
         ***************************************************/
        
        //--------------------------------------------------
        /// <summary>
        /// Convert the error to a string.
        /// </summary>
        [NotNull]
        private static string ToString([CanBeNull] string id, [CanBeNull] string message, [NotNull] Error[] nested)
        {
            static string InnerToString(string id, string message, Error[] nested, int indent)
            {
                // add id
                var str = !string.IsNullOrEmpty(id)
                    ? $"{"".PadLeft(indent)}[{id}]:"
                    : null;

                // add message
                str = !string.IsNullOrEmpty(message)
                    ? str is null
                        ? $"{"".PadLeft(indent)}{message}"
                        : $"{str} {message}"
                    : str;

                var newIndent = string.IsNullOrEmpty(str)
                    ? indent
                    : indent + 2;
                
                // add nested errors
                return nested.Length > 0
                    ? str is null
                        ? $"{"".PadLeft(indent)}{string.Join("\n", nested.Select(err => InnerToString(err.Id, err._message, err.Nested, newIndent)))}"
                        : $"{str}\n{string.Join("\n", nested.Select(err => InnerToString(err.Id, err._message, err.Nested, newIndent)))}"
                   : str ?? "Error (without details) occurred.";
            }

            return InnerToString(id, message, nested, 0);
        }
        
        
        //--------------------------------------------------
        [CanBeNull] private readonly string _message;
        
    }
    
}
