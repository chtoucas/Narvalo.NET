// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

    public static class Span
    {
        public static Span<T> Of<T>(T lowerEnd, T upperEnd)
            where T : struct, IEquatable<T>, IComparable<T>
        {
            //Demand.Range(lowerEnd.CompareTo(upperEnd) <= 0);
            //Contract.Requires(lowerEnd.CompareTo(upperEnd) <= 0);

            return new Span<T>(lowerEnd, upperEnd);
        }
    }
}
