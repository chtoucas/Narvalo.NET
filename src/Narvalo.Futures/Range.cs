// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Range
    {
        public static Range<T> Of<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
            => new Range<T>(lowerEnd, upperEnd);

        [Pure]
        public static bool Validate<T>(T lowerEnd, T upperEnd)
            where T : struct, IComparable<T>
            => lowerEnd.CompareTo(upperEnd) <= 0;
    }
}
