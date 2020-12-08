using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{

    /// <summary>
    /// Represents a failed <see cref="Result{T}"/>, with the
    /// error that caused the failure.
    /// </summary>
    public sealed class Failure<TValue> : Result<TValue>
    {

        //--------------------------------------------------
        /// <summary>
        /// Construct a failure.
        /// </summary>
        public Failure([NotNull] Error error)
            => this.Error = error ?? throw new ArgumentNullException(nameof(error));


        //--------------------------------------------------
        /// <summary>
        /// Construct a failure.
        /// </summary>
        /// <param name="id">The id of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="value">The value that caused the error</param>
        public Failure(
            [NotNull] string id,
            [NotNull] string message,
            [CanBeNull] object value = null)
            : this(new Error(id, message, value))
        { }

        
        //--------------------------------------------------
        /// <summary>
        /// Convert an error into a failure.
        /// </summary>
        [NotNull]
        public static implicit operator Failure<TValue>([NotNull] Error error)
            => new Failure<TValue>(error);
        
        
        //--------------------------------------------------
        /// <summary>
        /// Add nested errors.
        /// </summary>
        [NotNull]
        public Failure<TValue> this[[NotNull, ItemCanBeNull] params Error[] errors]
            => new Failure<TValue>(this.Error[errors]);
        
        
        //--------------------------------------------------
        /// <summary>
        /// Add nested errors.
        /// </summary>
        [NotNull]
        public Failure<TValue> this[[NotNull, ItemCanBeNull] IEnumerable<Error> errors]
            => new Failure<TValue>(this.Error[errors.ToArray()]);
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override bool Equals(object obj)
            => !object.ReferenceEquals(obj, null)
                && (object.ReferenceEquals(this, obj)
                    || (obj is Failure<TValue> failure && this.Error.Equals(failure.Error)));

        
        //--------------------------------------------------
        /// <inheritdoc />
        public override int GetHashCode()
            => this.Error.GetHashCode();


        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString()
            => this.Error.ToString();

        
        //--------------------------------------------------
        /// <summary>
        /// The error of the failure.
        /// </summary>
        [NotNull] public readonly Error Error;
        
    }
    
}
