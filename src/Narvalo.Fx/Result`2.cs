﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Narvalo.Fx.Properties;

    using HashHelpers = Narvalo.Fx.Internal.HashHelpers;

    // Typical use case:
    // - Result<T, string> for lightweight error reporting to the caller;
    //   think of it as a verbose Maybe<T>.
    // Few recommendations:
    // - For methods with a void return type, prefer Result or Maybe<TError>.
    // - Result<T, Exception> should be used only in very rare situations; this is **not** a
    //   replacement for the standard exception mechanism in .NET. See Result and Result<T>
    //   for better alternatives.
    // - Result<T, TError> is a value type; for long-lived objects prefer Either<T, TError>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result<,>.DebugView))]
    public partial struct Result<T, TError>
        : IEquatable<Result<T, TError>>, IStructuralEquatable, Internal.IEither<T, TError>, Internal.Iterable<T>
    {
        private readonly T _value;
        private readonly TError _error;

        private Result(T value, TError error, bool isSuccess)
        {
            _error = error;
            _value = value;
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// Gets a value indicating whether the object is the result of a successful computation.
        /// </summary>
        /// <value>true if the outcome was successful; otherwise false.</value>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the object is the result of an unsuccessful computation.
        /// </summary>
        /// <value>true if the outcome was unsuccessful; otherwise false.</value>
        public bool IsError => !IsSuccess;

        /// <summary>
        /// Obtains the enclosed value if any.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsSuccess"/> is true.</remarks>
        internal T Value { get { Demand.State(IsSuccess); return _value; } }

        /// <summary>
        /// Obtains the enclosed error if any.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal TError Error { get { Demand.State(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        /// <summary>
        /// Obtains the enclosed value if any; otherwise the default value of the type T.
        /// </summary>
        public T ValueOrDefault() => _value;

        public Maybe<T> ValueOrNone() => IsSuccess ? Maybe.Of(Value) : Maybe<T>.None;

        /// <summary>
        /// Returns the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        public T ValueOrElse(T other) => IsSuccess ? Value : other;

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));
            return IsSuccess ? Value : valueFactory();
        }

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
        /// <remarks>Ensure that <see cref="Result{T, TError}.Value"/> and
        /// <see cref="Result{T, TError}.Error"/> do not throw in the debugger for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result<T, TError> _inner;

            public DebugView(Result<T, TError> inner)
            {
                _inner = inner;
            }

            public bool IsSuccess => _inner.IsSuccess;

            public TError Error => _inner._error;

            public T Value => _inner._value;
        }
    }

    // Conversion operators.
    public partial struct Result<T, TError>
    {
        public T ToValue()
        {
            if (IsError) { throw new InvalidCastException(Strings.InvalidCast_ToSuccess); }
            return Value;
        }

        public TError ToError()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidCast_ToError); }
            return Error;
        }

        public Maybe<T> ToMaybe() => ValueOrNone();

        // NB: In Haskell, the error is the left parameter.
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
        internal static Result<T, TError> μ(Result<Result<T, TError>, TError> square)
            => square.IsSuccess ? square.Value : Result.FromError<T, TError>(square.Error);
    }

    // Minimalist implementation of a monad on TError.
    public partial struct Result<T, TError>
    {
        public Result<T, TResult> BindError<TResult>(Func<TError, Result<T, TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsError ? selector(Error) : Result.Of<T, TResult>(Value);
        }

        public Result<T, TResult> SelectError<TResult>(Func<TError, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsError ? Result.FromError<T, TResult>(selector(Error)) : Result.Of<T, TResult>(Value);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> FromError(TError error)
        {
            Require.NotNullUnconstrained(error, nameof(error));

            return new Result<T, TError>(default(T), error, false);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> FlattenError(Result<T, Result<T, TError>> square)
            => square.IsError ? square.Error : Result.Of<T, TError>(square.Value);
    }

    // Implements the Internal.IEither<T, TError> interface.
    public partial struct Result<T, TError>
    {
        public bool Contains(T value)
        {
            if (IsError) { return false; }
            return EqualityComparer<T>.Default.Equals(Value, value);
        }

        public bool Contains(T value, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsError) { return false; }
            return comparer.Equals(Value, value);
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TError, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess(Value) : caseError(Error);
        }

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

        // Publicly hidden.
        bool Internal.ISecondaryContainer<TError>.Contains(TError value)
        {
            throw new NotSupportedException("XXX");
        }

        // Publicly hidden.
        bool Internal.ISecondaryContainer<TError>.Contains(TError value, IEqualityComparer<TError> comparer)
        {
            throw new NotSupportedException("XXX");
        }

        // Alias for WhenSuccess(). Publicly hidden.
        void Internal.IContainer<T>.When(Func<T, bool> predicate, Action<T> action)
           => WhenSuccess(predicate, action);

        // Alias for WhenError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.When(
            Func<TError, bool> predicate,
            Action<TError> action)
            => WhenError(predicate, action);

        // Alias for OnSuccess(). Publicly hidden.
        void Internal.IContainer<T>.Do(Action<T> action) => OnSuccess(action);

        // Alias for OnError(). Publicly hidden.
        void Internal.ISecondaryContainer<TError>.Do(Action<TError> action) => OnError(action);
    }

    // Implements the Internal.Iterable<T> interface.
    public partial struct Result<T, TError>
    {
        public IEnumerable<T> ToEnumerable() => IsSuccess ? Sequence.Of(Value) : Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Result<T, TError>> and IStructuralEquatable interfaces.
    public partial struct Result<T, TError>
    {
        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) => left.Equals(right);

        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) => !left.Equals(right);

        public bool Equals(Result<T, TError> other)
        {
            if (IsSuccess) { return other.IsSuccess && EqualityComparer<T>.Default.Equals(Value, other.Value); }
            return other.IsError && EqualityComparer<TError>.Default.Equals(Error, other.Error);
        }

        public override bool Equals(object obj)
            => (obj is Result<T, TError>) && Equals((Result<T, TError>)obj);

        public override int GetHashCode()
            => HashHelpers.Combine(_value?.GetHashCode() ?? 0, _error?.GetHashCode() ?? 0);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null) || !(other is Result<T, TError>)) { return false; }

            var obj = (Result<T, TError>)other;

            if (IsSuccess) { return obj.IsSuccess && comparer.Equals(Value, obj.Value); }
            return obj.IsError && comparer.Equals(Error, obj.Error);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashHelpers.Combine(comparer.GetHashCode(_value), comparer.GetHashCode(_error));
        }
    }
}
