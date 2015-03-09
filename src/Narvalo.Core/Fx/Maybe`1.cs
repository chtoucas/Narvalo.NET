// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.CompilerServices;

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
     * The main defects of this implementation are :
     * 
     * - The most obvious weakness of the Maybe monad is that it is all black or all white. In some
     *   circumstances, I might like to be able to give an explanation for the absence of a value. 
     *   In fact, that's one of the purpose of the Either monad.
     * - It is a reference type
     * - An instance is mutable for reference types
     * - (more to be added later, I am sure there are other problems)
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
     * `Maybe<T>` behave like `Nullable<T>`. As a matter of convenience, we also implement the
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
        private readonly T _value;

        /// <summary>
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
        /// <exception cref="InvalidOperationException">The object does not enclose any value.</exception>
        internal T Value
        {
            get
            {
                Contract.Ensures(Contract.Result<T>() != null);

#if DEBUG
                if (!_isSome)
                {
                    throw new InvalidOperationException(Strings_Core.Maybe_NoneHasNoValue);
                }
#endif

                if (_value == null)
                {
                    throw new InvalidOperationException(Strings_Core.Maybe_NoneHasNoValue);
                }

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

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-30-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
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
            return _isSome ? _value : default(T);
        }

        /// <summary>
        /// Obtains the enclosed value if any; otherwise <paramref name="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
        /// <returns>The enclosed value if any; otherwise <paramref name="defaultValue"/>.</returns>
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
            Contract.Ensures(Contract.Result<T>() != null);

            return ValueOrThrow(() => exception);
        }

        public T ValueOrThrow(Func<Exception> exceptionFactory)
        {
            Require.NotNull(exceptionFactory, "exceptionFactory");
            Contract.Ensures(Contract.Result<T>() != null);

            // Do prefer IsNone and Value to !_isSome and _value,
            // it should help CCCheck to discover the object invariants.
            if (IsNone)
            {
                throw exceptionFactory.Invoke();
            }

            return Value;
        }

        /// <copydoc cref="Object.ToString" />
        public override String ToString()
        {
            return _isSome ? _value.ToString() : "{None}";
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

    // Implements the IEnumerable<T> interface.
    public partial class Maybe<T>
    {
        /// <copydoc cref="IEnumerable{T}.GetEnumerator" />
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator<T>>() != null);

            if (IsSome)
            {
                return new List<T> { _value }.GetEnumerator();
            }
            else
            {
                return Enumerable.Empty<T>().AssumeNotNull().GetEnumerator();
            }
        }

        /// <copydoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            Contract.Ensures(Contract.Result<IEnumerator>() != null);

            return ((IEnumerable<T>)this).GetEnumerator();
        }
    }

    // Implements the IEquatable<T> and IEquatable<Maybe<T>> interfaces
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
                return !_isSome;
            }

            if (!_isSome)
            {
                return !other._isSome;
            }

            return comparer.Equals(_value, other._value);
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
                return !_isSome;
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

            return _isSome ? comparer.GetHashCode(_value) : 0;
        }

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
    }

    // Monad definition
    public partial class Maybe<T>
    {
#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
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

    // MonadOr definition.
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

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public Maybe<T> OrElse(Maybe<T> other)
        {
            Require.NotNull(other, "other");
            Contract.Ensures(Contract.Result<Maybe<T>>() != null);

            return IsNone ? other : this;
        }
    }

    // Custom versions of Monad extension methods (see Maybe.g.cs).
    public partial class Maybe<T>
    {
        #region Basic Monad functions

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public Maybe<TResult> Select<TResult>(Func<T, TResult> selector)
        {
            Require.NotNull(selector, "selector");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? Maybe<TResult>.η(selector.Invoke(Value)) : Maybe<TResult>.None;
        }

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-28-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
#endif
        public Maybe<TResult> Then<TResult>(Maybe<TResult> other)
        {
            Require.NotNull(other, "other");
            Contract.Ensures(Contract.Result<Maybe<TResult>>() != null);

            return IsSome ? other : Maybe<TResult>.None;
        }

        #endregion

        #region Monadic lifting operators

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-39-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
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

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
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

#if !NO_CCCHECK_SUPPRESSIONS
        [SuppressMessage("Microsoft.Contracts", "Suggestion-62-0",
            Justification = "[CodeContracts] Unrecognized precondition by CCCheck.")]
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

            if (IsNone)
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
