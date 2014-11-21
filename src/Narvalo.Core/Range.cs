// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Range
    {
        public static Range<T> Create<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            Contract.Requires(lowerEnd.CompareTo(upperEnd) <= 0);

            return new Range<T>(lowerEnd, upperEnd);
        }

        public static Range<DateTime> OneDay(int year, int month, int day)
        {
            Contract.Requires(year >= 1);
            Contract.Requires(month >= 1);
            Contract.Requires(day >= 1);
            Contract.Requires(year <= 9999);
            Contract.Requires(month <= 12);

            var lowerEnd = new DateTime(year, month, day);
            var upperEnd = lowerEnd.AddHours(23).AddMinutes(59).AddSeconds(59);

            return Create(lowerEnd, upperEnd);
        }
    }
}
