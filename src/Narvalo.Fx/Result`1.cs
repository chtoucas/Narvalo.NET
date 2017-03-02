// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;

    using Narvalo.Fx.Properties;

    /// <summary>
    /// Represents the outcome of a computation which might have thrown an exception.
    /// An instance of the <see cref="Result{T}"/> class contains either a <c>T</c>
    /// value or the exception state at the point it was thrown.
    /// </summary>
    /// <remarks>
    /// <para>We do not catch exceptions throw by any supplied delegate; there is only one exception
    /// though: <see cref="Result{T}.Select{TResult}(Func{T, TResult})"/>. A good pratice is that
    /// a function that returns a <see cref="Result{T}"/> does not normally throw.</para>
    /// <para>This class is not meant to replace the standard exception mechanism.</para>
    /// </remarks>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    // Friendly version of Either<T, ExceptionDispatchInfo>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Result<>.DebugView))]
    public partial struct Result<T>
        : IEquatable<Result<T>>, Internal.IEither<T, ExceptionDispatchInfo>, Internal.Iterable<T>
    {
        private readonly ExceptionDispatchInfo _exceptionInfo;
        private readonly T _value;

        private Result(T value)
        {
            _exceptionInfo = default(ExceptionDispatchInfo);
            _value = value;
            IsSuccess = true;
        }

        private Result(ExceptionDispatchInfo exceptionInfo)
        {
            Demand.NotNull(exceptionInfo);

            _exceptionInfo = exceptionInfo;
            _value = default(T);
            IsSuccess = false;
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
        /// Obtains the captured exception state.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal ExceptionDispatchInfo ExceptionInfo { get { Demand.State(IsError); return _exceptionInfo; } }

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => IsSuccess ? "Success" : "Error";

        public void ThrowIfError()
        {
            if (IsError) { ExceptionInfo.Throw(); }
        }

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

        public T ValueOrThrow()
        {
            if (IsError) { ExceptionInfo.Throw(); }
            return Value;
        }

        public override string ToString()
            => IsSuccess ? Format.Current("Success({0})", Value) : Format.Current("Error({0})", ExceptionInfo);

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Result{T}"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Result{T}.Value"/> and <see cref="Result{T}.ExceptionInfo"/>
        /// do not throw in the debugger for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Result<T> _inner;

            public DebugView(Result<T> inner)
            {
                _inner = inner;
            }

            public bool IsSuccess => _inner.IsSuccess;

            public T Value => _inner._value;

            public ExceptionDispatchInfo ExceptionInfo => _inner._exceptionInfo;
        }
    }

    // Conversions operators.
    public partial struct Result<T>
    {
        public T ToValue()
        {
            if (IsError) { throw new InvalidCastException(Strings.InvalidCast_ToSuccess); }
            return Value;
        }

        public ExceptionDispatchInfo ToExceptionInfo()
        {
            if (IsSuccess) { throw new InvalidCastException(Strings.InvalidCast_ToError); }
            return ExceptionInfo;
        }

        public Maybe<T> ToMaybe() => ValueOrNone();

        public static explicit operator T(Result<T> value) => value.ToValue();

        public static explicit operator ExceptionDispatchInfo(Result<T> value) => value.ToExceptionInfo();

        public static explicit operator Result<T>(T value) => Result.Of(value);
    }

    // Core Monad methods.
    public partial struct Result<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            if (IsError) { return Result.FromError<TResult>(ExceptionInfo); }

            try
            {
                return selector(Value);
            }
            catch (Exception ex)
            {
                var edi = ExceptionDispatchInfo.Capture(ex);
                return Result.FromError<TResult>(edi);
            }
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T> η(T value) => new Result<T>(value);

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T> FromError(ExceptionDispatchInfo exceptionInfo)
        {
            Require.NotNull(exceptionInfo, nameof(exceptionInfo));

            return new Result<T>(exceptionInfo);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Result<T> μ(Result<Result<T>> square)
            => square.IsSuccess ? square.Value : Result.FromError<T>(square.ExceptionInfo);
    }

    // Implements the Internal.IEither<T, ExceptionDispatchInfo> interface.
    public partial struct Result<T>
    {
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public TResult Match<TResult>(
            Func<T, TResult> caseSuccess,
            Func<ExceptionDispatchInfo, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess(Value) : caseError(ExceptionInfo);
        }

        // Alias for WhenSuccess().
        // NB: We keep this one public as it overrides the auto-generated method.
        public void When(Func<T, bool> predicate, Action<T> action)
            => WhenSuccess(predicate, action);

        // Alias for WhenError(). Publicly hidden.
        void Internal.ISecondaryContainer<ExceptionDispatchInfo>.When(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action)
            => WhenError(predicate, action);

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public void Do(Action<T> onSuccess, Action<ExceptionDispatchInfo> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(OnError));

            if (IsSuccess)
            {
                onSuccess(Value);
            }
            else
            {
                onError(ExceptionInfo);
            }
        }

        // Alias for OnSuccess(). Publicly hidden.
        void Internal.IContainer<T>.Do(Action<T> onSuccess) => OnSuccess(onSuccess);

        // Alias for OnError(). Publicly hidden.
        void Internal.ISecondaryContainer<ExceptionDispatchInfo>.Do(Action<ExceptionDispatchInfo> onError)
            => OnError(onError);

        public void WhenSuccess(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSuccess && predicate(Value)) { action(Value); }
        }

        public void WhenError(
            Func<ExceptionDispatchInfo, bool> predicate,
            Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsError && predicate(ExceptionInfo)) { action(ExceptionInfo); }
        }

        public void OnSuccess(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSuccess) { action(Value); }
        }

        public void OnError(Action<ExceptionDispatchInfo> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(ExceptionInfo); }
        }
    }

    // Implements the Internal.Iterable<T> interface.
    public partial struct Result<T>
    {
        public IEnumerable<T> ToEnumerable() => IsSuccess ? Sequence.Of(Value) : Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Result<T>> interface.
    public partial struct Result<T>
    {
        public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

        public static bool operator !=(Result<T> left, Result<T> right) => !left.Equals(right);

        public bool Equals(Result<T> other)
        {
            if (IsSuccess) { return other.IsSuccess && EqualityComparer<T>.Default.Equals(Value, other.Value); }
            return other.IsError && ReferenceEquals(ExceptionInfo, other.ExceptionInfo);
        }

        public bool Equals(Result<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSuccess) { return other.IsSuccess && comparer.Equals(Value, other.Value); }
            return other.IsError && ReferenceEquals(ExceptionInfo, other.ExceptionInfo);
        }

        public override bool Equals(object obj)
            => (obj is Result<T>) && Equals((Result<T>)obj);

        public bool Equals(object other, IEqualityComparer<T> comparer)
            => (other is Result<T>) && Equals((Result<T>)other, comparer);

        public override int GetHashCode()
            => IsSuccess ? Value.GetHashCode() : ExceptionInfo.GetHashCode();

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return IsSuccess ? comparer.GetHashCode(Value) : ExceptionInfo.GetHashCode();
        }
    }
}
