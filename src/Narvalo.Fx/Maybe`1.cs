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

    using static System.Diagnostics.Contracts.Contract;

    /// <summary>
    /// Represents an object that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The underlying type of the value.</typeparam>
    /**
     * <content markup="commonmark">
     * <![CDATA[
     * The Maybe Monad
     * ===============
     *
     * The `Maybe<T>` class is kind of like the `Nullable<T>` class but without any restriction on
     * the underlying type: *it provides a way to tell the absence or the presence of a value*.
     * Taken alone, it might not look that useful, we could simply use a nullable for value types
     * and `null` for reference types. That's where the monad comes into play. The `Maybe<T>`
     * satisfies a very simple grammar, known as the monad laws, from which derives a rich
     * vocabulary.
     *
     * What I like the most about this class is that it helps to clearly express our intent with a
     * very clean syntax. For instance, consider the following methods
     *
     * ```csharp
     * string GetPhoneNumber() { }
     * Maybe<string> MayGetPhoneNumber() { }
     * ```
     *
     * I believe that the second version makes it clearer that we might actually not know the phone
     * number. It then makes easy to write what we do in either cases:
     *
     * ```csharp
     * MayGetPhoneNumber().OnNone(action).OnSome(action)
     * ```
     *
     * This class is sometimes referred to as the Option type.
     *
     * ### Naming convention ###
     *
     * We suggest to prefix with _May_ the methods that return a Maybe instance.
     *
     * Design of `Maybe<T>`
     * --------------------
     *
     * ### Class vs structure ###
     *
     * Argument _towards_ a structure
     * : C# then guarantees that an instance is never null, which seems like a good thing here.
     *   Isn't it one of the reasons why in the first place we decided to create such a class?
     *
     * Argument _against_ a structure
     * : An instance is _mutable_ if `T` is mutable. This should always raise a big warning.
     *   (TODO: Other things to discuss: impact on performances (boxing, size of the struct...)
     *
     * ### `Maybe<T>` vs `Nullable<T>` ###
     *
     * For value types, most of the time `T?` offers a much better alternative. We can not discourage
     * you enough to use a `Maybe<T>` when a nullable would make a better fit,
     * We can not enforce this rule with a generic constraint. For instance, this would prevent us
     * from being able to use `Maybe<Unit>` which must be allowed to unleash the real power of the
     * Maybe monad.
     *
     * Constructor
     * -----------
     *
     * All constructors are made private. Having complete control over the creation of an
     * instance helps us to ensure that `value` is never `null` when passed to the constructor.
     * This is exactly what we do in the static method `Maybe<T>.η(value)`.
     *
     * To make things simpler, we provide two public factory methods:
     *
     * ```csharp
     * Maybe.Of<T>(value)
     * Maybe.Of<T?>(value)
     * ```
     *
     * and one static property `Maybe<T>.None` to reference a Maybe that has no value.
     *
     * `IEnumerable<T>` interface
     * --------------------------
     *
     * To support LINQ we only need to create the appropriate methods and the C# compiler will work
     * its magic. Actually, this is something that we have almost already done. Indeed, this is just
     * a matter of using the right terminology:
     *
     * - `Select` is the LINQ name for the `Map` method from monads
     * - `SelectMany` is the LINQ name for the `Bind` method from monads
     *
     * Nevertheless, since this might look a bit too unusual we also explicitely implement the
     * `IEnumerable<T>` interface.
     *
     * `IEquatable<T>` and `IEquatable<Maybe<T>>` interfaces
     * -------------------------------------------------
     *
     * ### Referential equality and structural equality ###
     *
     * (To be revised)
     * We redefine the `Equals()` method to allow for structural equality for reference types that
     * follow value type semantics. Nevertheless, we do not change the meaning of the equality
     * operators (`==` and `!=`) which continue to test referential equality, behaviour expected by
     * the .NET framework for all reference types. I might change my mind on this and try to make
     * `Maybe<T>` behave more like `Nullable<T>`. As a matter of convenience, we also implement the
     * `IEquatable<T>` interface. Another (abandonned) possibility has been to implement the
     * `IStructuralEquatable` interface.
     *
     * ### Sample rules ###
     *
     * (To be revised)
     * ```csharp
     * Maybe<T>.None != null
     * Maybe<T>.None.Equals(null)
     *
     * Maybe.Of(1) != Maybe.Of(1)
     * Maybe.Of(1).Equals(Maybe.Of(1))
     * Maybe.Of(1) != 1
     * Maybe.Of(1).Equals(1)
     * ```
     *
     * References
     * ----------
     *
     * - [Wikipedia](http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad)
     * - [Haskell](http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html)
     * - [Kinds of Immutability](http://blogs.msdn.com/b/ericlippert/archive/2007/11/13/immutability-in-c-part-one-kinds-of-immutability.aspx)
     *
     * There are many alternative implementations in C#; just google it!
     * ]]>
     * </content>
     */
    [DebuggerDisplay("IsSome = {IsSome}")]
    [DebuggerTypeProxy(typeof(Maybe<>.DebugView))]
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "[Intentionally] Maybe<T> only pretends to be a collection.")]
    public partial struct Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>
    {
        private readonly bool _isSome;

        // You should NEVER use this field directly. Always use instead the property; the Code Contracts
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
            Demand.NotNull(value);

            _value = value;
            _isSome = true;
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
                Demand.State(IsSome);
                Ensures(Result<T>() != null);

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

        #region Operators

        public static explicit operator Maybe<T>(T value) => η(value);

        public static explicit operator T(Maybe<T> value)
        {
            Ensures(Result<T>() != null);

            if (!value.IsSome)
            {
                throw new InvalidCastException(Strings.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates",
            Justification = "[Intentionally] IsSome provides the alternate name for the 'true' operator overload.")]
        public static bool operator true(Maybe<T> value) => value.IsSome;

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates",
            Justification = "[Intentionally] IsSome provides the alternate name for the 'true' operator overload.")]
        public static bool operator false(Maybe<T> value) => !value.IsSome;

        #endregion

        public TResult Map<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone)
        {
            Require.NotNull(caseSome, nameof(caseSome));
            Require.NotNull(caseNone, nameof(caseNone));

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
            Expect.NotNull(action);

            Invoke(action);
        }

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
            Require.NotNull(other, nameof(other));
            Ensures(Result<T>() != null);

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
            Ensures(Result<T>() != null);

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, nameof(exceptionFactory));
            Ensures(Result<T>() != null);

            if (!IsSome)
            {
                throw exceptionFactory.Invoke();
            }

            return Value;
        }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Ensures(Result<string>() != null);

            return IsSome ? Format.Current("Maybe({0})", Value) : "Maybe(None)";
        }

        #region Overrides for auto-generated (extension) methods

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

            if (!IsSome || !inner.IsSome)
            {
                return Maybe<TResult>.None;
            }

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

            // REVIEW: I can't remember why I didn't include !inner.IsSome before?
            if (!IsSome || !inner.IsSome)
            {
                return Maybe<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner))
                : Maybe<TResult>.None;
        }

        #endregion

        #region Non-standard extensions

        public Maybe<T> Invoke(Action<T> action, Action caseNone)
        {
            Require.NotNull(action, nameof(action));
            Require.NotNull(caseNone, nameof(caseNone));

            if (IsSome)
            {
                action.Invoke(Value);
            }
            else
            {
                caseNone.Invoke();
            }

            return this;
        }

        public Maybe<T> Invoke(Action<T> action)
        {
            Require.NotNull(action, nameof(action));

            if (IsSome)
            {
                action.Invoke(Value);
            }

            return this;
        }

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, nameof(action));

            if (!IsSome)
            {
                action.Invoke();
            }

            return this;
        }

        #endregion

        #endregion

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

            public bool IsSome
            {
                get { return _inner.IsSome; }
            }

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
    public partial struct Maybe<T>
    {
        [SuppressMessage("Microsoft.Contracts", "Suggestion-14-0",
            Justification = "[Ignore] Unrecognized postcondition by CCCheck.")]
        public IEnumerable<T> ToEnumerable()
        {
            Ensures(Result<IEnumerable<T>>() != null);

            return IsSome ? Sequence.Pure(Value) : Sequence<T>.Empty;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Ensures(Result<IEnumerator<T>>() != null);

            return ToEnumerable().GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            Ensures(Result<IEnumerator>() != null);

            return ToEnumerable().GetEnumerator();
        }
    }

    /// <content>
    /// Implements the <see cref="IEquatable{T}"/> and <c>IEquatable&lt;Maybe&lt;T&gt;&gt;</c> interfaces.
    /// </content>
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

            return !other.IsSome;
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

    /// <content>
    /// Provides the core Monad methods.
    /// </content>
    public partial struct Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selector)
        {
            Require.NotNull(selector, nameof(selector));

            return IsSome ? selector.Invoke(Value) : Maybe<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [DebuggerHidden]
        internal static Maybe<T> η(T value)
            => value != null ? new Maybe<T>(value) : Maybe<T>.None;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "[Intentionally] Standard naming convention from mathematics. Only used internally.")]
        [DebuggerHidden]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
            => square.IsSome ? square.Value : Maybe<T>.None;
    }

    /// <content>
    /// Provides the core MonadOr methods.
    /// </content>
    public partial struct Maybe<T>
    {
        /// <summary>
        /// An instance of <see cref="Maybe{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static readonly Maybe<T> None = new Maybe<T>();

        public Maybe<T> OrElse(Maybe<T> other) => !IsSome ? other : this;
    }
}
