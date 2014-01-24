namespace Narvalo.Fx
{
    using System.Collections.Generic;

    public partial struct Maybe<T>
    {
        /// <summary />
        public static bool operator ==(Maybe<T> left, T right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(T left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, T right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(T left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
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

            if (_isSome != other._isSome) { return false; }

            return
                !_isSome                                  // Les deux options sont vides.
                || comparer.Equals(_value, other._value); // Les deux options ont la même valeur.
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }
            else if (obj is Unit) {
                return !_isSome;
            }
            else if (obj is Maybe<T>) {
                return Equals((Maybe<T>)obj);
            }
            else if (obj is T) {
                return Equals((T)obj);
            }
            else {
                return false;
            }
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
