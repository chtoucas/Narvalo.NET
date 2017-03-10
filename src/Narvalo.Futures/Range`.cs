﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    public static class Range
    {
        public static Range<T> Of<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
            => new Range<T>(lowerEnd, upperEnd);
    }

    /// <summary>
    /// Represents a range of values.
    /// </summary>
    /// <typeparam name="T">The underlying type of the values.</typeparam>
    public partial struct Range<T> : IEquatable<Range<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        public Range(T lowerEnd, T upperEnd)
        {
            Require.Range(lowerEnd.CompareTo(upperEnd) <= 0, nameof(lowerEnd));

            LowerEnd = lowerEnd;
            UpperEnd = upperEnd;
        }

        public T LowerEnd { get; }

        public T UpperEnd { get; }

        public bool IsDegenerate => LowerEnd.Equals(UpperEnd);

        /// <summary>
        /// Returns true if the value is contained in the range.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public bool Contains(T value)
            => value.CompareTo(LowerEnd) >= 0 && value.CompareTo(UpperEnd) <= 0;

        /// <summary>
        /// Returns true if the value is included in the range.
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="range"></param>
        /// <returns></returns>
        [Pure]
        public bool Includes(Range<T> range)
            => range.LowerEnd.CompareTo(LowerEnd) >= 0 && range.UpperEnd.CompareTo(UpperEnd) <= 0;

        public override string ToString()
            => "LowerEnd=" + LowerEnd.ToString() + "; UpperEnd=" + UpperEnd.ToString();
    }

    // Implements the IEquatable<Range<T>> interface.
    public partial struct Range<T>
    {
        public static bool operator ==(Range<T> left, Range<T> right) => left.Equals(right);

        public static bool operator !=(Range<T> left, Range<T> right) => !left.Equals(right);

        public bool Equals(Range<T> other)
            => LowerEnd.Equals(other.LowerEnd) && UpperEnd.Equals(other.UpperEnd);

        public override bool Equals(object obj) => (obj is Range<T>) && Equals((Range<T>)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(LowerEnd.GetHashCode(), UpperEnd.GetHashCode());
    }
}