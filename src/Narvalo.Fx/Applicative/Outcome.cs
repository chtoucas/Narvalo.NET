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
        private readonly string _error;

        private Outcome(string error)
        {
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

        internal string Error { get { Demand.State(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        public override string ToString() => IsSuccess ? "Success" : Format.Current("Error({0})", Error);

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

    // Factory methods.
    public partial struct Outcome
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Outcome s_Success = new Outcome();

        /// <summary>
        /// Obtains an instance of <see cref="Outcome" /> that represents a successful computation.
        /// </summary>
        public static Outcome Success => s_Success;

        public static Outcome FromError(string value)
        {
            Require.NotNullOrEmpty(value, nameof(value));

            return new Outcome(value);
        }
    }

    // Implements the Internal.IResult<string> interface.
    public partial struct Outcome
    {
        public Result<TResult, string> Then<TResult>(Func<Result<TResult, string>> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult, string>.FromError(Error) : func();
        }

        public Result<TResult, string> Then<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Result<TResult, string>.FromError(Error) : Result<TResult, string>.Of(func());
        }

        public Result<TResult, string> Then<TResult>(Result<TResult, string> other)
            => IsError ? Result<TResult, string>.FromError(Error) : other;

        public Result<TResult, string> Then<TResult>(TResult result)
            => IsError
            ? Result<TResult, string>.FromError(Error)
            : Result<TResult, string>.Of(result);

        public TResult Match<TResult>(Func<TResult> caseSuccess, Func<string, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess() : caseError(Error);
        }

        public void Do(Action onSuccess, Action<string> onError)
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

        public void OnError(Action<string> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error); }
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
            return other.IsError && Error == other.Error;
        }

        public override bool Equals(object obj)
            => (obj is Outcome) && Equals((Outcome)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
