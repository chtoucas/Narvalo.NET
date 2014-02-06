// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System.Collections.Generic;

    /*!
     * Referential equality and structural equality
     * --------------------------------------------
     * 
     * We redefine the `Equals()` method to allow for structural equality for reference types that follow
     * value type semantics. Nevertheless, we do not change the meaning of the equality operators (== and !=)
     * which continue to test referential equality, behaviour expected by the .NET framework for all reference types.
     * I might change my mind on this and try to make `Maybe<T>` behave like `Nullable<T>`.
     * As a matter of convenience, we also implement the `IEquatable<T>` interface.
     * Another (abandonned) possibility has been to implement the IStructuralEquatable interface.
     * 
     * ### Sample rules ###
     * 
     * '''
     * Maybe<T>.None != null
     * Maybe<T>.None.Equals(null)
     *   
     * Maybe.Create(1) != Maybe.Create(1)
     * Maybe.Create(1).Equals(Maybe.Create(1))
     *   
     * Maybe.Create(1) != 1
     * Maybe.Create(1).Equals(1)
     * '''
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
            if (ReferenceEquals(other, null)) {
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

            // Usually, we test the condition obj.GetType() == this.GetType(), in case "this" or "obj" 
            // is an instance of a derived type, something that can not happen here because Maybe is sealed.
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
}
