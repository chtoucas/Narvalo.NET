// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics.Contracts;

#if CONTRACTS_FULL // FIXME: Contract visibility.
    public
#else
    internal
#endif
        static class PredicateFor
    {
        [Pure]
        public static bool Range<T>(T lowerEnd, T upperEnd)
            where T : struct, IComparable<T>
            => lowerEnd.CompareTo(upperEnd) <= 0;
    }
}
