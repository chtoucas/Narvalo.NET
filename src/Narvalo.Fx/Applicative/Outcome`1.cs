// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Applicative
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Narvalo.Properties;

    // Friendly version of Result<T, string>.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [DebuggerTypeProxy(typeof(Outcome<>.DebugView))]
    public partial struct Outcome<T>
        : IEquatable<Outcome<T>>, Internal.IEither<T, string>, Internal.Iterable<T>
    {
        private readonly string _error;
        private readonly T _value;

        private Outcome(T value)
        {
            _error = String.Empty;
            _value = value;
            IsSuccess = true;
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "isSuccess", Justification = "[Intentionally] Only added to disambiguate the two ctors when T = string.")]
        private Outcome(string error, bool isSuccess)
        {
            Debug.Assert(error != null);

            _error = error;
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
        internal T Value { get { Debug.Assert(IsSuccess); return _value; } }

        /// <summary>
        /// Obtains the captured exception state.
        /// </summary>
        /// <remarks>Any access to this method must be protected by checking before that
        /// <see cref="IsError"/> is true.</remarks>
        internal string Error { get { Debug.Assert(IsError); return _error; } }

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

        public T ValueOrThrow(Func<string, Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            if (IsError) { throw exceptionFactory(Error); }
            return Value;
        }

        public override string ToString()
            => IsSuccess ? "Success(" + Value?.ToString() + ")" : "Error(" + Error + ")";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Outcome{T}"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Outcome{T}.Value"/> and <see cref="Outcome{T}.Error"/>
        /// do not throw in the debugger for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Outcome<T> _inner;

            public DebugView(Outcome<T> inner) => _inner = inner;

            public bool IsSuccess => _inner.IsSuccess;

            public T Value => _inner._value;

            public string Error => _inner._error;
        }
    }

    // Conversions operators.
    public partial struct Outcome<T>
    {
        public T ToValue()
        {
            if (IsError) { throw new InvalidCastException(Strings_Fx.InvalidConversionToValue); }
            return Value;
        }

        public Maybe<T> ToMaybe() => ValueOrNone();

        public Result<T, string> ToResult()
            => IsError ? Result<T, string>.FromError(Error) : Result<T, string>.Of(Value);

        public static explicit operator T(Outcome<T> value) => value.ToValue();

        public static explicit operator Outcome<T>(T value) => η(value);
    }

    // Core Monad methods.
    public partial struct Outcome<T>
    {
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "[Intentionally] Raison d'être of this method.")]
        public Outcome<TResult> Bind<TResult>(Func<T, Outcome<TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsSuccess ? binder(Value) : Outcome<TResult>.FromError(Error);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T> η(T value) => new Outcome<T>(value);

        // NB: This method is normally internal, but Outcome<T>.FromError() is more readable
        // than Outcome.FromError<T>() - no type inference.
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Intentionally] A static method in a static class won't help.")]
        public static Outcome<T> FromError(string error)
        {
            Require.NotNullOrEmpty(error, nameof(error));

            return new Outcome<T>(error, false);
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Outcome<T> μ(Outcome<Outcome<T>> square)
            => square.IsSuccess ? square.Value : FromError(square.Error);
    }

    // Query Expression Pattern.
    public partial struct Outcome<T>
    {
        // Outcome<T> is not a MonadOr but we can still construct a Where() operator.
        // It is weird that it works (filter is not a predicate) but it does.
        public Outcome<T> Where(Func<T, Outcome> filter)
        {
            Require.NotNull(filter, nameof(filter));

            return Bind(val => filter(val).ReplaceBy(val));
        }
    }

    // Implements the Internal.IEither<T, string> interface.
    public partial struct Outcome<T>
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
        public TResult Match<TResult>(Func<T, TResult> caseSuccess, Func<string, TResult> caseError)
        {
            Require.NotNull(caseSuccess, nameof(caseSuccess));
            Require.NotNull(caseError, nameof(caseError));

            return IsSuccess ? caseSuccess(Value) : caseError(Error);
        }

        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "[Intentionally] Internal interface.")]
        [SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "1#", Justification = "[Intentionally] Internal interface.")]
        public void Do(Action<T> onSuccess, Action<string> onError)
        {
            Require.NotNull(onSuccess, nameof(onSuccess));
            Require.NotNull(onError, nameof(onError));

            if (IsSuccess) { onSuccess(Value); } else { onError(Error); }
        }

        public void WhenSuccess(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSuccess && predicate(Value)) { action(Value); }
        }

        public void WhenError(Func<string, bool> predicate, Action<string> action)
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

        public void OnError(Action<string> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsError) { action(Error); }
        }

        #region Publicly hidden methods.

        bool Internal.ISecondaryContainer<string>.Contains(string value)
            => throw new NotSupportedException();

        bool Internal.ISecondaryContainer<string>.Contains(string value, IEqualityComparer<string> comparer)
            => throw new NotSupportedException();

        // Alias for WhenSuccess().
        void Internal.IContainer<T>.When(Func<T, bool> predicate, Action<T> action)
           => WhenSuccess(predicate, action);

        // Alias for WhenError().
        void Internal.ISecondaryContainer<string>.When(Func<string, bool> predicate, Action<string> action)
            => WhenError(predicate, action);

        // Alias for OnSuccess().
        void Internal.IContainer<T>.Do(Action<T> action) => OnSuccess(action);

        // Alias for OnError().
        void Internal.ISecondaryContainer<string>.Do(Action<string> action)
            => OnError(action);

        #endregion
    }

    // Implements the Internal.Iterable<T> interface.
    public partial struct Outcome<T>
    {
        public IEnumerable<T> ToEnumerable() => IsSuccess ? Sequence.Of(Value) : Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();
    }

    // Implements the IEquatable<Outcome<T>> interface.
    public partial struct Outcome<T>
    {
        public static bool operator ==(Outcome<T> left, Outcome<T> right) => left.Equals(right);

        public static bool operator !=(Outcome<T> left, Outcome<T> right) => !left.Equals(right);

        public bool Equals(Outcome<T> other)
        {
            if (IsSuccess) { return other.IsSuccess && EqualityComparer<T>.Default.Equals(Value, other.Value); }
            return other.IsError && Error == other.Error;
        }

        public bool Equals(Outcome<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSuccess) { return other.IsSuccess && comparer.Equals(Value, other.Value); }
            return other.IsError && Error == other.Error;
        }

        public override bool Equals(object obj)
            => (obj is Outcome<T>) && Equals((Outcome<T>)obj);

        public bool Equals(object other, IEqualityComparer<T> comparer)
            => (other is Outcome<T>) && Equals((Outcome<T>)other, comparer);

        public override int GetHashCode()
            // NB: _error is never null.
            => HashCodeHelpers.Combine(_value?.GetHashCode() ?? 0, _error.GetHashCode());

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            // NB: _error is never null.
            return HashCodeHelpers.Combine(comparer.GetHashCode(_value), _error.GetHashCode());
        }
    }
}
