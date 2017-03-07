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

    /// <summary>
    /// Represents an object that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    [DebuggerDisplay("IsSome = {IsSome}")]
    [DebuggerTypeProxy(typeof(Maybe<>.DebugView))]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[Intentionally] Maybe<T> only pretends to be a collection.")]
    public partial struct Maybe<T> : IEquatable<Maybe<T>>, Internal.IMaybe<T>
    {
        // You should NEVER use this field directly, use the Value property instead. The Code Contracts
        // static checker should then prove that no illegal access to this field happens (i.e. when IsSome is false).
        private readonly T _value;

        /// <summary>
        /// Prevents a default instance of the <see cref="Maybe{T}" /> class from being created outside.
        /// Initializes a new instance of the <see cref="Maybe{T}" /> class for the specified value.
        /// </summary>
        /// <param name="value">A value to wrap.</param>
        /// <seealso cref="Maybe.Of{T}(T)"/>
        /// <seealso cref="Maybe.Of{T}(T?)"/>
        private Maybe(T value)
        {
            Demand.NotNullUnconstrained(value);

            _value = value;
            IsSome = true;
        }

        /// <summary>
        /// Gets a value indicating whether the object does hold a value.
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value>true if the object does hold a value; otherwise false.</value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSome { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNone => !IsSome;

        /// <summary>
        /// Gets the enclosed value.
        /// </summary>
        /// <remarks>Any access to this property must be protected by checking before that
        /// <see cref="IsSome"/> is true.</remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal T Value { get { Demand.State(IsSome); return _value; } }

        /// <summary>
        /// Obtains the enclosed value if any; otherwise the default value of type T.
        /// </summary>
        /// <returns>The enclosed value if any; otherwise the default value of type T.</returns>
        public T ValueOrDefault() => IsSome ? Value : default(T);

        /// <summary>
        /// Obtains the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="other"/>.</returns>
        public T ValueOrElse(T other)
        {
            Require.NotNullUnconstrained(other, nameof(other));

            return IsSome ? Value : other;
        }

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));

            return IsSome ? Value : valueFactory();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, nameof(exception));

            throw exception;
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));

            if (IsNone)
            {
                throw exceptionFactory();
            }

            return Value;
        }

        public override string ToString() => IsSome ? "Maybe(" + Value.ToString() + ")" : "Maybe(None)";

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Maybe{T}"/>.
        /// </summary>
        /// <remarks>Ensure that <see cref="Maybe{T}.Value"/> does not throw in the debugger
        /// for DEBUG builds.</remarks>
        [ExcludeFromCodeCoverage]
        private sealed class DebugView
        {
            private readonly Maybe<T> _inner;

            public DebugView(Maybe<T> inner)
            {
                _inner = inner;
            }

            public bool IsSome => _inner.IsSome;

            public T Value => _inner._value;
        }
    }

    // Conversion operators.
    public partial struct Maybe<T>
    {
        public static explicit operator Maybe<T>(T value) => η(value);

        public static explicit operator T(Maybe<T> value)
        {
            if (value.IsNone)
            {
                throw new InvalidCastException(Strings.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }
    }

    // Provides the core Monad methods.
    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> binder)
        {
            Require.NotNull(binder, nameof(binder));

            return IsSome ? binder(Value) : Maybe<TResult>.None;
        }

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Maybe<T> η(T value) => value != null ? new Maybe<T>(value) : None;

        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square) => square.IsSome ? square.Value : None;
    }

    // Provides the core MonadOr methods.
    public partial struct Maybe<T>
    {
        private static readonly Maybe<T> s_None = new Maybe<T>();

        /// <summary>
        /// Obtains an instance of <see cref="Maybe{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Maybe<T> None => s_None;

        public Maybe<T> OrElse(Maybe<T> other) => IsNone ? other : this;
    }

    // Implements the Internal.IMaybe<T> interface.
    public partial struct Maybe<T>
    {
        public IEnumerable<T> ToEnumerable() => IsSome ? Sequence.Of(Value) : Enumerable.Empty<T>();

        public IEnumerator<T> GetEnumerator() => ToEnumerable().GetEnumerator();

        public bool Contains(T value)
        {
            if (IsNone) { return false; }
            return EqualityComparer<T>.Default.Equals(Value, value);
        }

        public bool Contains(T value, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsNone) { return false; }
            return comparer.Equals(Value, value);
        }

        public TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));
            Require.NotNull(caseNone, nameof(caseNone));

            return IsSome ? caseSome(Value) : caseNone();
        }

        public TResult Match<TResult>(Func<T, TResult> caseSome, TResult caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));

            return IsSome ? caseSome(Value) : caseNone;
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return IsSome && predicate(Value) ? selector(Value) : otherwise();
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate(Value) ? thenResult : elseResult;
        }

        public void When(Func<T, bool> predicate, Action<T> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (IsSome && predicate(Value)) { action(Value); } else { otherwise(); }
        }

        public void When(Func<T, bool> predicate, Action<T> action)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));

            if (IsSome && predicate(Value)) { action(Value); }
        }

        public void Do(Action<T> onSome, Action onNone)
        {
            Require.NotNull(onSome, nameof(onSome));
            Require.NotNull(onNone, nameof(onNone));

            if (IsSome) { onSome(Value); } else { onNone(); }
        }

        public void OnSome(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSome) { action(Value); }
        }

        public void OnNone(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsNone) { action(); }
        }

        // Alias for OnSome(). Publicly hidden.
        void Internal.IContainer<T>.Do(Action<T> action) => OnSome(action);
    }

    // Implements the IEquatable<Maybe<T>> interface.
    public partial struct Maybe<T>
    {
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        public bool Equals(Maybe<T> other)
        {
            if (IsSome) { return other.IsSome && EqualityComparer<T>.Default.Equals(Value, other.Value); }
            return other.IsNone;
        }

        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSome) { return other.IsSome && comparer.Equals(Value, other.Value); }
            return other.IsNone;
        }

        public override bool Equals(object obj)
            => (obj is Maybe<T>) && Equals((Maybe<T>)obj);

        public bool Equals(object other, IEqualityComparer<T> comparer)
            => (other is Maybe<T>) && Equals((Maybe<T>)other, comparer);

        public override int GetHashCode() => _value?.GetHashCode() ?? 0;

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return IsSome ? comparer.GetHashCode(Value) : 0;
        }
    }
}
