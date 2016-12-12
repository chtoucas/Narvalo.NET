// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a range of values.
    /// </summary>
    /// <typeparam name="T">The underlying type of the values.</typeparam>
    public partial struct Range<T> : IEquatable<Range<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        private readonly T _lowerEnd;
        private readonly T _upperEnd;

        public Range(T lowerEnd, T upperEnd)
        {
            Require.Range(PredicateFor.Range(lowerEnd, upperEnd), nameof(lowerEnd));

            _lowerEnd = lowerEnd;
            _upperEnd = upperEnd;
        }

        public T LowerEnd => _lowerEnd;

        public T UpperEnd => _upperEnd;

        public bool IsDegenerate => LowerEnd.Equals(UpperEnd);

        /// <summary>
        /// Returns <see langword="true"/> if the value is contained in the range.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public bool Contains(T value)
        {
            return value.CompareTo(LowerEnd) >= 0
                && value.CompareTo(UpperEnd) <= 0;
        }

        [Pure]
        [ExcludeFromCodeCoverage(Justification = "Obsolete method.")]
        [Obsolete("Use Range<T>.Contains() instead.")]
        public bool Includes(T value) => Contains(value);

        /// <summary>
        /// Returns <see langword="true"/> if the value is included in the range.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="range"></param>
        /// <returns></returns>
        [Pure]
        public bool Includes(Range<T> range)
        {
            return range.LowerEnd.CompareTo(LowerEnd) >= 0
                && range.UpperEnd.CompareTo(UpperEnd) <= 0;
        }

        public override string ToString()
        {
            Warrant.NotNull<string>();

            return Format.Current("LowerEnd({0}) - UpperEnd({1})", LowerEnd, UpperEnd);
        }
    }

    // Implements the IEquatable<Range<T>> interface.
    public partial struct Range<T>
    {
        public static bool operator ==(Range<T> left, Range<T> right) => left.Equals(right);

        public static bool operator !=(Range<T> left, Range<T> right) => !left.Equals(right);

        public bool Equals(Range<T> other)
            => LowerEnd.Equals(other.LowerEnd)
                && UpperEnd.Equals(other.UpperEnd);

        public override bool Equals(object obj)
        {
            if (!(obj is Range<T>))
            {
                return false;
            }

            return Equals((Range<T>)obj);
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