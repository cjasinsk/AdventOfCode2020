using System;
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
        
        
        //--------------------------------------------------
        /// <summary>
        /// Convert from an error to a failed result.
        /// </summary>
        [NotNull]
        public static implicit operator Result<TValue>([NotNull] Error error)
            => new Failure<TValue>(error);
        
        
        //--------------------------------------------------
        /// <summary>
        /// Creates an awaiter for the 'await' keyword.
        /// </summary>
        [NotNull]
        public ResultAwaiter<TValue> GetAwaiter()
            => new ResultAwaiter<TValue>(this);

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
                Failure<T> failure => throw failure.Error, // short circuits execution, will be caught by method builder
                _ => throw new InvalidOperationException($"The result is in an unknown awaitable state {this._result.GetType().Name}.")
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
            if (exception is Error error)
            {
                this.Task = error;
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
    
}
