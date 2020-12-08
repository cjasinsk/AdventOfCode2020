using System;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{

    /// <summary>
    /// Represents a failed <see cref="Result{T}"/>, with the
    /// error and value that caused the failure.
    /// </summary>
    public sealed class Failure<TValue> : Result<TValue>
    {

        //--------------------------------------------------
        /// <summary>
        /// Construct a failure.
        /// </summary>
        public Failure([NotNull] Error error, [CanBeNull] object value = null)
        {
            this.Error = error ?? throw new ArgumentNullException(nameof(error));
            this.Value = value;
        }


        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString()
            => this.Error.ToString();

        //--------------------------------------------------
        /// <summary>
        /// The error of the failure.
        /// </summary>
        [NotNull] public readonly Error Error;
        
        
        //--------------------------------------------------
        /// <summary>
        /// The value that caused the failure.
        /// </summary>
        [CanBeNull] public readonly object Value;
        
    }
    
}
