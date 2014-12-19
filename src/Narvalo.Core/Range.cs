﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    public static class Range
    {
#if !NO_HACK
        [SuppressMessage("Microsoft.Contracts", "Requires",
            Justification = "CCCheck does not seem to be able to prove a Require in conjunction with IComparable<T>.")]
#endif
        public static Range<T> Create<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            Contract.Requires(lowerEnd.CompareTo(upperEnd) <= 0);

            return new Range<T>(lowerEnd, upperEnd);
        }

#if !NO_HACK
        [SuppressMessage("Microsoft.Contracts", "Requires",
            Justification = "CCCheck does not seem to be able to prove a Require in conjunction with IComparable<T>.")]
#endif
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
