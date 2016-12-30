// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System.Collections.Generic;

    public static class Int32Calculator
    {
        // Reproduces the Math.DivRem() method which is not available with PCL:
        // > int rem;
        // > int div = Math.DivRem(m, n, out rem);
        public static int Divide(int dividend, int divisor, out int remainder)
        {
            int q = dividend / divisor;
            // NB: remainder = m % n is slower.
            remainder = dividend - q * divisor;
            return q;
        }

        public static DivCollection<int> Divide(int amount, int parts)
        {
            int rem;
            int q = Divide(amount, parts, out rem);

            return DivCollection<int>.Create(q, rem, parts);
        }

        // Distribute an integer (value) across n copies of amount / n:
        //   amount = nq + r = (n - r) q + r (q + 1) where q = amount / n.
        // First returns the high value r times, then the low value n - r times.
        public static IEnumerable<int> Distribute(int amount, int parts)
        {
            int rem;
            int q = Divide(amount, parts, out rem);
            int h = q + 1;

            for (var i = 0; i < parts; i++)
            {
                yield return i < rem ? h : q;
            }
        }
    }
}
