// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.CompilerServices;

    using Narvalo.Collections;
    using Narvalo.Internal;

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
     * The most obvious weakness of the Maybe monad is that it is all black or all white. In some
     * circumstances, I might like to be able to give an explanation for the absence of a value. 
     * In fact, that's one of the purpose of the Either monad.
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
     * In the end, I prefer to be conservative; creating a structure seems way too hazardous.
     * 
     * ### Apparent Immutability ###
     * 
     * The class is mutable but it is not something observable from the outside.
     *   
     * ### `Maybe<T>` vs `Nullable<T>` ###
     * 
     * For value types, most of the time `T?` offers a much better alternative. We can not discourage 
     * you enough to use a `Maybe<T>` when a nullable would make a better fit, 
     * We can not enforce this rule with a generic constraint. For instance, this would prevent us
     * from being able to use `Maybe<Unit>` which must be allowed to unleash the real power of the Maybe monad.
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
     * Maybe.Create<T>(value)
     * Maybe.Create<T?>(value)
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
     * ```csharp
     * Maybe<T>.None != null
     * Maybe<T>.None.Equals(null)
     *   
     * Maybe.Create(1) != Maybe.Create(1)
     * Maybe.Create(1).Equals(Maybe.Create(1))
     * Maybe.Create(1) != 1
     * Maybe.Create(1).Equals(1)
     * ```
     * 
     * References
     * ----------
     * 
     * - [Wikipedia](http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad)
     * - [Haskell](http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html)
     * - [Kinds of Immutability](http://blogs.msdn.com/b/ericlippert/archive/2007/11/13/immutability-in-c-part-one-kinds-of-immutability.aspx)
     * 
     * Alternative implementations in C#:
     * - [iSynaptic.Commons](https://github.com/iSynaptic/iSynaptic.Commons/blob/master/Application/iSynaptic.Commons/Maybe.cs)
     * ]]>
     * </content>
     */
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "Maybe<T> only pretends to be a collection.")]
    public sealed partial class Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        private readonly bool _isSome;

        // You should NEVER use this field directly. Use instead the property; the Code Contracts 
        // static checker should then prove that no illegal access to this field happen (i.e. when IsNone is true).
        private readonly T _value;

        /// <summary>
        /// Prevents a default instance of the <see cref="Maybe{T}" /> class from being created outside.
        /// Initializes a new instance of the <see cref="Maybe{T}" /> class that does not hold any value.
        /// </summary>
        /// <seealso cref="Maybe{T}.None"/>
        private Maybe()
        {
            _isSome = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}" /> class for the specified value. 
        /// </summary>
        /// <param name="value">A value to wrap.</param>
        /// <seealso cref="Maybe.Create{T}(T)"/>
        /// <seealso cref="Maybe.Create{T}(T?)"/>
        private Maybe(T value)
        {
            Contract.Requires(value != null);

            _value = value;
            _isSome = true;
        }

        /// <summary>
        /// Gets a value indicating whether the object does not enclose any value.
        /// </summary>
        /// <value><c>true</c> if the object does not have enclose any value; otherwise <c>false</c>.</value>
        internal bool IsNone { get { return !_isSome; } }

        /// <summary>
        /// Gets a value indicating whether the object does hold a value.
        /// </summary>
        /// <value><c>true</c> if the object does hold a value; otherwise <c>false</c>.</value>
        internal bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Gets the enclosed value.
        /// </summary>
        /// <remarks>
        /// Any access to this property must be protected by firstly checking that <see cref="IsSome"/> 
        /// is <see langword="true"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">The object does not enclose any value.</exception>
        internal T Value
        {
            get
            {
                // Ensures that any call to this property is guarded upstream.
                Promise.Condition(IsSome);
                Contract.Ensures(Contract.Result<T>() != null);

#if CONTRACTS_FULL

                if (_value == null)
                {
                    // I'm pretty sure this can never happen. Even if the caller "nullify" the referenced 
                    // value, we still hold a reference to it so that it can not be null after all.
                    // Whatever, keep this around to make the Code Contracts static checker happy.
                    // If I am right about this, without Code Contracts, we are better off disabling this;
                    // the property will be inlined for sure. Furthermore, not throwing the exception means
                    // that we can use this property safely, for instance inside the GetHashCode() method.
                    // Remember that in our particular setup the Code Contracts symbol does not exist in any 
                    // build except when we do run the Code Contracts tools, which implies that this particular
                    // piece of code will never execute at runtime.
                    throw new InvalidOperationException();
                }

#endif

                return _value;
            }
        }

        public static explicit operator Maybe<T>(T value)
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return η(value);
        }

        public static explicit operator T(Maybe<T> value)
        {
            Require.NotNull(value, "value");
            Contract.Ensures(Contract.Result<T>() != null);

            if (value.IsNone)
            {
                throw new InvalidCastException(Strings_Core.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-30-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<T> OnSome(Action<T> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return Run(action);
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
        /// Obtains the enclosed value if any; otherwise <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="defaultValue"/>.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return IsSome ? Value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return IsSome ? Value : defaultValueFactory.Invoke();
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

            if (IsNone)
            {
                throw exceptionFactory.Invoke();
            }

            return Value;
        }

        /// <copydoc cref="Object.ToString" />
        public override String ToString()
        {
            return IsSome ? Value.ToString() : "{None}";
        }

#if CONTRACTS_FULL

        [ContractInvariantMethod]
        private void ObjectInvariants()
        {
            Contract.Invariant(IsNone == !IsSome);
            Contract.Invariant(IsNone || Value != null);
        }

#endif
    }

    /// <content>
    /// Implements the <see cref="IEnumerable{T}"/> interface.
    /// </content>
    public partial class Maybe<T>
    {
        /// <copydoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<T>>() != null);

            return AsEnumerable().GetEnumerator();
        }

        /// <copydoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator>() != null);

            ////return ((IEnumerable<T>)this).GetEnumerator();
            return AsEnumerable().GetEnumerator();
        }

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-17-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public IEnumerable<T> AsEnumerable()
        {
            Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);

            return IsSome ? Sequence.Single(Value) : Sequence.Empty<T>();
        }
    }

    /// <content>
    /// Implements the <see cref="IEquatable{T}"/> and <c>IEquatable&lt;Maybe&lt;T&gt;&gt;</c> interfaces.
    /// </content>
    public partial class Maybe<T>
    {
        /// <copydoc cref="IEquatable{T}.Equals" />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (Object.ReferenceEquals(other, null))
            {
                return IsNone;
            }

            if (IsNone || other.IsNone)
            {
                // If one is none, they must be both none to be equal.
                return IsNone && other.IsNone;
            }

            return comparer.Equals(Value, other.Value);
        }

        /// <copydoc cref="IEquatable{T}.Equals" />
        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Contract.Requires(comparer != null);

            return Equals(η(other), comparer);
        }

        /// <copydoc cref="Object.Equals(Object)" />
        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<T>.Default);
        }

        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (other == null)
            {
                return IsNone;
            }

            if (other is T)
            {
                return Equals((T)other, comparer);
            }

            // Usually, we test the condition `obj.GetType() == this.GetType()`, in case `this` or
            // `obj` is an instance of a derived type, something that can not happen here because
            // Maybe is sealed.
            return Equals(other as Maybe<T>, comparer);
        }

        /// <copydoc cref="Object.GetHashCode" />
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

        ////public static bool operator ==(Maybe<T> left, Maybe<T> right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null) ? true : right.IsNone;
        ////    }

        ////    return left.Equals(right);
        ////}

        ////public static bool operator ==(Maybe<T> left, T right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null);
        ////    }

        ////    return left.Equals(right);
        ////}

        ////public static bool operator ==(T left, Maybe<T> right)
        ////{
        ////    return right == left;
        ////}

        ////public static bool operator !=(Maybe<T> left, Maybe<T> right)
        ////{
        ////    return !(left == right);
        ////}

        ////public static bool operator !=(Maybe<T> left, T right)
        ////{
        ////    return !(left == right);
        ////}

        ////public static bool operator !=(T left, Maybe<T> right)
        ////{
        ////    return !(right == left);
        ////}

        #endregion
    }

    /// <content>
    /// Provides the core Monad methods.
    /// </content>
    public partial class Maybe<T>
    {
#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? (selector.Invoke(Value) ?? Maybe<TResult>.None) : Maybe<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Maybe<T> η(T value)
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            Require.NotNull(square, "square");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return square.IsSome ? square.Value : Maybe<T>.None;
        }
    }

    /// <content>
    /// Provides the core MonadOr methods.
    /// </content>
    public partial class Maybe<T>
    {
        private static readonly Maybe<T> s_None = new Maybe<T>();

        /// <summary>
        /// Gets an instance of <see cref="Maybe{T}" /> that does not enclose any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        public static Maybe<T> None
        {
            get
            {
                Contract.Ensures(Contract.Result<Maybe<T>>() != null);

                return s_None;
            }
        }

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<T> OrElse(Maybe<T> other)
        {
            Require.NotNull(other, "other");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return IsNone ? other : this;
        }
    }

    /// <content>
    /// Provides overrides for a bunch of auto-generated (extension) methods (see Maybe.g.cs).
    /// </content>
    public partial class Maybe<T>
    {
        #region Basic Monad functions

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? Maybe<TResult>.η(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            Require.NotNull(other, "other");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? other : Maybe<TResult>.None;
        }

        #endregion

        #region Monadic lifting operators

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-39-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> Zip<TSecond, TResult>(
            Maybe<TSecond> second,
            Func<T, TSecond, TResult> resultSelector)
        {
            Require.NotNull(second, "second");
            Require.NotNull(resultSelector, "resultSelector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome && second.IsSome
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, second.Value))
                : Maybe<TResult>.None;
        }

        #endregion

        #region LINQ extensions

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> Join<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            if (IsNone || inner.IsNone)
            {
                return Maybe<TResult>.None;
            }

            var outerKey = outerKeySelector.Invoke(Value);
            var innerKey = innerKeySelector.Invoke(inner.Value);

            return (comparer ?? EqualityComparer<TKey>.Default).Equals(outerKey, innerKey)
                ? Maybe<TResult>.η(resultSelector.Invoke(Value, inner.Value))
                : Maybe<TResult>.None;
        }

#if !NO_CONTRACTS_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[CodeContracts] Unrecognized postcondition by CCCheck.")]
#endif
        public Maybe<TResult> GroupJoin<TInner, TKey, TResult>(
            Maybe<TInner> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, Maybe<TInner>, TResult> resultSelector,
            IEqualityComparer<TKey> comparer)
        {
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            // REVIEW: I can't remember why I didn't include inner.IsNone before?
            if (IsNone || inner.IsNone)
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

        public Maybe<T> Run(Action<T> action)
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            if (IsSome)
            {
                action.Invoke(Value);
            }

            return this;
        }

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            if (IsNone)
            {
                action.Invoke();
            }

            return this;
        }

        #endregion
    }
}
