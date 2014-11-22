// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public partial struct Range<T> : IEquatable<Range<T>>
        where T : struct, IEquatable<T>, IComparable<T>
    {
        readonly T _lowerEnd;
        readonly T _upperEnd;

        public Range(T lowerEnd, T upperEnd)
        {
            // REVIEW: Strict range? Do we allow for equality?
            Require.CheckRange(lowerEnd.CompareTo(upperEnd) <= 0, upperEnd, "upperEnd", SR.Range_LowerEndNotLesserThanUpperEnd);

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
        public bool Includes(Range<T> range)
        {
            return range.LowerEnd.CompareTo(LowerEnd) >= 0
                && range.UpperEnd.CompareTo(UpperEnd) <= 0;
        }

        /// <summary />
        public override string ToString()
        {
            return String.Format(
                CultureInfo.InvariantCulture,
                "LowerEnd={0};UpperEnd={1}",
                LowerEnd.ToString(),
                UpperEnd.ToString());
        }
    }
}