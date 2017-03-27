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
    [DebuggerTypeProxy(typeof(Fallible.DebugView))]
    public partial struct Fallible : IEquatable<Fallible>
    {
        private readonly ExceptionDispatchInfo _error;

        private Fallible(ExceptionDispatchInfo error)
        {
            Debug.Assert(error != null);

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
        internal ExceptionDispatchInfo Error { get { Debug.Assert(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        public void ThrowIfError()
        {
            if (IsError) { Error.Throw(); }
        }

        public override string ToString()
            => IsError ? "Error(" + Error.ToString() + ")" : "Success";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Fallible"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Fallible.Error"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Fallible _inner;

            public DebugView(Fallible inner) => _inner = inner;

            public bool IsSuccess => _inner.IsSuccess;

            public ExceptionDispatchInfo Error => _inner._error;
        }
    }

    // Factory methods.
    public partial struct Fallible
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Fallible s_Ok = new Fallible();

        /// <summary>
        /// Obtains an instance of <see cref="Fallible" /> that represents a successful computation.
        /// </summary>
        public static Fallible Ok => s_Ok;

        public static Fallible FromError(ExceptionDispatchInfo error)
        {
            Require.NotNull(error, nameof(error));

            return new Fallible(error);
        }
    }

    // Conversion operators.
    public partial struct Fallible
    {
        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidConversionToError); }
            return Error;
        }

        public static explicit operator ExceptionDispatchInfo(Fallible value) => value.ToExceptionInfo();
    }

    // Core methods.
    public partial struct Fallible
    {
        public Fallible<TResult> Bind<TResult>(Func<Fallible<TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsError ? Fallible<TResult>.FromError(Error) : binder();
        }

        public Fallible<TResult> Select<TResult>(Func<TResult> func)
        {
            Require.NotNull(func, nameof(func));

            return IsError ? Fallible<TResult>.FromError(Error) : Of(func());
        }

        public Fallible<TResult> ReplaceBy<TResult>(TResult result)
            => IsError ? Fallible<TResult>.FromError(Error) : Of(result);

        public Fallible<TResult> ContinueWith<TResult>(Fallible<TResult> other)
            => IsError ? Fallible<TResult>.FromError(Error) : other;

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

    // Implements the IEquatable<Fallible> interface.
    public partial struct Fallible
    {
        public static bool operator ==(Fallible left, Fallible right) => left.Equals(right);

        public static bool operator !=(Fallible left, Fallible right) => !left.Equals(right);

        public bool Equals(Fallible other)
        {
            if (IsError) { return other.IsError && ReferenceEquals(Error, other.Error); }
            return other.IsSuccess;
        }

        public override bool Equals(object obj) => (obj is Fallible) && Equals((Fallible)obj);

        public override int GetHashCode() => _error?.GetHashCode() ?? 0;
    }
}
