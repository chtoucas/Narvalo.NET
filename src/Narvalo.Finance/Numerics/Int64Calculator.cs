// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System.Collections.Generic;

    public static class Int64Calculator
    {
        /// <see cref="Int32Calculator.Divide(int, int, out int)">.
        internal static long Divide(long m, long n, out long rem)
        {
            long q = m / n;
            rem = m - q * n;
            return q;
        }

        /// <see cref="Int32Calculator.Divide(int, int)">.
        public static DivCollection<long> Divide(long amount, int parts)
        {
            long rem;
            long q = Divide(amount, parts, out rem);

            return DivCollection<long>.Create(q, rem, parts);
        }

        /// <see cref="Int32Calculator.Distribute(int, int)">.
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
