// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Range
    {
        //[SuppressMessage("Microsoft.Contracts", "Requires",
        //    Justification = "[CodeContracts] CCCheck does not seem to be able to prove a Require in conjunction with IComparable<T>.")]
        public static Range<T> Create<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            Contract.Requires(lowerEnd.CompareTo(upperEnd) <= 0);

            return new Range<T>(lowerEnd, upperEnd);
        }

        //[SuppressMessage("Microsoft.Contracts", "Requires",
        //    Justification = "[CodeContracts] CCCheck does not seem to be able to prove a Require in conjunction with IComparable<T>.")]
        public static Range<DateTime> OneDay(int year, int month, int day)
        {
            Contract.Requires(year >= 1 && year <= 9999);
            Contract.Requires(month >= 1 && month <= 12);
            Contract.Requires(day >= 1);

            var lowerEnd = new DateTime(year, month, day);
            var upperEnd = lowerEnd.AddHours(23).AddMinutes(59).AddSeconds(59);

            Contract.Assume(lowerEnd.CompareTo(upperEnd) <= 0, "This is guaranteed by construction of upperEnd.");

            return Create(lowerEnd, upperEnd);
        }
    }
}
