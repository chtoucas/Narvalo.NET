// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    // Friendly version of Result<Unit, string>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Outcome.DebugView))]
    public partial struct Outcome : IEquatable<Outcome>
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

        internal string Error { get { Debug.Assert(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        public void Deconstruct(out bool succeed, out string error)
        {
            succeed = IsSuccess;
            error = _error;
        }

        public override string ToString() => IsError ? "Error(" + Error + ")" : "Success";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Outcome"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Outcome.Error"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Outcome _inner;

            public DebugView(Outcome inner) => _inner = inner;

            public bool IsSuccess => _inner.IsSuccess;

            public string Error => _inner._error;
        }
    }

    // Factory methods.
    public partial struct Outcome
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Outcome s_Ok = new Outcome();

        /// <summary>
        /// Obtains an instance of <see cref="Outcome" /> that represents a successful computation.
        /// </summary>
        public static Outcome Ok => s_Ok;

        public static Outcome FromError(string error)
        {
            Require.NotNullOrEmpty(error, nameof(error));

            return new Outcome(error);
        }
    }

    // Core methods.
    public partial struct Outcome
    {
        // Compare w/ Bind(Func<Unit, Outcome<TResult>> func).
        public Outcome<TResult> Bind<TResult>(Func<Outcome<TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsError ? Outcome<TResult>.FromError(Error) : binder();
        }

        // Compare w/ Select(Func<Unit, TResult> func).
        public Outcome<TResult> Map<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Outcome<TResult>.FromError(Error) : Of(func());
        }

        public Outcome<TResult> ReplaceBy<TResult>(TResult result)
            => IsError ? Outcome<TResult>.FromError(Error) : Of(result);

        public Outcome<TResult> ContinueWith<TResult>(Outcome<TResult> result)
            => IsError ? Outcome<TResult>.FromError(Error) : result;

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

        public override bool Equals(object obj) => (obj is Outcome) && Equals((Outcome)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
