// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /*!
     * The Maybe Monad
     * ===============
     * 
     * The `Maybe<T>` class is kind of like the `Nullable<T>` class but without any restriction on the underlying type :
     * _it provides a way to tell the absence or the presence of a value_. Taken alone, it might not look that useful,
     * we could simply use a nullable for value types and a `null` for reference types. That's where the monad
     * comes into play. The `Maybe<T>` satisfies a very simple grammar, known as the monad laws, from which 
     * derives a rich vocabulary.
     * 
     * What I like the most about this class is that it helps to clearly express our intent with a very clean syntax.
     * For instance, considering the following methods
     * '''
     * string GetPhoneNumber() { ... }
     * Maybe<string> MayGetPhoneNumber() { ... }
     * '''
     * I believe that the second version makes it clearer that we might actually not know the phone number.
     * It then makes easy to write what we do in either cases:
     * '''
     * MayGetPhoneNumber().OnNone( ... ).OnSome( ... );
     * '''
     * 
     * The most obvious weakness of the Maybe monad is that it is all black or all white. In some circumstances,
     * I might like to be able to be able to give an explanation for the absence of a value. In fact, that's one
     * of the purpose of the Either monad.
     * 
     * The main defects of this implementation are :
     * + It is a reference type,
     * + An instance is mutable for reference types,
     * + (more to be added later, I am sure there are other problems)
     * 
     * This class is sometimes referred to as the Option type.
     * 
     * ### Naming convention ###
     * 
     * We prefix with "May" all methods that return a Maybe instance.
     * 
     * 
     * Design of `Maybe<T>`
     * --------------------
     * 
     * ### Class vs structure ###
     * 
     * + _Argument towards a structure:_
     * C# then guarantees that an instance is never null, which seems like a good thing here.
     * Isn't it one of the reasons why we decided in the first place to create such a class?
     * 
     * + _Argument against a structure:_
     * An instance is mutable if `T` is a reference type. This should always raise a big warning.
     *   
     * In the end, creating a mutable structure seems way too hazardous.
     *   
     * ### Maybe<T> vs Nullable<T> ###
     * 
     * Most of the time, for value types, `T?` offers a much better alternative. To discourage the use
     * of the `Maybe<T>` when a nullable would make a better fit, we shall create a FxCop rule.
     * 
     * 
     * References
     * ----------
     * 
     * + [Wikipedia]: http://en.wikipedia.org/wiki/Monad_(functional_programming)#The_Maybe_monad
     * + [Haskell]: http://hackage.haskell.org/package/base-4.6.0.1/docs/Data-Maybe.html
     * 
     * Alternative implementations in C#:
     * + [iSynaptic.Commons]: https://github.com/iSynaptic/iSynaptic.Commons/blob/master/Application/iSynaptic.Commons/Maybe.cs
     */

    /// <summary>
    /// Represents a value that is either a single value of type T, or no value at all.
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix",
        Justification = "Maybe<T> only pretends to be a collection.")]
    public sealed partial class Maybe<T> : IMaybe<T>, IEnumerable<T>, IEquatable<Maybe<T>>, IEquatable<T> 
    {
        static readonly Maybe<T> None_ = new Maybe<T>();

        readonly bool _isSome;
        readonly T _value;

        /*!
         * Constructor
         * -----------
         * 
         * All constructors are made private. Having complete control over the creation of an instance
         * helps us to ensure that `value` is never `null` when passed to the constructor. This is exactly
         * what we do in the static method `Maybe<T>.η(value)`.
         * 
         * To make things simpler, we provide two public factory methods:
         * + `Maybe.Create<T>(value)`,
         * + `Maybe.Create<T?>(value)`,
         * and one static property `Maybe<T>.None` to reference a Maybe that has no value.
         */

        /// <summary>
        /// Initializes a new instance of <see cref="Narvalo.Fx.Maybe&lt;T&gt;" /> that does not hold any value.
        /// </summary>
        Maybe()
        {
            _isSome = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Narvalo.Fx.Maybe&lt;T&gt;" /> using the specified value. 
        /// </summary>
        /// <param name="value">The underlying value.</param>
        Maybe(T value)
        {
            _value = value;
            _isSome = true;
        }

        /// <summary>
        /// Returns an instance of <see cref="Narvalo.Fx.Maybe&lt;T&gt;" /> that does not hold any value.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "A generic version would not improve the readability.")]
        public static Maybe<T> None { get { return None_; } }

        /// <summary>
        /// Returns true if the object does not have an underlying value, false otherwise.
        /// </summary>
        public bool IsNone { get { return !_isSome; } }

        /// <summary>
        /// Returns true if the object contains a value, false otherwise.
        /// </summary>
        public bool IsSome { get { return _isSome; } }

        /// <summary>
        /// Returns the underlying value.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The object does not contain any value.</exception>
        public T Value
        {
            get
            {
                if (!_isSome) {
                    throw new InvalidOperationException(SR.Maybe_NoneHasNoValue);
                }

                return _value;
            }
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
        /// <param name="defaultValue">A default value to be used if if there is no underlying value.</param>
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
    }
}
