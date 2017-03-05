// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    using Narvalo.Properties;

    // Friendly version of Result<Unit, ExceptionDispatchInfo>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result.DebugView))]
    public partial struct Result : IEquatable<Result>
    {
        private readonly ExceptionDispatchInfo _error;

        private Result(ExceptionDispatchInfo error)
        {
            Demand.NotNull(error);

            _error = error;
            IsError = true;
        }

        /// <summary>
        /// Gets a value indicating whether the object is the result of an unsuccessful computation.
        /// </summary>
        /// <value>true if the outcome was unsuccessful; otherwise false.</value>
        public bool IsError { get; }

        /// <summary>
        /// Gets a value indicating whether the object is the result of a successful computation.
        /// </summary>
        /// <value>true if the outcome was successful; otherwise false.</value>
        public bool IsSuccess => !IsError;

        /// <summary>
        /// Obtains the captured exception state.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal ExceptionDispatchInfo Error { get { Demand.State(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        public void ThrowIfError()
        {
            if (IsError) { Error.Throw(); }
        }

        public override string ToString()
            => IsError ? "Error(" + Error.SourceException.ToString() + ")" : "Success";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Result.Error"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result _inner;

            public DebugView(Result inner)
            {
                _inner = inner;
            }

            public bool IsSuccess => _inner.IsSuccess;

            public ExceptionDispatchInfo Error => _inner._error;
        }
    }

    // Factory methods.
    public partial struct Result
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Result s_Ok = new Result();

        /// <summary>
        /// Obtains an instance of <see cref="Result" /> that represents a successful computation.
        /// </summary>
        public static Result Ok => s_Ok;

        public static Result FromError(ExceptionDispatchInfo error)
        {
            Require.NotNull(error, nameof(error));

            return new Result(error);
        }
    }

    // Conversion operators.
    public partial struct Result
    {
        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidCast_ToError); }
            return Error;
        }

        public static explicit operator ExceptionDispatchInfo(Result value) => value.ToExceptionInfo();
    }

    // Core methods.
    public partial struct Result
    {
        public Result<TResult> Bind<TResult>(Func<Result<TResult>> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult>.FromError(Error) : func();
        }

        public Result<TResult> Select<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult>.FromError(Error) : Of(func());
        }

        public Result<TResult> ReplaceBy<TResult>(TResult result)
            => IsError ? Result<TResult>.FromError(Error) : Of(result);

        public Result<TResult> ContinueWith<TResult>(Result<TResult> other)
            => IsError ? Result<TResult>.FromError(Error) : other;

        public TResult Match<TResult>(
            Func<TResult> caseSuccess,
            Func<ExceptionDispatchInfo, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess() : caseError(Error);
        }

        public void Do(Action onSuccess, Action<ExceptionDispatchInfo> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess) { onSuccess(); } else { onError(Error); }
        }

        public void OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); }
        }

        public void OnError(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error); }
        }
    }

    // Implements the IEquatable<Result> interface.
    public partial struct Result
    {
        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public bool Equals(Result other)
        {
            if (IsError) { return other.IsError && ReferenceEquals(Error, other.Error); }
            return other.IsSuccess;
        }

        public override bool Equals(object obj) => (obj is Result) && Equals((Result)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
