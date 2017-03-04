// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.ExceptionServices;

    using Narvalo.Properties;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result.DebugView))]
    public partial struct Result : IEquatable<Result>, Internal.IResult<ExceptionDispatchInfo>
    {
        private readonly ExceptionDispatchInfo _exceptionInfo;

        private Result(ExceptionDispatchInfo exceptionInfo)
        {
            Demand.NotNull(exceptionInfo);

            _exceptionInfo = exceptionInfo;
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
        internal ExceptionDispatchInfo ExceptionInfo { get { Demand.State(IsError); return _exceptionInfo; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        public void ThrowIfError()
        {
            if (IsError) { ExceptionInfo.Throw(); }
        }

        public override string ToString()
            => IsError ? Format.Current("Error({0})", ExceptionInfo.SourceException) : "Success";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Result.ExceptionInfo"/> does not throw in the debugger
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

            public ExceptionDispatchInfo ExceptionInfo => _inner._exceptionInfo;
        }
    }

    // Conversion operators.
    public partial struct Result
    {
        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidCast_ToError); }
            return ExceptionInfo;
        }

        public static explicit operator ExceptionDispatchInfo(Result value) => value.ToExceptionInfo();
    }

    // Implements the Internal.IResult<ExceptionDispatchInfo> interface.
    public partial struct Result
    {
        public Result<TResult, ExceptionDispatchInfo> Then<TResult>(
            Func<Result<TResult, ExceptionDispatchInfo>> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult, ExceptionDispatchInfo>.FromError(ExceptionInfo) : func();
        }

        public Result<TResult, ExceptionDispatchInfo> Then<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError
                ? Result<TResult, ExceptionDispatchInfo>.FromError(ExceptionInfo)
                : Result<TResult, ExceptionDispatchInfo>.Of(func());
        }

        public Result<TResult, ExceptionDispatchInfo> Then<TResult>(
            Result<TResult, ExceptionDispatchInfo> other)
            => IsError ? Result<TResult, ExceptionDispatchInfo>.FromError(ExceptionInfo) : other;

        public Result<TResult, ExceptionDispatchInfo> Then<TResult>(TResult result)
            => IsError
            ? Result<TResult, ExceptionDispatchInfo>.FromError(ExceptionInfo)
            : Result<TResult, ExceptionDispatchInfo>.Of(result);

        public TResult Match<TResult>(
            Func<TResult> caseSuccess,
            Func<ExceptionDispatchInfo, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess() : caseError(ExceptionInfo);
        }

        public void Do(Action onSuccess, Action<ExceptionDispatchInfo> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess) { onSuccess(); } else { onError(ExceptionInfo); }
        }

        public void OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); }
        }

        public void OnError(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(ExceptionInfo); }
        }
    }

    // Implements the IEquatable<Result> interface.
    public partial struct Result
    {
        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public bool Equals(Result other)
        {
            if (IsError) { return other.IsError && ReferenceEquals(ExceptionInfo, other.ExceptionInfo); }
            return other.IsSuccess;
        }

        public override bool Equals(object obj) => (obj is Result) && Equals((Result)obj);

        public override int GetHashCode() => _exceptionInfo?.GetHashCode() ?? 0;
    }
}
