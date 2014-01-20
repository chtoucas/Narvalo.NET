namespace Narvalo.Fx
{
    using System;

    public partial struct Failure<T>
    {
        /// <summary />
        public static bool operator ==(Failure<T> left, Failure<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Failure<T> left, Failure<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(T other)
        {
            if (other == null) { return false; }

            return _exception.Equals(other);
        }

        /// <summary />
        public bool Equals(Failure<T> other)
        {
            return _exception.Equals(other._exception);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Failure<T>)) {
                return false;
            }

            return Equals((Failure<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _exception.GetHashCode();
        }
    }
}