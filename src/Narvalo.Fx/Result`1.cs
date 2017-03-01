// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    // Friendly version of Result<Unit, TError>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result<>.DebugView))]
    public partial struct Result<TError> : IEquatable<Result<TError>>, Internal.IMaybe<TError>
    {
        private readonly TError _error;

        private Result(TError error)
        {
            Demand.NotNullUnconstrained(error);
            _error = error;
            IsError = true;
        }

        public bool IsSuccess => !IsError;

        public bool IsError { get; }

        internal TError Error { get { Demand.State(IsError); return _error; } }

        public override string ToString() => IsError ? Format.Current("Error({0})", Error) : "Success";

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result{TError}"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result<TError> _inner;

            public DebugView(Result<TError> inner)
            {
                _inner = inner;
            }

            public bool IsError => _inner.IsError;

            public TError Error => IsError ? _inner.Error : default(TError);
        }
    }

    // Conversion operators.
    public partial struct Result<TError>
    {
        public TError ToError()
        {
            if (IsSuccess) { throw new InvalidCastException("XXX"); }

            return Error;
        }

        public Maybe<Unit> ToMaybe() => IsSuccess ? Maybe.Unit : Maybe<Unit>.None;

        public static explicit operator TError(Result<TError> value) => value.ToError();

        public static explicit operator Result<TError>(TError error) => Result.FromError(error);
    }

    // Provides the core Monad methods.
    public partial struct Result<TError>
    {
        public Result<TResult> Bind<TResult>(Func<TError, Result<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess ? Result<TResult>.Void : selector(Error);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<TError> η(TError value)
        {
            Require.NotNullUnconstrained(value, nameof(value));

            return new Result<TError>(value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<TError> μ(Result<Result<TError>> square)
            => square.IsError ? square.Error : Void;
    }

    // Provides the core MonadOr methods.
    public partial struct Result<TError>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Result<TError> s_Void = new Result<TError>();

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Result<TError> Void => s_Void;

        [SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "OrElse", Justification = "[Intentionally] Standard name for the monoid method.")]
        public Result<TError> OrElse(Result<TError> other) => IsSuccess ? other : this;
    }

    // Implements the Internal.IMaybe<TError> interface.
    public partial struct Result<TError>
    {
        // Named <c>maybeToList</c> in Haskell parlance.
        public IEnumerable<TError> ToEnumerable()
            => IsSuccess ? Enumerable.Empty<TError>() : Sequence.Of(Error);

        public IEnumerator<TError> GetEnumerator() => ToEnumerable().GetEnumerator();

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<TError, TResult> caseError, Func<TResult> caseSuccess)
        {
            Require.NotNull(caseError, nameof(caseError));
            Require.NotNull(caseSuccess, nameof(caseSuccess));

            return IsError ? caseError(Error) : caseSuccess();
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<TError, TResult> caseError, TResult caseSuccess)
        {
            Require.NotNull(caseError, nameof(caseError));

            return IsError ? caseError(Error) : caseSuccess;
        }

        public TResult Coalesce<TResult>(
            Func<TError, bool> predicate,
            Func<TError, TResult> selector,
            Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return IsError && predicate(Error) ? selector(Error) : otherwise();
        }

        public TResult Coalesce<TResult>(Func<TError, bool> predicate, TResult thenResult, TResult elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsError && predicate(Error) ? thenResult : elseResult;
        }

        public void When(Func<TError, bool> predicate, Action<TError> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (IsError && predicate(Error))
            {
                action(Error);
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

            if (IsError && predicate(Error)) { action(Error); }
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

            if (IsError) { action(Error); }
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public void Do(Action<TError> onError, Action onSuccess)
        {
            Require.NotNull(onError, nameof(onError));
            Require.NotNull(onSuccess, nameof(onSuccess));

            if (IsError)
            {
                onError(Error);
            }
            else
            {
                onSuccess();
            }
        }
    }

    // Implements the IEquatable<Result<TError>> interfaces.
    public partial struct Result<TError>
    {
        public static bool operator ==(Result<TError> left, Result<TError> right) => left.Equals(right);

        public static bool operator !=(Result<TError> left, Result<TError> right) => !left.Equals(right);

        public bool Equals(Result<TError> other) => Equals(other, EqualityComparer<TError>.Default);

        public bool Equals(Result<TError> other, IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSuccess) { return other.IsSuccess; }

            return other.IsError && comparer.Equals(Error, other.Error);
        }

        public override bool Equals(object obj) => Equals(obj, EqualityComparer<TError>.Default);

        public bool Equals(object other, IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is Result<TError>)) { return false; }

            return Equals((Result<TError>)other, comparer);
        }

        public override int GetHashCode() => GetHashCode(EqualityComparer<TError>.Default);

        public int GetHashCode(IEqualityComparer<TError> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return IsError ? comparer.GetHashCode(Error) : 0;
        }
    }
}

