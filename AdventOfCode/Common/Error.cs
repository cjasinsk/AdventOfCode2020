using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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
        /// <param name="message">The error message</param>
        /// <param name="id">The id of the error</param>
        /// <param name="nested">Any additional nested errors</param>
        /// <param name="originFilePath">If left unset, the compiler fills this in with where this constructor was called</param>
        /// <param name="originLineNumber">If left unset, the compiler fills this in with where this constructor was called</param>
        /// <param name="originMemberName">If left unset, the compiler fills this in with where this constructor was called</param>
        public Error(
            [NotNull] string message,
            [CanBeNull] string id = null,
            [CanBeNull] Error[] nested = null,
            [CallerFilePath, NotNull] string originFilePath = "",
            [CallerLineNumber] int originLineNumber = 0,
            [CallerMemberName, NotNull] string originMemberName = "")
            : base(Error.ToExceptionString(originFilePath, originLineNumber, id, message, nested ?? new Error[0]))
        {
            this.Id = id;
            this.Message = message ?? throw new ArgumentNullException(nameof(message));
            this.Nested = nested ?? new Error[0];
            
            // ReSharper disable ExplicitCallerInfoArgument
            this.Origin = new CallerInfo(originFilePath, originLineNumber, originMemberName);
            // ReSharper restore ExplicitCallerInfoArgument
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Nest errors into this error.
        /// </summary>
        [NotNull]
        public Error this[[NotNull] params Error[] nested] =>
            new Error(
                this.Message,
                this.Id,
                this.Nested.Concat(nested ?? throw new ArgumentNullException(nameof(nested))).ToArray(),
                // ReSharper disable ExplicitCallerInfoArgument
                this.Origin.FilePath,
                this.Origin.LineNumber,
                this.Origin.MemberName);
                // ReSharper restore ExplicitCallerInfoArgument


        //--------------------------------------------------
        /// <summary>
        /// The Id of the error.
        /// </summary>
        [CanBeNull] public readonly string Id;
        
        
        //--------------------------------------------------
        /// <summary>
        /// The error message.
        /// </summary>
        [NotNull] public new readonly string Message;
        
        
        //--------------------------------------------------
        /// <summary>
        /// Additional nested errors.
        /// </summary>
        [NotNull] public readonly Error[] Nested;

        
        //--------------------------------------------------
        /// <summary>
        /// The origin where the error was created.
        /// </summary>
        public readonly CallerInfo Origin;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString()
            => Error.ToString(this.Id, this.Message, this.Nested, 0);

        
        
        /**************************************************
         * Private
         ***************************************************/
        
        //--------------------------------------------------
        /// <summary>
        /// Convert the error to a string, with indentation.
        /// </summary>
        [NotNull]
        private static string ToString([CanBeNull] string id, [NotNull] string message, [NotNull] IReadOnlyCollection<Error> nested, int indent)
        {
            var str = $"{"".PadLeft(indent)}{(id is null ? message : $"[{id}]: {message}")}";
            return nested.Count > 0
                ? $"{str}\n{string.Join("\n", nested.Select(err => Error.ToString(err.Id, err.Message, err.Nested, indent + 2)))}"
                : str;
        }

        
        //--------------------------------------------------
        /// <summary>
        /// Convert the error to a string representation useful
        /// when used as an exception.
        /// </summary>
        [NotNull]
        private static string ToExceptionString([NotNull] string filePath, int lineNumber, [CanBeNull] string id, [NotNull] string message, [NotNull] Error[] nested)
        {
            var str = $"An Error was thrown as an exception\n  on {filePath}:line {lineNumber}\n";
            return str + Error.ToString(id, message, nested, 2);
        }
        
    }
    
}
