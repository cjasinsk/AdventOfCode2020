using System;
using System.Runtime.Remoting.Messaging;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{
    
    public readonly struct Maybe<T>
    {

        private Maybe(T value, bool hasValue)
        {
            if (value is null && hasValue) { throw new InvalidOperationException($"Can not set '{nameof(null)}' as a value."); }
            
            this._value = value;
            this._hasValue = hasValue;
        }


        
        public override bool Equals(object other)
            => other switch
            {
                T value => this._hasValue && this._value.Equals(value),
                Maybe<T> value => this._hasValue
                    ? value._hasValue && this._value.Equals(value._value)
                    : !value._hasValue,
                null => !this._hasValue,
                _ => false
            };


        public override int GetHashCode()
            => this._hasValue ? this._value.GetHashCode() : 0;


        public Maybe<TResult> OnSome<TResult>(Func<T, TResult> onSome)
            => this.Select(onSome, () => default);

        public Maybe<T> OnSome(Action<T> onSome)
        {
            if (this._hasValue)
            {
                onSome(this._value);
            }

            return this;
        }
        
        /// <summary>
        /// <see cref=""/>
        /// </summary>
        /// <param name="onSome"></param>
        /// <param name="onNone"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public Maybe<TResult> Select<TResult>(
            Func<T, TResult> onSome,
            Func<TResult> onNone)
        {
            if (onSome is null) { throw new ArgumentNullException(nameof(onSome)); }
            if (onNone is null) { throw new ArgumentNullException(nameof(onNone)); }

            return this._hasValue
                ? onSome(this._value)
                : onNone();
        }


        public override string ToString()
            => this._hasValue ? this._value.ToString() : string.Empty;

        
        public T Unwrap()
        {
            if (!this._hasValue) { throw new InvalidOperationException("Can not unwrap a maybe with no value."); }
            return this._value!;
        }

        public Maybe<T> Where(Func<T, bool> predicate)
        {
            if (predicate is null) { throw new ArgumentNullException(nameof(predicate)); }

            return this._hasValue && predicate(this._value)
                ? this
                : Maybe<T>.None;
        }
        
        
        public static implicit operator Maybe<T>(T value)
            => new Maybe<T>(value, !(value is null));


        public static bool operator ==(Maybe<T> left, Maybe<T> right)
            => left.Equals(right);

        
        public static bool operator ==(Maybe<T> left, T right)
            => left.Equals(right);

        
        public static bool operator ==(T left, Maybe<T> right)
            => right.Equals(left);

        
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
            => !left.Equals(right);

        
        public static bool operator !=(Maybe<T> left, T right)
            => !left.Equals(right);

        
        public static bool operator !=(T left, Maybe<T> right)
            => !right.Equals(left);
        
        
        public static Maybe<T> None
            => new Maybe<T>(default, false);
        
        
        public static Maybe<T> Some(T value, bool hasValue = true)
            => new Maybe<T>(value, hasValue);
        
        
        private readonly T _value;
        private readonly bool _hasValue;
    }
    
}
