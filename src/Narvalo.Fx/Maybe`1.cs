// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    using Narvalo.Fx.Properties;

    /// <summary>
    /// Represents an object that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    [DebuggerDisplay("IsSome = {IsSome}")]
    [DebuggerTypeProxy(typeof(Maybe<>.DebugView))]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "[Intentionally] Maybe<T> only pretends to be a collection.")]
    public partial struct Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, Internal.IAlternative<T>
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
        /// <value>true if the object does hold a value; otherwise false.</value>
        // Named <c>isJust</c> in Haskell parlance.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSome { get { return _isSome; } }

        // Named <c>isNothing</c> in Haskell parlance.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsNone => !IsSome;

        /// <summary>
        /// Gets the enclosed value.
        /// </summary>
        /// <remarks>Any access to this property must be protected by checking before that
        /// <see cref="IsSome"/> is true.</remarks>
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

        #region Conversion operators.

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
        /// Obtains the enclosed value if any; otherwise the default value of type T.
        /// </summary>
        /// <returns>The enclosed value if any; otherwise the default value of type T.</returns>
        public T ValueOrDefault() => IsSome ? Value : default(T);

        /// <summary>
        /// Obtains the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="other"/>.</returns>
        // Named <c>fromMaybe</c> in Haskell parlance.
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

        // Named <c>fromJust</c> in Haskell parlance.
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

    // Provides the core Monad methods.
    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selectorM)
        {
            Require.NotNull(selectorM, nameof(selectorM));

            return IsSome ? selectorM.Invoke(Value) : Maybe<TResult>.None;
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
        /// <summary>
        /// An instance of <see cref="Maybe{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static readonly Maybe<T> None = new Maybe<T>();

        public Maybe<T> OrElse(Maybe<T> other) => IsNone ? other : this;
    }

    // Implements the Internal.IAlternative<T> interface.
    public partial struct Maybe<T>
    {
        public TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));
            Require.NotNull(caseNone, nameof(caseNone));

            return IsSome ? caseSome.Invoke(Value) : caseNone.Invoke();
        }

        // Named <c>maybe</c> in Haskell parlance.
        public TResult Match<TResult>(Func<T, TResult> caseSome, TResult caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));

            return IsSome ? caseSome.Invoke(Value) : caseNone;
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(selector, nameof(selector));
            Require.NotNull(otherwise, nameof(otherwise));

            return IsSome && predicate.Invoke(Value) ? selector.Invoke(Value) : otherwise.Invoke();
        }

        public TResult Coalesce<TResult>(Func<T, bool> predicate, TResult then, TResult other)
        {
            Require.NotNull(predicate, nameof(predicate));

            return IsSome && predicate.Invoke(Value) ? then : other;
        }

        public void Do(Func<T, bool> predicate, Action<T> action, Action otherwise)
        {
            Require.NotNull(predicate, nameof(predicate));
            Require.NotNull(action, nameof(action));
            Require.NotNull(otherwise, nameof(otherwise));

            if (IsSome && predicate.Invoke(Value))
            {
                action.Invoke(Value);
            }
            else
            {
                otherwise.Invoke();
            }
        }

        public void Do(Action<T> onSome, Action onNone)
        {
            Require.NotNull(onSome, nameof(onSome));
            Require.NotNull(onNone, nameof(onNone));

            if (IsSome)
            {
                onSome.Invoke(Value);
            }
            else
            {
                onNone.Invoke();
            }
        }

        // Alias for Do(Action<T>).
        public void OnSome(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSome) { action.Invoke(Value); }
        }

        public void OnNone(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (IsNone) { action.Invoke(); }
        }
    }

    // Implements the IEnumerable<T> interface.
    public partial struct Maybe<T>
    {
        // Named <c>maybeToList</c> in Haskell parlance.
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

    // Implements the IEquatable<Maybe<<T>> interfaces.
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
