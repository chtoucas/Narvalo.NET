// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

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

        public void Deconstruct(out bool succeed, out string error)
        {
            succeed = IsSuccess;
            error = _error;
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
        /// <summary>
        /// Obtains an instance of <see cref="Outcome" /> that represents a successful computation.
        /// </summary>
        public static Outcome Ok => default(Outcome);

        public static Outcome FromError(string error)
        {
            Require.NotNullOrEmpty(error, nameof(error));

            return new Outcome(error);
        }
    }

    // Conversion operators.
    public partial struct Outcome
    {
        public Outcome<Unit> ToOutcomeT()
            => IsError
            ? Outcome<Unit>.FromError(Error)
            : Outcome.Unit;

        public Result<Unit, string> ToResult()
            => IsError
            ? Result<Unit, string>.FromError(Error)
            : Result<string>.Unit;
    }

    // Core methods.
    public partial struct Outcome
    {
        public TResult Match<TResult>(Func<TResult> caseSuccess, Func<string, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsError ? caseError(Error) : caseSuccess();
        }

        public void Do(Action onSuccess, Action<string> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsError) { onError(Error); } else { onSuccess(); }
        }

        public bool OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); return true; }
            return false;
        }

        public bool OnError(Action<string> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error); return true; }
            return false;
        }
    }

    // Monad-like methods.
    public partial struct Outcome
    {
        // Compare w/ Bind(Func<Unit, Outcome<TResult>>).
        public Outcome<TResult> Bind<TResult>(Func<Outcome<TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsError ? Outcome<TResult>.FromError(Error) : binder();
        }

        public Outcome<TResult> ReplaceBy<TResult>(TResult value)
            => IsError ? Outcome<TResult>.FromError(Error) : Of(value);

        public Outcome<TResult> ContinueWith<TResult>(Outcome<TResult> result)
            => IsError ? Outcome<TResult>.FromError(Error) : result;

        // Compare w/ Select(Func<Unit, TResult>).
        public Outcome<TResult> Select<TResult>(Func<TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsError ? Outcome<TResult>.FromError(Error) : Of(selector());
        }
    }

    // Implements the IEquatable<Outcome> interface.
    public partial struct Outcome
    {
        public static bool operator ==(Outcome left, Outcome right) => left.Equals(right);

        public static bool operator !=(Outcome left, Outcome right) => !left.Equals(right);

        public bool Equals(Outcome other)
        {
            if (IsError) { return other.IsError && Error == other.Error; }
            return other.IsSuccess;
        }

        public override bool Equals(object obj) => (obj is Outcome) && Equals((Outcome)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
