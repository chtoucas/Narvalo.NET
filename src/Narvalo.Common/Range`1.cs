// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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
            // REVIEW: Strict range? Do we allow for equality?
            Enforce.LessThanOrEqualTo(lowerEnd, upperEnd, "lowerEnd");

            _lowerEnd = lowerEnd;
            _upperEnd = upperEnd;
        }

        public T LowerEnd { get { return _lowerEnd; } }

        public T UpperEnd { get { return _upperEnd; } }

        /// <summary>
        /// Returns <c>true</c> if the value is included in the range. 
        /// </summary>
        /// <remarks>Range borders are included in the comparison.</remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure]
        public bool Includes(T value)
        {
            return value.CompareTo(LowerEnd) >= 0
                && value.CompareTo(UpperEnd) <= 0;
        }

        /// <summary>
        /// Returns <c>true</c> if the value is included in the range. 
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
            Contract.Ensures(Contract.Result<string>() != null);

            return Format.CurrentCulture(
                "LowerEnd({0}) - UpperEnd({1})",
                LowerEnd.ToString(),
                UpperEnd.ToString());
        }
    }

    /// <content>
    /// Implements the <c>IEquatable&lt;Range&lt;T&gt;&gt;</c> interface.
    /// </content>
    public partial struct Range<T>
    {
        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static bool operator ==(Range<T> left, Range<T> right)
        {
            return left.Equals(right);
        }

        [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
            Justification = "[Ignore] An explicit conversion operator must be static.")]
        public static bool operator !=(Range<T> left, Range<T> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Range<T> other)
        {
            return LowerEnd.Equals(other.LowerEnd)
                && UpperEnd.Equals(other.UpperEnd);
        }

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
            int hash = 17;
            hash = (23 * hash) + LowerEnd.GetHashCode();
            hash = (23 * hash) + UpperEnd.GetHashCode();
            return hash;
        }
    }
}