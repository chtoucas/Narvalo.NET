namespace Narvalo.Fx
{
    using System;

    public partial struct Identity<T> : IEquatable<Identity<T>>, IEquatable<T>
    {
        readonly T _value;

        public Identity(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }

        #region IEquatable<T>

        /// <summary />
        public bool Equals(T other)
        {
            return ReferenceEquals(Value, other);
        }

        #endregion

        #region IEquatable<Identity<T>>

        /// <summary />
        public bool Equals(Identity<T> other)
        {
            return ReferenceEquals(Value, other.Value);
        }

        #endregion

        #region Comparaisons

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
            return _value != null ? Value.GetHashCode() : 0;
        }

        #endregion
    }
}
