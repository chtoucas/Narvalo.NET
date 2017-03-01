// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result<,>.DebugView))]
    public partial struct Result<T, TError>
        : IEquatable<Result<T, TError>>, Internal.IEither<T, TError>, Internal.Iterable<T>
    {
        private readonly T _value;
        private readonly TError _error;

        private Result(T value, TError error, bool isSuccess)
        {
            _error = error;
            _value = value;
            IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }

        public bool IsError => !IsSuccess;

        internal T Value { get { Demand.State(IsSuccess); return _value; } }

        internal TError Error { get { Demand.State(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        public T ValueOrDefault() => IsSuccess ? Value : default(T);

        public T ValueOrElse(T other) => IsSuccess ? Value : other;

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));
            return IsSuccess ? Value : valueFactory();
        }

        public Maybe<T> ValueOrNone() => IsSuccess ? Maybe.Of(Value) : Maybe<T>.None;

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            if (IsError) { throw exception; }

            return Value;
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            if (IsError) { throw exceptionFactory(); }

            return Value;
        }

        public override string ToString()
            => IsSuccess ? Format.Current("Success({0})", Value) : Format.Current("Error({0})", Error);

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result{T, TError}"/>.
        /// </summary>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result<T, TError> _inner;

            public DebugView(Result<T, TError> inner)
            {
                _inner = inner;
            }

            public bool IsError => _inner.IsError;

            public TError Error => _inner.Error;

            public T Value => _inner.Value;
        }
    }

    // Conversion operators.
    public partial struct Result<T, TError>
    {
        public T ToValue()
        {
            if (IsError) { throw new InvalidCastException("XXX"); }
            return Value;
        }

        public TError ToError()
        {
            if (IsSuccess) { throw new InvalidCastException("XXX"); }
            return Error;
        }

        // NB: In Haskell, the error is the left type parameter.
        public Either<T, TError> ToEither()
            => IsSuccess ? Either.OfLeft<T, TError>(Value) : Either.OfRight<T, TError>(Error);

        public static explicit operator T(Result<T, TError> value) => value.ToValue();

        public static explicit operator TError(Result<T, TError> value) => value.ToError();

        public static explicit operator Result<T, TError>(T value) => Result.Of<T, TError>(value);

        public static explicit operator Result<T, TError>(TError error) => Result.FromError<T, TError>(error);
    }

    // Provides the core Monad methods.
    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> Bind<TResult>(Func<T, Result<TResult, TError>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSuccess ? selector(Value) : Result.FromError<TResult, TError>(Error);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> η(T value) => new Result<T, TError>(value, default(TError), true);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> FromError(TError error) => new Result<T, TError>(default(T), error, false);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> μ(Result<Result<T, TError>, TError> square)
            => square.IsSuccess ? square.Value : Result.FromError<T, TError>(square.Error);
    }

    // Implements the Internal.IEither<T, TError> interface.
    public partial struct Result<T, TError>
    {
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TError, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess(Value) : caseError(Error);
        }

        // Alias for WhenSuccess().
        // NB: We keep this one public as it overrides the auto-generated method.
        public void When(Func<T, bool> predicate, Action<T> action)
            => WhenSuccess(predicate, action);

        // Alias for WhenError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.When(
            Func<TError, bool> predicate,
            Action<TError> action)
            => WhenError(predicate, action);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public void Do(Action<T> onSuccess, Action<TError> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess)
            {
                onSuccess(Value);
            }
            else
            {
                onError(Error);
            }
        }

        // Alias for OnSuccess(). Publicly hidden.
        void Internal.IContainer<T>.Do(Action<T> onSuccess) => OnSuccess(onSuccess);

        // Alias for OnError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.Do(Action<TError> onError) => OnError(onError);

        public void WhenSuccess(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSuccess && predicate(Value)) { action(Value); }
        }

        public void WhenError(Func<TError, bool> predicate, Action<TError> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsError && predicate(Error)) { action(Error); }
        }

        public void OnSuccess(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(Value); }
        }

        public void OnError(Action<TError> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error); }
        }
    }

    // Implements the Internal.Iterable<T> interface.
    public partial struct Result<T, TError>
    {
        public IEnumerable<T> ToEnumerable() => IsSuccess ? Sequence.Of(Value) : Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Result<T, TError>> interfaces.
    public partial struct Result<T, TError>
    {
        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) => left.Equals(right);

        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) => !left.Equals(right);

        public bool Equals(Result<T, TError> other)
            => Equals(other, EqualityComparer<T>.Default, EqualityComparer<TError>.Default);

        public bool Equals(
            Result<T, TError> other,
            IEqualityComparer<T> comparer,
            IEqualityComparer<TError> errComparer)
        {
            Require.NotNull(comparer, nameof(comparer));
            Require.NotNull(errComparer, nameof(errComparer));

            if (IsSuccess) { return other.IsSuccess && comparer.Equals(Value, other.Value); }

            return other.IsError && errComparer.Equals(Error, other.Error);
        }

        public override bool Equals(object obj)
            => Equals(obj, EqualityComparer<T>.Default, EqualityComparer<TError>.Default);

        public bool Equals(
            object other,
            IEqualityComparer<T> comparer,
            IEqualityComparer<TError> errComparer)
        {
            Require.NotNull(comparer, nameof(comparer));
            Require.NotNull(errComparer, nameof(errComparer));

            if (!(other is Result<T, TError>)) { return false; }

            return Equals((Result<T, TError>)other, comparer, errComparer);
        }

        public override int GetHashCode()
            => GetHashCode(EqualityComparer<T>.Default, EqualityComparer<TError>.Default);

        public int GetHashCode(IEqualityComparer<T> comparer, IEqualityComparer<TError> errComparer)
        {
            Require.NotNull(comparer, nameof(comparer));
            Require.NotNull(errComparer, nameof(errComparer));

            unchecked
            {
                int hash = 17;
                hash = 31 * hash + IsSuccess.GetHashCode();
                hash = 31 * hash + (IsSuccess ? comparer.GetHashCode(Value) : errComparer.GetHashCode(Error));
                return hash;
            }
        }
    }
}
