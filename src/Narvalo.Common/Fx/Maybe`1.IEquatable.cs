namespace Narvalo.Fx
{
    using System.Collections.Generic;

    public partial class Maybe<T>
    {
        /// <summary />
        public static bool operator ==(Maybe<T> left, T right)
        {
            if (ReferenceEquals(left, null)) {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(T left, Maybe<T> right)
        {
            if (ReferenceEquals(left, null)) {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            if (ReferenceEquals(left, null)) {
                return ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, T right)
        {
            return !(left == right);
        }

        /// <summary />
        public static bool operator !=(T left, Maybe<T> right)
        {
            return !(left == right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !(left == right);
        }

        /// <summary />
        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(T other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            // "this" n'est jamais null si on arrive ici.
            if (ReferenceEquals(other, null)) {
                return false;
            }

            // "this" ne contient pas de valeur.
            if (!_isSome) {
                return false;
            }

            // Retourne vrai si "this" contient la valeur "other".
            return comparer.Equals(_value, other);
        }

        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Maybe<T> other, IEqualityComparer<T> comparer)
        {
            Require.NotNull(comparer, "comparer");

            // "this" n'est jamais null si on arrive ici.
            if (ReferenceEquals(other, null)) {
                return false;
            }

            // "this" et "other" sont différents car l'un est vide et l'autre ne l'est pas.
            if (_isSome != other._isSome) {
                return false;
            }

            // Les deux options sont vides.
            if (!_isSome) {
                return true;
            }

            // Les deux options contiennent la même valeur.
            return comparer.Equals(_value, other._value);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            // "this" n'est jamais null si on arrive ici.
            if (ReferenceEquals(obj, null)) {
                return false;
            }

            // REVIEW: Une option vide est toujours égale à l'unité.
            //if (obj is Unit) {
            //    return !_isSome;
            //}

            var option = obj as Maybe<T>;
            if (ReferenceEquals(option, null)) {
                return Equals(option);
            }

            if (obj is T) {
                return Equals((T)obj);
            }

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

            return _isSome ? comparer.GetHashCode(_value) : 0;
        }
    }
}
