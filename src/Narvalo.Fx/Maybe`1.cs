// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx.Properties;

    /// <summary>
    /// Represents an object that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    [DebuggerDisplay("IsSome = {IsSome}")]
    [DebuggerTypeProxy(typeof(Maybe<>.DebugView))]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[Intentionally] Maybe<T> only pretends to be a collection.")]
    public partial struct Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, Internal.ISwitch<T>
    {
        private readonly bool _isSome;

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
            _isSome = true;
        }

        /// <summary>
        /// Gets a value indicating whether the object does hold a value.
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value><see langword="true"/> if the object does hold a value; otherwise false.</value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSome { get { return _isSome; } }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNone => !IsSome;

        /// <summary>
        /// Gets the enclosed value.
        /// </summary>
        /// <remarks>
        /// Any access to this property must be protected by checking before that <see cref="IsSome"/>
        /// is true.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal T Value
        {
            get
            {
                Demand.State(IsSome);
                Warrant.NotNullUnconstrained<T>();

#if CONTRACTS_FULL // Helps CCCheck with the object invariance.

                if (_value == null)
                {
                    // If IsSome is true, this can never happen but we keep this around because we want
                    // that the Code Contracts static checker proves that we never call this property
                    // when IsSome is false. Without Code Contracts, we are better off disabling this.
                    // Furthermore, not throwing the exception means that we can use this property safely,
                    // for instance inside the GetHashCode() method.
                    // Remember that in our particular setup the Code Contracts symbol does not exist in any
                    // build except when we do run the Code Contracts tools, which implies that this particular
                    // piece of code will never execute at runtime.
                    throw new InvalidOperationException();
                }

#endif

                return _value;
            }
        }

        #region Conversion operators

        public static explicit operator Maybe<T>(T value) => η(value);

        public static explicit operator T(Maybe<T> value)
        {
            Warrant.NotNullUnconstrained<T>();

            if (value.IsNone)
            {
                throw new InvalidCastException(Strings.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }

        #endregion

        /// <summary>
        /// Obtains the enclosed value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The enclosed value if any; otherwise the default value of the type T.</returns>
        public T ValueOrDefault() => IsSome ? Value : default(T);

        /// <summary>
        /// Obtains the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="other"/>.</returns>
        public T ValueOrElse(T other)
        {
            Require.NotNullUnconstrained(other, nameof(other));
            Warrant.NotNullUnconstrained<T>();

            return IsSome ? Value : other;
        }

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, nameof(valueFactory));

            return IsSome ? Value : valueFactory.Invoke();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, nameof(exception));
            Warrant.NotNullUnconstrained<T>();

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));
            Warrant.NotNullUnconstrained<T>();

            if (IsNone)
            {
                throw exceptionFactory.Invoke();
            }

            return Value;
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return IsSome ? Format.Current("Maybe({0})", Value) : "Maybe(None)";
        }

        /// <summary>
        /// Represents a debugger type proxy for <see cref="Maybe{T}"/>.
        /// </summary>
        [ContractVerification(false)] // Debugger-only code.
        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        private sealed class DebugView
        {
            private readonly Maybe<T> _inner;

            public DebugView(Maybe<T> inner)
            {
                _inner = inner;
            }

            public bool IsSome { get { return _inner.IsSome; } }

            public T Value
            {
                get
                {
                    if (!IsSome)
                    {
                        return default(T);
                    }

                    return _inner.Value;
                }
            }
        }
    }

    // Overrides for auto-generated (extension) methods.
    public partial struct Maybe<T>
    {
        #region Basic Monad functions

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? Maybe<TResult>.η(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
            => IsSome ? other : Maybe<TResult>.None;

        #endregion

        #region Monadic lifting operators

        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(resultSelector, nameof(resultSelector));

            return IsSome && second.IsSome
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #endregion

        #region LINQ extensions

        public Maybe<TResult> Join<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            if (IsNone || inner.IsNone) { return Maybe<TResult>.None; }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner.Value))
                : Maybe<TResult>.None;
        }

        public Maybe<TResult> GroupJoin<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, nameof(outerKeySelector));
            Require.NotNull(innerKeySelector, nameof(innerKeySelector));
            Require.NotNull(resultSelector, nameof(resultSelector));

            // REVIEW: I can't remember why I didn't include !inner.IsSome before. Mistake?
            if (IsNone || inner.IsNone) { return Maybe<TResult>.None; }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner))
                : Maybe<TResult>.None;
        }

        #endregion
    }

    // Provides the core Monad methods.
    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selectorM)
        {
            Require.NotNull(selectorM, nameof(selectorM));

            return IsSome ? selectorM.Invoke(Value) : Maybe<TResult>.None;
        }

        [DebuggerHidden]
        internal static Maybe<T> η(T value) => value != null ? new Maybe<T>(value) : None;

        [DebuggerHidden]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square) => square.IsSome ? square.Value : None;
    }

    // Provides the core MonadOr methods.
    public partial struct Maybe<T>
    {
        /// <summary>
        /// An instance of <see cref="Maybe{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static readonly Maybe<T> None = new Maybe<T>();

        public Maybe<T> OrElse(Maybe<T> other) => IsNone ? other : this;
    }

    // Implements the Internal.ISwitch<T> interface.
    public partial struct Maybe<T>
    {
        public TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));
            Require.NotNull(caseNone, nameof(caseNone));

            return IsSome ? caseSome.Invoke(Value) : caseNone.Invoke();
        }

        public void Match(Action<T> caseSome, Action caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));
            Require.NotNull(caseNone, nameof(caseNone));

            if (IsSome)
            {
                caseSome.Invoke(Value);
            }
            else
            {
                caseNone.Invoke();
            }
        }
    }

    // Implements the IEnumerable>T> interface.
    public partial struct Maybe<T>
    {
        [SuppressMessage("Microsoft.Contracts", "Suggestion-6-0", Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public IEnumerable<T> ToEnumerable()
        {
            Warrant.NotNull<IEnumerable<T>>();

            if (IsSome)
            {
                yield return Value;
            }
            else
            {
                yield break;

            }
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Warrant.NotNull<IEnumerator<T>>();

            return ToEnumerable().GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            Warrant.NotNull<IEnumerator>();

            return ToEnumerable().GetEnumerator();
        }
    }

    // Implements the IEquatable<T> and IEquatable<Maybe<<T>> interfaces.
    public partial struct Maybe<T>
    {
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Maybe<T> other) => Equals(other, EqualityComparer<T>.Default);

        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (IsSome)
            {
                return other.IsSome && comparer.Equals(Value, other.Value);
            }

            return other.IsNone;
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj) => Equals(obj, EqualityComparer<T>.Default);

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            if (!(other is Maybe<T>))
            {
                return false;
            }

            return Equals((Maybe<T>)other);
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode() => GetHashCode(EqualityComparer<T>.Default);

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, nameof(comparer));

            return IsSome ? comparer.GetHashCode(Value) : 0;
        }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Fx
{
    using System.Diagnostics.Contracts;

    public partial struct Maybe<T>
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!IsSome || Value != null);
        }
    }
}

#endif
