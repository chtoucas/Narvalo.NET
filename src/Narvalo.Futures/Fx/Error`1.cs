// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    // Provides static helpers for Result<TError>.
    //public static partial class Result
    //{
    //    [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
    //    public static Result<ExceptionDispatchInfo> Void => Result<ExceptionDispatchInfo>.Void;

    //    /// <summary>
    //    /// Obtains an instance of the <see cref="Result{TError}"/> class for the specified value.
    //    /// </summary>
    //    /// <typeparam name="TError">The underlying type of <paramref name="value"/>.</typeparam>
    //    /// <param name="value">A value to be wrapped into an object of type <see cref="Result{TError}"/>.</param>
    //    /// <returns>An instance of the <see cref="Result{TError}"/> class for the specified value.</returns>
    //    public static Result<TError> FromError<TError>(TError value) => Result<TError>.η(value);
    //}

    // Friendly version of Result<Unit, TError>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Error<>.DebugView))]
    [Obsolete]
    public partial struct Error<TError> : IEquatable<Error<TError>>, Internal.IMaybe<TError>
    {
        private readonly TError _error;

        private Error(TError error)
        {
            Demand.NotNullUnconstrained(error);
            _error = error;
            IsError = true;
        }

        public bool IsSuccess => !IsError;

        public bool IsError { get; }

        internal TError Value { get { Demand.State(IsError); return _error; } }

        public override string ToString() => IsError ? Format.Current("Error({0})", Value) : "Success";

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Error{TError}"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Error<TError> _inner;

            public DebugView(Error<TError> inner)
            {
                _inner = inner;
            }

            public bool IsError => _inner.IsError;

            public TError Error => IsError ? _inner.Value : default(TError);
        }
    }

    // Conversion operators.
    public partial struct Error<TError>
    {
        public TError ToError()
        {
            if (IsSuccess) { throw new InvalidCastException("XXX"); }

            return Value;
        }

        public Maybe<Unit> ToMaybe() => IsSuccess ? Maybe.Unit : Maybe<Unit>.None;

        public static explicit operator TError(Error<TError> value) => value.ToError();

        //public static explicit operator Result<TError>(TError error) => Result.FromError(error);
    }

    // Provides the core Monad methods.
    public partial struct Error<TError>
    {
        public Error<TResult> Bind<TResult>(Func<TError, Error<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess ? Error<TResult>.Void : selector(Value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Error<TError> η(TError value)
        {
            Require.NotNullUnconstrained(value, nameof(value));

            return new Error<TError>(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Error<TError> μ(Error<Error<TError>> square)
            => square.IsError ? square.Value : Void;
    }

    // Provides the core MonadOr methods.
    public partial struct Error<TError>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Error<TError> s_Void = new Error<TError>();

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Error<TError> Void => s_Void;

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "OrElse", Justification = "[Intentionally] Standard name for the monoid method.")]
        public Error<TError> OrElse(Error<TError> other) => IsSuccess ? other : this;
    }

    // Implements the Internal.IMaybe<TError> interface.
    public partial struct Error<TError>
    {
        // Named <c>maybeToList</c> in Haskell parlance.
        public IEnumerable<TError> ToEnumerable()
            => IsSuccess ? Enumerable.Empty<TError>() : Sequence.Of(Value);

        public IEnumerator<TError> GetEnumerator() => ToEnumerable().GetEnumerator();

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess)
        {
            Require.NotNull(caseError, nameof(caseError));
            Require.NotNull(caseSuccess, nameof(caseSuccess));

            return IsError ? caseError(Value) : caseSuccess();
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess)
        {
            Require.NotNull(caseError, nameof(caseError));

            return IsError ? caseError(Value) : caseSuccess;
        }

        public TResult Coalesce<TResult>(
            Func<TError, bool> predicate,
            Func<TError, TResult> selector,
            Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return IsError && predicate(Value) ? selector(Value) : otherwise();
        }

        public TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult thenResult, TResult elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsError && predicate(Value) ? thenResult : elseResult;
        }

        public void When(Func<TError, bool> predicate, Action<TError> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (IsError && predicate(Value))
            {
                action(Value);
            }
            else
            {
                otherwise();
            }
        }

        public void When(Func<TError, bool> predicate, Action<TError> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsError && predicate(Value)) { action(Value); }
        }

        // Alias for OnError(). Publicly hidden.
        void Internal.IContainer<TError>.Do(Action<TError> action) => OnError(action);

        public void OnSuccess(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(); }
        }

        public void OnError(Action<TError> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Value); }
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public void Do(Action<TError> onError, Action onSuccess)
        {
            Require.NotNull(onError, nameof(onError));
            Require.NotNull(onSuccess, nameof(onSuccess));

            if (IsError)
            {
                onError(Value);
            }
            else
            {
                onSuccess();
            }
        }
    }

    // Implements the IEquatable<Result<TError>> interfaces.
    public partial struct Error<TError>
    {
        public static bool operator ==(Error<TError> left, Error<TError> right) => left.Equals(right);

        public static bool operator !=(Error<TError> left, Error<TError> right) => !left.Equals(right);

        public bool Equals(Error<TError> other) => Equals(other, EqualityComparer<TError>.Default);

        public bool Equals(Error<TError> other, IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSuccess) { return other.IsSuccess; }

            return other.IsError && comparer.Equals(Value, other.Value);
        }

        public override bool Equals(object obj) => Equals(obj, EqualityComparer<TError>.Default);

        public bool Equals(object other, IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is Error<TError>)) { return false; }

            return Equals((Error<TError>)other, comparer);
        }

        public override int GetHashCode() => GetHashCode(EqualityComparer<TError>.Default);

        public int GetHashCode(IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return IsError ? comparer.GetHashCode(Value) : 0;
        }
    }
}

