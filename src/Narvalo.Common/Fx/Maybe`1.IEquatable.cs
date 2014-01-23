namespace Narvalo.Fx
{
    public partial struct Maybe<T>
    {
        /// <summary />
        public static bool operator ==(Maybe<T> left, Maybe<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Maybe<T> left, Maybe<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(T other)
        {
            if (!_isSome) { return false; }

            return _value.Equals(other);
        }

        /// <summary />
        public bool Equals(Maybe<T> other)
        {
            if (_isSome != other._isSome) { return false; }
            return
                !_isSome                        // Les deux options sont vides.
                || _value.Equals(other._value); // Les deux options ont la même valeur.
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (obj == null) {
                return false;
            }

            if (!_isSome && obj is Unit) {
                return true;
            }

            if (!(obj is Maybe<T>)) {
                return false;
            }

            return Equals((Maybe<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _isSome ? _value.GetHashCode() : 0;
        }
    }
}
