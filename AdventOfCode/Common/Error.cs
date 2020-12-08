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
            [NotNull] string id,
            [NotNull] string message,
            [CanBeNull] object value = null,
            [CanBeNull] Error[] nested = null)
        {
            this.Id = id ?? throw new ArgumentNullException(nameof(id));
            this.Messages = new [] { message ?? throw new ArgumentNullException(nameof(message)) };
            this.Nested = nested ?? new Error[0];
            this.Value = value;
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Construct a new Error.
        /// </summary>
        /// <param name="id">The id of the error</param>
        /// <param name="messages">The error messages</param>
        /// <param name="value">The value that caused the error</param>
        /// <param name="nested">Any additional nested errors</param>
        public Error(
            [NotNull] string id,
            [NotNull] string[] messages,
            [CanBeNull] object value = null,
            [CanBeNull] Error[] nested = null)
        {
            this.Id = id ?? throw new ArgumentNullException(nameof(id));
            this.Messages = messages ?? throw new ArgumentNullException(nameof(messages));
            this.Nested = nested ?? new Error[0];
            this.Value = value;
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Nest errors into this error.
        /// </summary>
        [NotNull]
        public Error this[[NotNull, ItemCanBeNull] params Error[] nested]
            => new Error(
                this.Id,
                this.Messages,
                this.Value,
                this.Nested
                    .Concat(nested ?? throw new ArgumentNullException(nameof(nested)))
                    .Where(x => !(x is null))
                    .ToArray());

        
        
        //--------------------------------------------------
        /// <summary>
        /// Add messages into this error.
        /// </summary>
        [NotNull]
        public Error this[[NotNull, ItemCanBeNull] params string[] messages]
            => new Error(
                this.Id,
                this.Messages
                    .Concat(messages)
                    .Where(x => !(x is null))
                    .ToArray(),
                this.Value,
                this.Nested);

        
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
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Messages.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Nested.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Value != null ? this.Value.GetHashCode() : 0);
                return hashCode;
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// The Id of the error.
        /// </summary>
        [NotNull] public readonly string Id;
        

        //--------------------------------------------------
        /// <summary>
        /// The error messages.
        /// </summary>
        [NotNull]
        public override string Message => this.ToString();
        
        
        //--------------------------------------------------
        /// <summary>
        /// The error messages.
        /// </summary>
        [NotNull] public readonly string[] Messages;
        
        
        //--------------------------------------------------
        /// <summary>
        /// Additional nested errors.
        /// </summary>
        [NotNull] public readonly Error[] Nested;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString()
            => Error.ToString(this.Id, this.Messages, this.Nested);

        
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
        private static string ToString([NotNull] string id, [NotNull] string[] messages, [NotNull] Error[] nested)
        {
            static string InnerToString(string id, string[] messages, Error[] nested, int indent)
            {
                var str = $"{"".PadLeft(indent)}[{id}]: ";
                for (var i = 0; i < messages.Length; i += 1)
                {
                    str += i == 0
                        ? messages[i]
                        : $"\n{string.Empty.PadLeft(indent + 2)}{messages[i]}";
                }
                return nested.Length > 0
                    ? $"{str}\n{string.Join("\n", nested.Select(err => InnerToString(err.Id, err.Messages, err.Nested, indent + 2)))}"
                    : str;
            }

            return InnerToString(id, messages, nested, 0);
        }
        
    }
    
}
