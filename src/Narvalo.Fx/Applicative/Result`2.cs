﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Narvalo.Linq;
    using Narvalo.Properties;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result<,>.DebugView))]
    public partial struct Result<T, TError>
        : IEquatable<Result<T, TError>>, IStructuralEquatable, Internal.IResult<T, TError>, Internal.Iterable<T>
    {
        private readonly T _value;
        private readonly TError _error;

        private Result(T value)
        {
            _error = default(TError);
            _value = value;
            IsError = false;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "isSuccess", Justification = "[Intentionally] Only added to disambiguate the two ctors when T = TError.")]
        private Result(TError error, bool isSuccess)
        {
            _error = error;
            _value = default(T);
            IsError = true;
        }

        public void Deconstruct(out bool succeed, out T value, out TError error)
        {
            succeed = IsSuccess;
            value = _value;
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

        /// <summary>
        /// Obtains the enclosed value if any.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsSuccess"/> is true.</remarks>
        internal T Value { get { Debug.Assert(IsSuccess); return _value; } }

        /// <summary>
        /// Obtains the enclosed error if any.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal TError Error { get { Debug.Assert(IsError); return _error; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsError ? "Error" : "Success";

        /// <summary>
        /// Obtains the enclosed value if any; otherwise the default value of the type T.
        /// </summary>
        public T ValueOrDefault() => _value;

        public Maybe<T> ValueOrNone() => IsError ? Maybe<T>.None : Maybe.Of(Value);

        /// <summary>
        /// Returns the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        public T ValueOrElse(T other) => IsError ? other : Value;

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));

            return IsError ? valueFactory() : Value;
        }

        public T ValueOrThrow(Func<TError, Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            return IsError ? throw exceptionFactory(Error) : Value;
        }

        public override string ToString()
            => IsError ? "Error(" + Error.ToString() + ")" : "Success(" + Value?.ToString() + ")";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result{T, TError}"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Result{T, TError}.Value"/> and
        /// <see cref="Result{T, TError}.Error"/> do not throw in the debugger for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result<T, TError> _inner;

            public DebugView(Result<T, TError> inner) => _inner = inner;

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
            if (IsError) { throw new InvalidCastException(Strings.InvalidConversionToValue); }
            return Value;
        }

        public TError ToError()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidConversionToError); }
            return Error;
        }

        public Maybe<T> ToMaybe() => ValueOrNone();

        // NB: In Haskell, the error is the left parameter.
        public Either<T, TError> ToEither()
            => IsError ? Either<T, TError>.OfRight(Error) : Either<T, TError>.OfLeft(Value);

        public static explicit operator T(Result<T, TError> value) => value.ToValue();

        public static explicit operator TError(Result<T, TError> value) => value.ToError();

        public static explicit operator Result<T, TError>(T value) => Of(value);

        public static explicit operator Result<T, TError>(TError error) => FromError(error);
    }

    // Provides the core Monad methods.
    public partial struct Result<T, TError>
    {
        public Result<TResult, TError> Bind<TResult>(Func<T, Result<TResult, TError>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsError ? Result<TResult, TError>.FromError(Error) : binder(Value);
        }

        // NB: This method is normally internal, but Result<T, TError>.Of() is more readable
        // than Result.Of<T, TError>() - no type inference.
        // See also Result.OfTError<TError>.Of(T);
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
        public static Result<T, TError> Of(T value) => new Result<T, TError>(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> μ(Result<Result<T, TError>, TError> square)
            => square.IsError ? FromError(square.Error) : square.Value;
    }

    // Minimalist implementation of a monad on TError.
    public partial struct Result<T, TError>
    {
        public Result<T, TResult> BindError<TResult>(Func<TError, Result<T, TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsError ? binder(Error) : Result<T, TResult>.Of(Value);
        }

        public Result<T, TResult> SelectError<TResult>(Func<TError, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsError ? Result<T, TResult>.FromError(selector(Error)) : Result<T, TResult>.Of(Value);
        }

        // NB: This method is normally internal, but Result<T, TError>.FromError() is more readable
        // than Result.FromError<T, TError>() - no type inference.
        // See also Result.OfType<T>.FromError(TError);
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
        public static Result<T, TError> FromError(TError error)
        {
            Require.NotNullUnconstrained(error, nameof(error));

            return new Result<T, TError>(error, false);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T, TError> FlattenError(Result<T, Result<T, TError>> square)
            => square.IsError ? square.Error : Of(square.Value);
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
            if (IsError) { return false; }
            return (comparer ?? EqualityComparer<T>.Default).Equals(Value, value);
        }

        public TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<TError, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsError ? caseError(Error) : caseSuccess(Value);
        }

        public void Do(Action<T> onSuccess, Action<TError> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsError) { onError(Error); } else { onSuccess(Value); }
        }

        public bool OnSuccess(Action<T> action)
        {
            Require.NotNull(action, nameof(action));
            if (IsSuccess) { action(Value); return true; }
            return false;
        }

        public bool OnError(Action<TError> action)
        {
            Require.NotNull(action, nameof(action));
            if (IsError) { action(Error); return true; }
            return false;
        }

        #region Publicly hidden methods.

        [ExcludeFromCodeCoverage]
        bool Internal.ISecondaryContainer<TError>.Contains(TError value)
            => throw new NotSupportedException();

        [ExcludeFromCodeCoverage]
        bool Internal.ISecondaryContainer<TError>.Contains(TError value, IEqualityComparer<TError> comparer)
            => throw new NotSupportedException();

        // Alias for OnSuccess().
        [ExcludeFromCodeCoverage]
        bool Internal.IContainer<T>.Do(Action<T> action) => OnSuccess(action);

        // Alias for OnError().
        [ExcludeFromCodeCoverage]
        bool Internal.ISecondaryContainer<TError>.Do(Action<TError> action) => OnError(action);

        #endregion
    }

    // Implements the Internal.Iterable<T> interface.
    public partial struct Result<T, TError>
    {
        public IEnumerable<T> ToEnumerable() => IsError ? Enumerable.Empty<T>() : Sequence.Return(Value);

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Result<T, TError>> and IStructuralEquatable interfaces.
    public partial struct Result<T, TError>
    {
        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) => left.Equals(right);

        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) => !left.Equals(right);

        public bool Equals(Result<T, TError> other)
        {
            if (IsError) { return other.IsError && EqualityComparer<TError>.Default.Equals(Error, other.Error); }
            return other.IsSuccess && EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
            => (obj is Result<T, TError>) && Equals((Result<T, TError>)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(_value?.GetHashCode() ?? 0, _error?.GetHashCode() ?? 0);

        bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (ReferenceEquals(other, null) || !(other is Result<T, TError>)) { return false; }

            var obj = (Result<T, TError>)other;

            if (IsError) { return obj.IsError && comparer.Equals(Error, obj.Error); }
            return obj.IsSuccess && comparer.Equals(Value, obj.Value);
        }

        int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return HashCodeHelpers.Combine(comparer.GetHashCode(_value), comparer.GetHashCode(_error));
        }
    }
}
