// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public partial struct Span<T> : IEquatable<Span<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        private readonly T _lowerEnd;
        private readonly T _upperEnd;

        public Span(T lowerEnd, T upperEnd)
        {
            // REVIEW: Strict range? Do we allow for equality?
            Guard.LessThanOrEqualTo(lowerEnd, upperEnd, "lowerEnd");

            _lowerEnd = lowerEnd;
            _upperEnd = upperEnd;
        }

        public T LowerEnd { get { return _lowerEnd; } }

        public T UpperEnd { get { return _upperEnd; } }

        [Pure]
        public bool Includes(T value)
        {
            return value.CompareTo(LowerEnd) >= 0
                && value.CompareTo(UpperEnd) <= 0;
        }

        [Pure]
        public bool Includes(Span<T> range)
        {
            return range.LowerEnd.CompareTo(LowerEnd) >= 0
                && range.UpperEnd.CompareTo(UpperEnd) <= 0;
        }

        public override string ToString()
        {
            Contract.Ensures(Contract.Result<string>() != null);

            return Format.CurrentCulture(
                "LowerEnd({0}) - UpperEnd({1})",
                LowerEnd.ToString(),
                UpperEnd.ToString());
        }
    }

    public partial struct Span<T>
    {
        public static bool operator ==(Span<T> left, Span<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Span<T> left, Span<T> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Span<T> other)
        {
            return LowerEnd.Equals(other.LowerEnd)
                && UpperEnd.Equals(other.UpperEnd);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Span<T>))
            {
                return false;
            }

            return Equals((Span<T>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // FIXME: Use EqualityComparer.
                int hash = 17;
                hash = 23 * hash + LowerEnd.GetHashCode();
                hash = 23 * hash + UpperEnd.GetHashCode();
                return hash;
            }
        }
    }
}