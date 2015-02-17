// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /*!
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
     *     string GetPhoneNumber() { ... }
     *     Maybe<string> MayGetPhoneNumber() { ... }
     * 
     * I believe that the second version makes it clearer that we might actually not know the phone 
     * number. It then makes easy to write what we do in either cases:
     * 
     *     MayGetPhoneNumber().OnNone( ... ).OnSome( ... )
     * 
     * The most obvious weakness of the Maybe monad is that it is all black or all white. In some
     * circumstances, I might like to be able to give an explanation for the absence of a value. 
     * In fact, that's one of the purpose of the Either monad.
     * 
     * The main defects of this implementation are :
     * 
     * + It is a reference type
     * + An instance is mutable for reference types
     * + (more to be added later, I am sure there are other problems)
     * 
     * This class is sometimes referred to as the Option type.
     * 
     * ### Naming convention ###
     * 
     * We prefix with _May_ all methods that return a Maybe instance.
     * 
     * Design of `Maybe<T>`
     * --------------------
     * 
     * ### Class vs structure ###
     * 
     * Argument _towards_ a structure
     * : C# then guarantees that an instance is never null, which seems like a good thing here.
     *   Isn't it one of the reasons why we decided in the first place to create such a class?
     * 
     * Argument _against_ a structure
     * : An instance is mutable if `T` is a reference type. This should always raise a big warning.
     *   
     * In the end, creating a mutable structure seems way too hazardous.
     *   
     * ### Maybe<T> vs Nullable<T> ###
     * 
     * Most of the time, for value types, `T?` offers a much better alternative. To discourage the 
     * use of the `Maybe<T>` when a nullable would make a better fit, I should create a FxCop rule.
     * 
     * Implementation
     * --------------
     */

    /// <summary>
    /// Represents a value that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "Maybe<T> only pretends to be a collection.")]
    public sealed partial class Maybe<T> : IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T>
    {
        readonly bool _isSome;
        readonly T _value;

        /*!
         * ### Constructor ###
         * 
         * All constructors are made private. Having complete control over the creation of an 
         * instance helps us to ensure that `value` is never `null` when passed to the constructor.
         * This is exactly what we do in the static method `Maybe<T>.η(value)`.
         * 
         * To make things simpler, we provide two public factory methods:
         * 
         * + `Maybe.Create<T>(value)`
         * + `Maybe.Create<T?>(value)`
         * 
         * and one static property `Maybe<T>.None` to reference a Maybe that has no value.
         */

        /// <summary>
        /// Initializes a new instance of <see cref="Maybe{T}" /> that does not hold any value.
        /// </summary>
        Maybe()
        {
            _isSome = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Maybe{T}" /> using the specified value. 
        /// </summary>
        /// <param name="value">The underlying value.</param>
        Maybe(T value)
        {
            Contract.Requires(value != null);

            _value = value;
            _isSome = true;
        }

        /*!
         * ### Properties ###
         */

        /// <summary>
        /// Returns true if the object does not have an underlying value, false otherwise.
        /// </summary>
        internal bool IsNone { get { return !_isSome; } }

        /// <summary>
        /// Returns true if the object contains a value, false otherwise.
        /// </summary>
        internal bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Returns the underlying value.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The object does not contain any value.
        /// </exception>
        internal T Value
        {
            get
            {
                if (!_isSome) {
                    throw new InvalidOperationException(Strings_Core.Maybe_NoneHasNoValue);
                }

                return _value;
            }
        }

        /*!
         * ### Cast operators ###
         */

        /// <summary />
        public static explicit operator Maybe<T>(T value)
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return η(value);
        }

        /// <summary />
        public static explicit operator T(Maybe<T> value)
        {
            Require.NotNull(value, "value");

            if (value.IsNone) {
                throw new InvalidCastException(Strings_Core.Maybe_CannotCastNoneToValue);
            }

            return value.Value;
        }

        /*!
         * ### Public methods ###
         */

        public Maybe<T> OnSome(Action<T> action)
        {
            Contract.Requires(action != null);
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return Run(action);
        }

        /// <summary>
        /// Returns the underlying value if any, the default value of the type T otherwise.
        /// </summary>
        /// <returns>The underlying value or the default value of the type T.</returns>
        public T ValueOrDefault()
        {
            return _isSome ? _value : default(T);
        }

        /// <summary>
        /// Returns the underlying value if any, defaultValue otherwise.
        /// </summary>
        /// <param name="defaultValue">
        /// A default value to be used if if there is no underlying value.
        /// </param>
        /// <returns>The underlying value or defaultValue.</returns>
        public T ValueOrElse(T defaultValue)
        {
            return _isSome ? _value : defaultValue;
        }

        public T ValueOrElse(Func<T> defaultValueFactory)
        {
            Require.NotNull(defaultValueFactory, "defaultValueFactory");

            return _isSome ? _value : defaultValueFactory.Invoke();
        }

        public T ValueOrThrow(Exception exception)
        {
            Require.NotNull(exception, "exception");

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");

            if (!_isSome) {
                throw exceptionFactory.Invoke();
            }

            return _value;
        }

        /// <summary />
        public override String ToString()
        {
            return _isSome ? _value.ToString() : "{None}";
        }

        ////#if CONTRACTS_FULL
        ////        [ContractInvariantMethod]
        ////        void ObjectInvariants()
        ////        {
        ////            Contract.Invariant(!_isSome || _value != null);
        ////        }
        ////#endif
    }

    /*!
     * IEnumerable interface
     * ---------------------
     * 
     * To support Linq we only need to create the appropriate methods and the C# compiler will work
     * its magic. Actually, this is something that we have almost already done. Indeed, this is just
     * a matter of using the right terminology :
     * 
     * + `Select` is the Linq name for the `Map` method from monads
     * + `SelectMany` is the Linq name for the `Bind` method from monads
     * + ...
     * 
     * Nevertheless, since this might look a bit too unusual we also explicitely implement the
     * `IEnumerable<T>` interface.
     */
    public partial class Maybe<T>
    {
        /// <summary />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            if (IsSome) {
                return new List<T> { _value }.GetEnumerator();
            }
            else {
                return Enumerable.Empty<T>().AssumeNotNull().GetEnumerator();
            }
        }

        /// <summary />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }

    /*!
     * IEquatable interface
     * --------------------
     * 
     * ### Referential equality and structural equality ###
     * 
     * We redefine the `Equals()` method to allow for structural equality for reference types that
     * follow value type semantics. Nevertheless, we do not change the meaning of the equality
     * operators (`==` and `!=`) which continue to test referential equality, behaviour expected by
     * the .NET framework for all reference types. I might change my mind on this and try to make
     * `Maybe<T>` behave like `Nullable<T>`. As a matter of convenience, we also implement the
     * `IEquatable<T>` interface. Another (abandonned) possibility has been to implement the
     * `IStructuralEquatable` interface.
     * 
     * ### Sample rules ###
     * 
     *     Maybe<T>.None != null
     *     Maybe<T>.None.Equals(null)
     *   
     *     Maybe.Create(1) != Maybe.Create(1)
     *     Maybe.Create(1).Equals(Maybe.Create(1))
     *     Maybe.Create(1) != 1
     *     Maybe.Create(1).Equals(1)
     */
    public partial class Maybe<T>
    {
        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (Object.ReferenceEquals(other, null)) {
                return !_isSome;
            }

            if (!_isSome) {
                return !other._isSome;
            }

            return comparer.Equals(_value, other._value);
        }

        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Contract.Requires(comparer != null);

            return Equals(η(other), comparer);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            return Equals(obj, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(object other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (other == null) {
                return !_isSome;
            }

            if (other is T) {
                return Equals((T)other, comparer);
            }

            // Usually, we test the condition `obj.GetType() == this.GetType()`, in case `this` or
            // `obj` is an instance of a derived type, something that can not happen here because
            // Maybe is sealed.
            return Equals(other as Maybe<T>, comparer);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return GetHashCode(EqualityComparer<T>.Default);
        }

        /// <summary />
        public int GetHashCode(IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            return _isSome ? comparer.GetHashCode(_value) : 0;
        }

        /////// <summary />
        ////public static bool operator ==(Maybe<T> left, Maybe<T> right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null) ? true : right.IsNone;
        ////    }

        ////    return left.Equals(right);
        ////}

        /////// <summary />
        ////public static bool operator ==(Maybe<T> left, T right)
        ////{
        ////    if (ReferenceEquals(left, null)) {
        ////        return ReferenceEquals(right, null);
        ////    }

        ////    return left.Equals(right);
        ////}

        /////// <summary />
        ////public static bool operator ==(T left, Maybe<T> right)
        ////{
        ////    return right == left;
        ////}

        /////// <summary />
        ////public static bool operator !=(Maybe<T> left, Maybe<T> right)
        ////{
        ////    return !(left == right);
        ////}

        /////// <summary />
        ////public static bool operator !=(Maybe<T> left, T right)
        ////{
        ////    return !(left == right);
        ////}

        /////// <summary />
        ////public static bool operator !=(T left, Maybe<T> right)
        ////{
        ////    return !(right == left);
        ////}
    }

    /*!
     * Monad definition
     * ----------------
     */
    public partial class Maybe<T>
    {
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? (selector.Invoke(Value) ?? Maybe<TResult>.None) : Maybe<TResult>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Maybe<T> η(T value)
        {
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return value != null ? new Maybe<T>(value) : Maybe<T>.None;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter",
            Justification = "Standard naming convention from mathematics. Only used internally.")]
        internal static Maybe<T> μ(Maybe<Maybe<T>> square)
        {
            Require.NotNull(square, "square");

            return square.IsSome ? square.Value : Maybe<T>.None;
        }
    }

    /*!
     * MonadOr definition
     * ------------------
     */
    public partial class Maybe<T>
    {
        static readonly Maybe<T> None_ = new Maybe<T>();

        /// <summary>
        /// Returns an instance of <see cref="Maybe{T}" /> that does not hold any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A non-generic version would not improve usability.")]
        public static Maybe<T> None
        {
            get
            {
                Contract.Ensures(Contract.Result<Maybe<T>>() != null);

                return None_;
            }
        }

        public Maybe<T> OrElse(Maybe<T> other)
        {
            return IsNone ? other : this;
        }
    }

    /*!
     * Optimized version for Monad extension methods
     * ---------------------------------------------
     */
    public partial class Maybe<T>
    {
        #region Basic Monad functions

        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? Maybe<TResult>.η(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            Require.NotNull(other, "other");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? other : Maybe<TResult>.None;
        }

        #endregion

        #region Monadic lifting operators

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

        #region Linq extensions

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

            if (IsNone || inner.IsNone) {
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
            Require.NotNull(inner, "inner");
            Require.NotNull(outerKeySelector, "valueSelector");
            Require.NotNull(innerKeySelector, "innerKeySelector");
            Require.NotNull(resultSelector, "resultSelector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            if (IsNone) {
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

            if (IsSome) {
                action.Invoke(Value);
            }

            return this;
        }

        public Maybe<T> OnNone(Action action)
        {
            Require.NotNull(action, "action");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            if (IsNone) {
                action.Invoke();
            }

            return this;
        }

        #endregion
    }

    /*!
     * References
     * ----------
     * 
     * + [Wikipedia](http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad)
     * + [Haskell](http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html)
     * 
     * Alternative implementations in C#:
     * 
     * + [iSynaptic.Commons](https://github.com/iSynaptic/iSynaptic.Commons/blob/master/Application/iSynaptic.Commons/Maybe.cs)
     * 
     */
}
