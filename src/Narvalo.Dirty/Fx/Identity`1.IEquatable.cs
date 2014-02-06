namespace Narvalo.Fx
{
    using System.Collections.Generic;

    public partial class Identity<T>
    {
        /// <summary />
        public bool Equals(Identity<T> other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
        public bool Equals(Identity<T> other, IEqualityComparer<T> comparer)
        {
            if (ReferenceEquals(other, null)) {
                return _value == null;
            }

            if (_value == null) {
                return other._value == null;
            }

            return comparer.Equals(_value, other._value);
        }

        /// <summary />
        public bool Equals(T other)
        {
            return Equals(other, EqualityComparer<T>.Default);
        }

        /// <summary />
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
                return _value == null;
            }

            if (other is T) {
                return Equals((T)other, comparer);
            }

            // Usually, we test the condition obj.GetType() == this.GetType(), in case "this" or "obj" 
            // is an instance of a derived type, something that can not happen here because Identity is sealed.
            return Equals(other as Identity<T>, comparer);
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

            return _value != null ? comparer.GetHashCode(_value) : 0;
        }

    }
}
