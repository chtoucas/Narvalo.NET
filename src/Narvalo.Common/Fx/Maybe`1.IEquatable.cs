namespace Narvalo.Fx
{
    using System.Collections.Generic;

    public partial class Maybe<T>
    {
        /* Egalité
         * -------
         * 
         * On ne vérifie pas si obj == this, car on veut prétendre que Maybe<T> est immuable. 
         */

        ///// <summary />
        //public static bool operator ==(Maybe<T> left, T right)
        //{
        //    if (ReferenceEquals(left, null)) {
        //        return right == null;
        //    }

        //    return left.Equals(right);
        //}

        ///// <summary />
        //public static bool operator ==(T left, Maybe<T> right)
        //{
        //    if (ReferenceEquals(right, null)) {
        //        return left == null;
        //    }

        //    return right.Equals(left);
        //}

        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            if (ReferenceEquals(left, null)) {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        ///// <summary />
        //public static bool operator !=(Maybe<T> left, T right)
        //{
        //    return !(left == right);
        //}

        ///// <summary />
        //public static bool operator !=(T left, Maybe<T> right)
        //{
        //    return !(left == right);
        //}

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !(left == right);
        }

        ///// <summary />
        //public bool Equals(T other)
        //{
        //    return Equals(other, EqualityComparer<T>.Default);
        //}

        ///// <summary />
        //public bool Equals(T other, IEqualityComparer<T> comparer)
        //{
        //    Require.NotNull(comparer, "comparer");

        //    // "this" n'est jamais null si on arrive ici.
        //    if (other == null) {
        //        return false;
        //    }

        //    if (_value == null) {
        //        return false;
        //    }

        //    // Retourne vrai si "this" contient la valeur "other".
        //    return comparer.Equals(_value, other);
        //}

        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            if (ReferenceEquals(other, null)) {
                return false;
            }

            //// "this" et "other" sont différents car l'un est vide et l'autre ne l'est pas.
            //if (_isSome != other._isSome) {
            //    return false;
            //}

            //// Les deux options sont vides.
            //if (IsNone) {
            //    return true;
            //}

            // Les deux options contiennent la même valeur.
            return comparer.Equals(_value, other._value);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            var option = obj as Maybe<T>;
            if (!ReferenceEquals(option, null)) {
                return Equals(option);
            }

            //if (obj is T) {
            //    return Equals((T)obj);
            //}

            return false;
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

            return ReferenceEquals(_value, null) ? 0 : comparer.GetHashCode(_value);
        }
    }
}
