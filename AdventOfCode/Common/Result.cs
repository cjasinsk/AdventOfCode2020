using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

using JetBrains.Annotations;

namespace AdventOfCode.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Represents a result that may be <see cref="Success{TValue}"/>
    /// or <see cref="Failure{TValue}"/>.
    /// </summary>
    [AsyncMethodBuilder(typeof(ResultAsyncMethodBuilder<>))]
    public abstract class Result<TValue>
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Deconstruct the result into a possible value, and
        /// a possible error.
        /// </summary>
        public void Deconstruct(out TValue value, out Error error)
        {
            switch (this)
            {
                case Success<TValue> s:
                    value = s.Value;
                    error = default;
                    return;
                case Failure<TValue> f:
                    value = default;
                    error = f.Error;
                    return;
                default:
                    value = default;
                    error = default;
                    return;
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Convert from a value to a successful result.
        /// </summary>
        [NotNull]
        public static implicit operator Result<TValue>([NotNull] TValue value)
            => new Success<TValue>(value);

    }


    public static class ResultAwaiterExtensions
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Creates an awaiter for the 'await' keyword.
        /// </summary>
        [NotNull]
        public static ResultAwaiter<TValue> GetAwaiter<TValue>([NotNull] this Result<TValue> result)
            => new ResultAwaiter<TValue>(result ?? throw new ArgumentNullException(nameof(result)));
        
        
        //--------------------------------------------------
        /// <summary>
        /// Creates an awaiter for the 'await' keyword.
        /// </summary>
        [NotNull]
        public static EnumerableResultAwaiter<TValue> GetAwaiter<TValue>([NotNull] this IEnumerable<Result<TValue>> results)
            => new EnumerableResultAwaiter<TValue>(results ?? throw new ArgumentNullException(nameof(results)));
        
        
        //--------------------------------------------------
        /// <summary>
        /// Creates an awaiter for the 'await' keyword.
        /// </summary>
        [NotNull]
        public static ImplicitResultAwaiter<TValue> GetAwaiter<TValue>(this (string Id, Result<TValue> Result) result)
            => new ImplicitResultAwaiter<TValue>(result);

    }
    
    /// <summary>
    /// An awaiter type used by the 'await' keyword.
    /// Ex: await new Success{string}("Test");
    /// </summary>
    public sealed class ResultAwaiter<T> : INotifyCompletion
    {
        
        /**************************************************
         * Public
         ***************************************************/

        //--------------------------------------------------
        /// <summary>
        /// Construct the awaiter with the provider result.
        /// </summary>
        public ResultAwaiter([NotNull] Result<T> result)
            => this._result = result ?? throw new ArgumentNullException(nameof(result));
        
            
        //--------------------------------------------------
        /// <summary>
        /// Indicates if the awaiter has completed.
        /// The async method builder checks this first prior
        /// to calling OnCompleted.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public bool IsCompleted { get; private set; }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Gets the result the 'await' returns.
        /// If <see cref="Success{TValue}"/> the value is returned.
        /// If <see cref="Failure{TValue}"/> the error is thrown. 
        /// </summary>
        /// <remarks>
        /// Required by compiler
        /// </remarks>
        [NotNull]
        public T GetResult()
            => this._result switch
            {
                Success<T> success => success.Value,
                Failure<T> failure => throw new ErrorException(failure.Error), // short circuits execution, will be caught by method builder
                _ => throw new InvalidOperationException($"Unknown Result state '{this._result.GetType().Name}'.")
            };

        
        //--------------------------------------------------
        /// <summary>
        /// The async method builder calls this when it is ready
        /// to continue.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void OnCompleted(Action continuation)
        {
            continuation?.Invoke();
            this.IsCompleted = true;
        }

        
        /**************************************************
         * Private
         ***************************************************/
        
        //--------------------------------------------------
        [NotNull] private readonly Result<T> _result;
        
    }


    
    /// <summary>
    /// An awaiter type used by the 'await' keyword
    /// for a collection of results
    /// </summary>
    public sealed class EnumerableResultAwaiter<T> : INotifyCompletion
    {
        
        /**************************************************
         * Public
         ***************************************************/

        //--------------------------------------------------
        /// <summary>
        /// Construct the awaiter with the provider result.
        /// </summary>
        public EnumerableResultAwaiter([NotNull] IEnumerable<Result<T>> results)
            => this._results = results ?? throw new ArgumentNullException(nameof(results));
        
            
        //--------------------------------------------------
        /// <summary>
        /// Indicates if the awaiter has completed.
        /// The async method builder checks this first prior
        /// to calling OnCompleted.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public bool IsCompleted { get; private set; }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Gets the result the 'await' returns.
        /// If <see cref="Success{TValue}"/> the value is returned.
        /// If <see cref="Failure{TValue}"/> the error is thrown. 
        /// </summary>
        /// <remarks>
        /// Required by compiler
        /// </remarks>
        [NotNull]
        public IEnumerable<T> GetResult()
        {
            var flattened = this._results.Aggregate(default(Result<IEnumerable<T>>), (total, result)
                => total switch
                {
                    Success<IEnumerable<T>> success => result switch
                    {
                        Success<T> s => new Success<IEnumerable<T>>(success.Value.Concat(new [] { s.Value })),
                        Failure<T> f => new Failure<IEnumerable<T>>(nested: new [] { f.Error }),
                        _ => throw new InvalidOperationException($"Unknown Result state '{result.GetType().Name}'.")
                    },
                    Failure<IEnumerable<T>> failure => result switch
                    {
                        Failure<T> f => new Failure<IEnumerable<T>>(f.Error.Id, f.Error.Message, f.Error.Value,
                            f.Error.Nested.Concat(new [] { f.Error }).ToArray()),
                        _ => failure
                    },
                    _ => result switch
                    {
                        Success<T> s => new Success<IEnumerable<T>>(new[] { s.Value }),
                        Failure<T> f => new Failure<IEnumerable<T>>(nested: new [] { f.Error }),
                        _ => throw new InvalidOperationException($"Unknown Result state '{result.GetType().Name}'.")
                    }
                });
            
            return flattened switch
            {
                Success<IEnumerable<T>> success => success.Value,
                Failure<IEnumerable<T>> failure => throw failure.Error, // short circuits execution, will be caught by method builder
                _ => throw new InvalidOperationException($"Unknown Result state '{flattened.GetType().Name}'.")
            };
        }


        //--------------------------------------------------
        /// <summary>
        /// The async method builder calls this when it is ready
        /// to continue.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void OnCompleted(Action continuation)
        {
            continuation?.Invoke();
            this.IsCompleted = true;
        }

        
        /**************************************************
         * Private
         ***************************************************/
        
        //--------------------------------------------------
        [NotNull] private readonly IEnumerable<Result<T>> _results;
        
    }



    public sealed class ErrorException : Exception
    {
        public ErrorException([NotNull] Error error)
            => this.Error = error ?? throw new ArgumentNullException(nameof(error));

        [NotNull] public override string Message => this.Error.ToString();
        [NotNull] public readonly Error Error;
    }
    /// <summary>
    /// An awaiter type used by the 'await' keyword.
    /// Ex: await ("id", result);
    /// </summary>
    public sealed class ImplicitResultAwaiter<T> : INotifyCompletion
    {
        
        /**************************************************
         * Public
         ***************************************************/

        //--------------------------------------------------
        /// <summary>
        /// Construct the awaiter with the provider result.
        /// </summary>
        public ImplicitResultAwaiter((string Id, Result<T> Result) result)
            => this._result = result;
        
            
        //--------------------------------------------------
        /// <summary>
        /// Indicates if the awaiter has completed.
        /// The async method builder checks this first prior
        /// to calling OnCompleted.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public bool IsCompleted { get; private set; }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Gets the result the 'await' returns.
        /// If <see cref="Success{TValue}"/> the value is returned.
        /// If <see cref="Failure{TValue}"/> the error is thrown. 
        /// </summary>
        /// <remarks>
        /// Required by compiler
        /// </remarks>
        [NotNull]
        public T GetResult()
            => this._result.Result switch
            {
                Success<T> success => success.Value,
                Failure<T> failure => throw new ErrorException(new Error(this._result.Id, nested: new [] { failure.Error })), // short circuits execution, will be caught by method builder
                _ => throw new InvalidOperationException($"Unknown Result state '{this._result.GetType().Name}'.")
            };

        
        //--------------------------------------------------
        /// <summary>
        /// The async method builder calls this when it is ready
        /// to continue.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void OnCompleted(Action continuation)
        {
            continuation?.Invoke();
            this.IsCompleted = true;
        }

        
        /**************************************************
         * Private
         ***************************************************/
        
        //--------------------------------------------------
        private readonly (string Id, Result<T> Result) _result;
        
    }
    
    
    
    /// <summary>
    /// Constructs the async state machine for <see cref="Result{TValue}"/>
    /// async methods that return a result.
    /// Ex: public static async Result{string} Test() {...}
    /// </summary>
    public sealed class ResultAsyncMethodBuilder<T>
    {
        
        //--------------------------------------------------
        /// <summary>
        /// Construct the async method builder
        /// </summary>
        public ResultAsyncMethodBuilder()
            => this.Task = default;
        
        
        //--------------------------------------------------
        /// <summary>
        /// Static constructor for the async method builder.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        [NotNull]
        public static ResultAsyncMethodBuilder<T> Create()
            => new ResultAsyncMethodBuilder<T>();
        
        
        //--------------------------------------------------
        /// <summary>
        /// Starts the stateMachine
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void Start<TStateMachine>([NotNull] ref TStateMachine stateMachine)
            where TStateMachine : IAsyncStateMachine
        {
            if (stateMachine is null) { throw new ArgumentNullException(nameof(stateMachine)); }
            stateMachine.MoveNext();
        }

        
        //--------------------------------------------------
        /// <summary></summary>
        /// <remarks>Required by compiler</remarks>
        public void SetStateMachine(IAsyncStateMachine stateMachine) { }
        
        
        //--------------------------------------------------
        /// <summary>
        /// handles any exceptions that occur within the async
        /// method. 
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void SetException([NotNull] Exception exception)
        {
            // listen for any short-circuit Error thrown by the ResultAwaiter
            if (exception is ErrorException ex)
            {
                this.Task = new Failure<T>(ex.Error);
            }
            else
            {
                // rethrow all other exceptions
                ExceptionDispatchInfo.Capture(exception).Throw();
            }
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Sets the result of the async method.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void SetResult(T result)
            => this.Task = result;
        
        
        //--------------------------------------------------
        /// <summary>
        /// Pass the next step in the state machine to the
        /// currently awaited on value.
        /// NOTE: the TAwaiter does not have to be a Result{T},
        /// it is anything that can be awaited on that occurs within
        /// the async method.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void AwaitOnCompleted<TAwaiter, TStateMachine>([NotNull] ref TAwaiter awaiter, [NotNull] ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is null) { throw new ArgumentNullException(nameof(awaiter)); }
            if (stateMachine is null) { throw new ArgumentNullException(nameof(stateMachine)); }
            
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        
        //--------------------------------------------------
        /// <summary>
        /// Pass the next step in the state machine to the
        /// currently awaited on value.
        /// NOTE: the TAwaiter does not have to be a Result{T},
        /// it is anything that can be awaited on that occurs within
        /// the async method.
        /// </summary>
        /// <remarks>Required by compiler</remarks>
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>([NotNull] ref TAwaiter awaiter, [NotNull] ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is null) { throw new ArgumentNullException(nameof(awaiter)); }
            if (stateMachine is null) { throw new ArgumentNullException(nameof(stateMachine)); }
            
            awaiter.OnCompleted(stateMachine.MoveNext);
        }

        
        //--------------------------------------------------
        /// <summary>
        /// The "task" (final result) the async method returns. 
        /// </summary>
        /// <remarks>Required by Compiler</remarks>
        [CanBeNull] public Result<T> Task { get; private set; }
        
    }
 
    
    
    public static class Result
    {

        //--------------------------------------------------
        [NotNull]
        public static Result<TValue> From<TValue>(
            [NotNull] string id,
            [NotNull] Func<Result<TValue>> action)
            => Result.From(id, action());
        
        
        //--------------------------------------------------
        [NotNull]
        public static Result<TValue> From<TValue>(
            [NotNull] string id,
            [NotNull] Result<TValue> result)
            => result switch
            {
                Success<TValue> success => success,
                Failure<TValue> failure => new Failure<TValue>(id, nested: new [] { failure.Error }),
                _ => throw new InvalidOperationException("Unknown result state.")
            };
        
        
        //--------------------------------------------------
        /// <summary>
        /// Validate 2 results in parallel
        /// </summary>
        public static Result<(T1, T2)> From<T1, T2>(
            (string Id, Result<T1> Result) r1,
            (string Id, Result<T2> Result) r2)
        {
            var (v1, e1) = Result.From(r1.Id, r1.Result);
            var (v2, e2) = Result.From(r2.Id, r2.Result);

            return e1 || e2
                ? new Failure<(T1, T2)>(nested: new [] { e1, e2 })
                : (v1, v2).Success();
        }
        
        
        //--------------------------------------------------
        /// <summary>
        /// Validate 7 results in parallel
        /// </summary>
        public static Result<(T1, T2, T3, T4, T5, T6, T7)> From<T1, T2, T3, T4, T5, T6, T7>(
            (string Id, Result<T1> Result) result1,
            (string Id, Result<T2> Result) result2,
            (string Id, Result<T3> Result) result3,
            (string Id, Result<T4> Result) result4,
            (string Id, Result<T5> Result) result5,
            (string Id, Result<T6> Result) result6,
            (string Id, Result<T7> Result) result7)
        {
            var (v1, e1) = Result.From(result1.Id, result1.Result);
            var (v2, e2) = Result.From(result2.Id, result2.Result);
            var (v3, e3) = Result.From(result3.Id, result3.Result);
            var (v4, e4) = Result.From(result4.Id, result4.Result);
            var (v5, e5) = Result.From(result5.Id, result5.Result);
            var (v6, e6) = Result.From(result6.Id, result6.Result);
            var (v7, e7) = Result.From(result7.Id, result7.Result);
            
            return e1 || e2 || e3 || e4 || e5 || e6 || e7
                ? new Failure<(T1, T2, T3, T4, T5, T6, T7)>(nested: new [] { e1, e2, e3, e4, e5, e6, e7 })
                : (v1, v2, v3, v4, v5, v6, v7).Success();
        }
    }
    
}
