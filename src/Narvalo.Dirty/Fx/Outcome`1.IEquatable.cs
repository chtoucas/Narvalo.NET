namespace Narvalo.Fx
{
    public partial struct Outcome<T>
    {
        /// <summary />
        public static bool operator ==(Outcome<T> left, Outcome<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Outcome<T> left, Outcome<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(Outcome<T> other)
        {
            return (_successful && _value.Equals(other._value))
                || (!_successful && _exception.Equals(other._exception));
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Outcome<T>)) {
                return false;
            }

            return Equals((Outcome<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _successful ? _value.GetHashCode() : _exception.GetHashCode();
        }
    }
}
