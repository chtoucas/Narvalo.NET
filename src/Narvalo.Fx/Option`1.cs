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
    [DebuggerDisplay("IsSome={IsSome}")]
    [DebuggerTypeProxy(typeof(Option<>.DebugView))]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "[Intentionally] Option<T> only pretends to be a collection.")]
    public partial struct Option<T> : IEnumerable<T>, IEquatable<Option<T>>, IEquatable<T>
    {
        private readonly bool _isSome;

        // You should NEVER use this field directly. Use instead the property; the Code Contracts 
        // static checker should then prove that no illegal access to this field happen (i.e. when IsSome is false).
        private readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Option{T}" /> class for the specified value. 
        /// </summary>
        /// <param name="value">A value to wrap.</param>
        /// <seealso cref="Option.Of{T}(T)"/>
        /// <seealso cref="Option.Of{T}(T?)"/>
        private Option(T value)
        {
            Contract.Requires(value != null);

            _value = value;
            _isSome = value != null;
        }

        /// <summary>
        /// Gets a value indicating whether the object does hold a value.
        /// </summary>
        /// <remarks>Most of the time, you don't need to access this property.
        /// You are better off using the rich vocabulary that this class offers.</remarks>
        /// <value><see langword="true"/> if the object does hold a value; otherwise <see langword="false"/>.</value>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Gets the enclosed value.
        /// </summary>
        /// <remarks>
        /// Any access to this property must be protected by checking before that <see cref="IsSome"/> 
        /// is <see langword="true"/>.
        /// </remarks>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal T Value
        {
            get
            {
                Promise.Condition(IsSome, "Prove that any call to this internal property is guarded upstream.");
                Contract.Ensures(Contract.Result<T>() != null);

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

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator Option<T>(T value)
        {
            return η(value);
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static explicit operator T(Option<T> value)
        {
            Contract.Ensures(Contract.Result<T>() != null);

            if (!value.IsSome)
            {
                throw new InvalidCastException(Strings.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }

        public TResult Map<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone)
        {
            Require.NotNull(caseSome, "caseSome");
            Require.NotNull(caseNone, "caseNone");

            if (IsSome)
            {
                return caseSome.Invoke(Value);
            }
            else
            {
                return caseNone.Invoke();
            }
        }

        public void OnSome(Action<T> action)
        {
            Contract.Requires(action != null);

            Invoke(action);
        }

        /// <summary>
        /// Obtains the enclosed value if any; otherwise the default value of the type T.
        /// </summary>
        /// <returns>The enclosed value if any; otherwise the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return IsSome ? Value : default(T);
        }

        /// <summary>
        /// Obtains the enclosed value if any; otherwise <paramref name="other"/>.
        /// </summary>
        /// <param name="other">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="other"/>.</returns>
        public T ValueOrElse(T other)
        {
            return IsSome ? Value : other;
        }

        public T ValueOrElse(Func<T> valueFactory)
        {
            Require.NotNull(valueFactory, "valueFactory");

            return IsSome ? Value : valueFactory.Invoke();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, "exception");
            Contract.Ensures(Contract.Result<T>() != null);

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<T>() != null);

            if (!IsSome)
            {
                throw exceptionFactory.Invoke();
            }

            return Value;
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return IsSome ? Format.CurrentCulture("Option({0})", Value) : "Option(None)";
        }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(!IsSome || Value != null);
        }

#endif
    }

    /// <content>
    /// Implements the <see cref="IEnumerable{T}"/> interface.
    /// </content>
    public partial struct Option<T>
    {
        [SuppressMessage("Microsoft.Contracts", "Suggestion-17-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public IEnumerable<T> ToEnumerable()
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return IsSome ? Sequence.Single(Value) : Sequence.Empty<T>();
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<T>>() != null);

            return ToEnumerable().GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator>() != null);

            return ToEnumerable().GetEnumerator();
        }
    }

    /// <content>
    /// Implements the <see cref="IEquatable{T}"/> and <c>IEquatable&lt;Option&lt;T&gt;&gt;</c> interfaces.
    /// </content>
    public partial struct Option<T>
    {
        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(Option<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Option<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (Object.ReferenceEquals(other, null))
            {
                return !IsSome;
            }

            if (!IsSome || !other.IsSome)
            {
                // If one is none, both must be none to be equal.
                return !IsSome && !other.IsSome;
            }

            return comparer.Equals(Value, other.Value);
        }

        /// <inheritdoc cref="IEquatable{T}.Equals" />
        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            // REVIEW: Use return Equals(η(other), comparer)?
            if (!IsSome)
            {
                return Object.ReferenceEquals(other, null);
            }

            return comparer.Equals(Value, other);
        }

        /// <inheritdoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<T>.Default);
        }

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (other == null)
            {
                return !IsSome;
            }

            if (other is T)
            {
                return Equals((T)other, comparer);
            }

            if (other is Option<T>)
            {
                return Equals((Option<T>)other, comparer);
            }

            return false;
        }

        /// <inheritdoc cref="Object.GetHashCode" />
        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            return IsSome ? comparer.GetHashCode(Value) : 0;
        }

        #region Equality operators (see "Referential equality and structural equality" above)

        ////public static bool operator ==(Option<T> left, Option<T> right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null) ? true : !right.IsSome;
        ////    }

        ////    return left.Equals(right);
        ////}

        ////public static bool operator ==(Option<T> left, T right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null);
        ////    }

        ////    return left.Equals(right);
        ////}

        ////public static bool operator ==(T left, Option<T> right)
        ////{
        ////    return right == left;
        ////}

        ////public static bool operator !=(Option<T> left, Option<T> right)
        ////{
        ////    return !(left == right);
        ////}

        ////public static bool operator !=(Option<T> left, T right)
        ////{
        ////    return !(left == right);
        ////}

        ////public static bool operator !=(T left, Option<T> right)
        ////{
        ////    return !(right == left);
        ////}

        #endregion
    }

    /// <content>
    /// Provides boolean operators.
    /// </content>
    public partial struct Option<T>
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates",
            Justification = "[Intentionally] IsSome provides the alternate name for the 'true' operator overload.")]
        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Intentionally] Operator's overloads must be static.")]
        public static bool operator true(Option<T> value)
        {
            return value.IsSome;
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates",
            Justification = "[Intentionally] IsSome provides the alternate name for the 'true' operator overload.")]
        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Intentionally] Operator's overloads must be static.")]
        public static bool operator false(Option<T> value)
        {
            return !value.IsSome;
        }
    }

    /// <content>
    /// Provides the core Monad methods.
    /// </content>
    public partial struct Option<T>
    {
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> selector)
        {
            Require.NotNull(selector, "selector");

            return IsSome ? selector.Invoke(Value) : Option<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [DebuggerHidden]
        internal static Option<T> η(T value)
        {
            return value != null ? new Option<T>(value) : Option<T>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [DebuggerHidden]
        internal static Option<T> μ(Option<Option<T>> square)
        {
            return square.IsSome ? square.Value : Option<T>.None;
        }
    }

    /// <content>
    /// Provides the core MonadOr methods.
    /// </content>
    public partial struct Option<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Option<T> s_None = new Option<T>();

        /// <summary>
        /// Gets an instance of <see cref="Option{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Option<T> None
        {
            get
            {
                return s_None;
            }
        }

        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<T> OrElse(Option<T> other)
        {
            return !IsSome ? other : this;
        }
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Option.g.cs).
    /// </content>
    public partial struct Option<T>
    {
        #region Basic Monad functions

        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");

            return IsSome ? Option<TResult>.η(selector.Invoke(Value)) : Option<TResult>.None;
        }

        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> Then<TResult>(Option<TResult> other)
        {
            return IsSome ? other : Option<TResult>.None;
        }

        #endregion

        #region Monadic lifting operators

        [SuppressMessage("Microsoft.Contracts", "Suggestion-39-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> Zip<TSecond, TResult>(
            Option<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(resultSelector, "resultSelector");

            return IsSome && second.IsSome
                ? Option<TResult>.η(resultSelector.Invoke(Value, second.Value))
                : Option<TResult>.None;
        }

        #endregion

        #region LINQ extensions

        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> Join<TInner, TKey, TResult>(
            Option<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            if (!IsSome || !inner.IsSome)
            {
                return Option<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Option<TResult>.η(resultSelector.Invoke(Value, inner.Value))
                : Option<TResult>.None;
        }

        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public Option<TResult> GroupJoin<TInner, TKey, TResult>(
            Option<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, Option<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");

            // REVIEW: I can't remember why I didn't include !inner.IsSome before?
            if (!IsSome || !inner.IsSome)
            {
                return Option<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Option<TResult>.η(resultSelector.Invoke(Value, inner))
                : Option<TResult>.None;
        }

        #endregion

        #region Non-standard extensions

        public void Invoke(Action<T> action, Action caseNone)
        {
            Require.NotNull(action, "action");
            Require.NotNull(caseNone, "caseNone");

            if (IsSome)
            {
                action.Invoke(Value);
            }
            else
            {
                caseNone.Invoke();
            }
        }

        public void Invoke(Action<T> action)
        {
            Require.NotNull(action, "action");

            if (IsSome)
            {
                action.Invoke(Value);
            }
        }

        public void OnNone(Action action)
        {
            Require.NotNull(action, "action");

            if (!IsSome)
            {
                action.Invoke();
            }
        }

        #endregion
    }

    /// <content>
    /// Provides a debugger view of <see cref="Option{T}"/>.
    /// </content>
    public partial struct Option<T>
    {
        /// <summary>
        /// Represents a debugger type proxy for <see cref="Option{T}"/>.
        /// </summary>
        [ContractVerification(false)] // Debugger-only code.
        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        private sealed class DebugView
        {
            private readonly Option<T> _inner;

            public DebugView(Option<T> inner)
            {
                _inner = inner;
            }

            public bool IsSome
            {
                get { return _inner.IsSome; }
            }

            [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
                Justification = "[Ignore] Debugger-only code.")]
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
}
