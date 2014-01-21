namespace Narvalo.Fx
{
    using System;

    public partial struct Identity<T>
    {
        /// <summary />
        public static bool operator ==(Identity<T> left, Identity<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Identity<T> left, Identity<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(T other)
        {
            return ReferenceEquals(_value, other);
        }

        /// <summary />
        public bool Equals(Identity<T> other)
        {
            return ReferenceEquals(_value, other._value);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Identity<T>)) {
                return false;
            }

            return Equals((Identity<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _value != null ? _value.GetHashCode() : 0;    
        }
    }
}
