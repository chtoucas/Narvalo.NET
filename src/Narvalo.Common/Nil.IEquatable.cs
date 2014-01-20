namespace Narvalo
{
    public partial struct Nil
    {
        /// <summary />
        public static bool operator ==(Nil left, Nil right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Nil left, Nil right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(Nil other)
        {
            return _successful || _message.Equals(other._message);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Nil)) {
                return false;
            }

            return Equals((Nil)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return _successful ? 0 : _message.GetHashCode();
        }
    }
}
