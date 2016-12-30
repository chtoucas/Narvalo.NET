// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System.Collections.Generic;

    public static class Int64Calculator
    {
        public static long Divide(long dividend, long divisor, out long remainder)
        {
            long q = dividend / divisor;
            remainder = dividend - q * divisor;
            return q;
        }

        public static DivCollection<long> Divide(long amount, int parts)
        {
            long rem;
            long q = Divide(amount, parts, out rem);

            return DivCollection<long>.Create(q, rem, parts);
        }

        public static IEnumerable<long> Distribute(long amount, long parts)
        {
            long rem;
            long q = Divide(amount, parts, out rem);
            long h = q + 1;

            for (var i = 0; i < parts; i++)
            {
                yield return i < rem ? h : q;
            }
        }
    }
}
