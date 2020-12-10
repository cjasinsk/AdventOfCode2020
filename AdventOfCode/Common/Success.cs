using System;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{
    
    /// <summary>
    /// Represents a successful <see cref="Result{T}"/>, containing
    /// the successful value.
    /// </summary>
    public sealed class Success<TValue> : Result<TValue>
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Construct the successful result.
        /// </summary>
        public Success([NotNull] TValue value)
            => this.Value = value ?? throw new ArgumentNullException(nameof(value));

        
        //--------------------------------------------------
        /// <summary>
        /// Convert a value into a successful result.
        /// </summary>
        [NotNull]
        public static implicit operator Success<TValue>([NotNull] TValue value)
            => new Success<TValue>(value);

        
        //--------------------------------------------------
        /// <summary>
        /// Convert a successful result into a value.
        /// </summary>
        [NotNull]
        public static implicit operator TValue([NotNull] Success<TValue> success)
            => success.Value;
        
        
        //--------------------------------------------------
        /// <summary>
        /// The successful value.
        /// </summary>
        [NotNull] public readonly TValue Value;
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString()
            => this.Value.ToString();
        
        
        //--------------------------------------------------
        /// <inheritdoc />
        public override bool Equals(object obj)
            => !object.ReferenceEquals(obj, null)
                && (object.ReferenceEquals(this, obj)
                    || (obj is Success<TValue> success && this.Value.Equals(success.Value)));

        
        //--------------------------------------------------
        /// <inheritdoc />
        public override int GetHashCode()
            => this.Value.GetHashCode();
        
    }
    
}
