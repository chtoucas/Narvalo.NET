// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    // Friendly version of Result<Unit, string>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Outcome.DebugView))]
    public partial struct Outcome : IEquatable<Outcome>, Internal.IResult<string>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Outcome s_Success = new Outcome();

        private readonly string _error;

        private Outcome(string error)
        {
            _error = error;
            IsError = true;
        }

        public static Outcome Success => s_Success;

        public bool IsError { get; }

        public bool IsSuccess => !IsError;

        private string Error_ { get { Demand.State(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        public static Outcome Error(string value)
        {
            Require.NotNullOrEmpty(value, nameof(value));

            return new Outcome(value);
        }

        public override string ToString() => IsSuccess ? "Success" : Format.Current("Error({0})", Error_);

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Outcome{TError}"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Outcome{TError}.Error"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Outcome _inner;

            public DebugView(Outcome inner)
            {
                _inner = inner;
            }

            public bool IsSuccess => _inner.IsSuccess;

            public string Error => _inner._error;
        }
    }

    // Implements the Internal.IResult<string> interface.
    public partial struct Outcome
    {
        public Result<TResult, string> Then<TResult>(Func<Result<TResult, string>> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult, string>.FromError(Error_) : func();
        }

        public Result<TResult, string> Then<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult, string>.FromError(Error_) : Result<TResult, string>.Of(func());
        }

        public Result<TResult, string> Then<TResult>(Result<TResult, string> other)
            => IsError ? Result<TResult, string>.FromError(Error_) : other;

        public Result<TResult, string> Then<TResult>(TResult result)
            => IsError
            ? Result<TResult, string>.FromError(Error_)
            : Result<TResult, string>.Of(result);

        public TResult Match<TResult>(Func<TResult> caseSuccess, Func<string, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess() : caseError(Error_);
        }

        public void Do(Action onSuccess, Action<string> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess) { onSuccess(); } else { onError(Error_); }
        }

        public void OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); }
        }

        public void OnError(Action<string> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error_); }
        }
    }

    // Implements the IEquatable<Outcome> interface.
    public partial struct Outcome
    {
        public static bool operator ==(Outcome left, Outcome right) => left.Equals(right);

        public static bool operator !=(Outcome left, Outcome right) => !left.Equals(right);

        public bool Equals(Outcome other)
        {
            if (IsSuccess) { return other.IsSuccess; }
            return other.IsError && Error_ == other.Error_;
        }

        public override bool Equals(object obj)
            => (obj is Outcome) && Equals((Outcome)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
