namespace Narvalo
{
    using System;
    using System.Globalization;

    public struct Range<T> : IEquatable<Range<T>>
        where T : IEquatable<T>, IComparable<T>
    {
        readonly T _lowerEnd;
        readonly T _upperEnd;

        public Range(T lowerEnd, T upperEnd)
        {
            Requires.LessThanOrEqualTo(lowerEnd, upperEnd, SR.Range_LowerEndNotLesserThanUpperEnd);

            _lowerEnd = lowerEnd;
            _upperEnd = upperEnd;
        }

        public T LowerEnd { get { return _lowerEnd; } }
        
        public T UpperEnd { get { return _upperEnd; } }

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

        public bool Includes(T value)
        {
            return value.CompareTo(LowerEnd) >= 0
                && value.CompareTo(UpperEnd) <= 0;
        }

        public bool Includes(Range<T> range)
        {
            return range.LowerEnd.CompareTo(LowerEnd) >= 0
                && range.UpperEnd.CompareTo(UpperEnd) <= 0;
        }

        #region IEquatable<Range<T>>

        /// <summary />
        public bool Equals(Range<T> other)
        {
            return LowerEnd.Equals(other.LowerEnd)
                && UpperEnd.Equals(other.UpperEnd);
        }

        #endregion

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
            // FIXME
            return LowerEnd.GetHashCode() ^ UpperEnd.GetHashCode();
        }

        /// <summary />
        public override string ToString()
        {
            // FIXME
            return String.Format(
                CultureInfo.InvariantCulture,
                "LowerEnd={0};UpperEnd={1}",
                LowerEnd.ToString(), 
                UpperEnd.ToString());
        }
    }
}