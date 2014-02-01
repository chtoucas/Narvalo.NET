namespace Narvalo.Fx
{
    using System.Collections.Generic;

    /* Referential equality and structural equality
     * --------------------------------------------
     * 
     * We redefine the `Equals()` method to allow for structural equality.
     * Nevertheless, we do not change the meaning of the equality operators (== and !=) which continue 
     * to test the referential equality, behaviour expected by the .NET framework for all reference types.
     * Another (abandonned) possibility would have been to implement the IStructuralEquatable interface.
     */

    public partial class Maybe<T>
    {
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

        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            if (ReferenceEquals(other, null)) {
                return IsNone;
            }

            if (IsNone) {
                return other.IsNone;
            }

            return comparer.Equals(_value, other._value);
        }

        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            return Equals(Maybe.Create(other), comparer);
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
                return IsNone;
            }

            if (other is T) {
                return Equals((T)other, comparer);
            }

            // Usually, we test the condition obj.GetType() == this.GetType() in case of "this" or "obj" 
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

            return _value == null ? 0 : comparer.GetHashCode(_value);
        }
    }
}
