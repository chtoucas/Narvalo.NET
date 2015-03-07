// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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
    }
}
