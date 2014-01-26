namespace Narvalo.Fx
{
    using System.Collections.Generic;

    // FIXME: C'est incorrect.
    public partial class Maybe<T>
    {
        /// <summary />
        public static bool operator ==(Maybe<T> left, T right)
        {
            return !ReferenceEquals(left, null) || left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(T left, Maybe<T> right)
        {
            return !ReferenceEquals(left, null) && left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return !ReferenceEquals(left, null) && left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, T right)
        {
            return ReferenceEquals(left, null) || !left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(T left, Maybe<T> right)
        {
            return ReferenceEquals(left, null) || !left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return ReferenceEquals(left, null) || !left.Equals(right);
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

            if (!_isSome) { return false; }

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

            if (ReferenceEquals(other, null)) {
                return false;
            }

            if (_isSome != other._isSome) {
                return false;
            }

            return
                !_isSome                                  // Les deux options sont vides.
                || comparer.Equals(_value, other._value); // Les deux options ont la même valeur.
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) {
                return false;
            }

            if (obj is Unit) {
                return !_isSome;
            }

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
