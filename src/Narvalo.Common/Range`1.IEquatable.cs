namespace Narvalo
{
    public partial struct Range<T>
    {
        /// <summary />
        public static bool operator ==(Range<T> left, Range<T> right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Range<T> left, Range<T> right)
        {
            return !left.Equals(right);
        }

        /// <summary />
        public bool Equals(Range<T> other)
        {
            return LowerEnd.Equals(other.LowerEnd)
                && UpperEnd.Equals(other.UpperEnd);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (!(obj is Range<T>)) {
                return false;
            }

            return Equals((Range<T>)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            return LowerEnd.GetHashCode() ^ UpperEnd.GetHashCode();
        }
    }
}