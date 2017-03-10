// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Finance.Allocators;

    internal static class Number
    {
        // Reproduces the Math.DivRem() method which is not available with PCL:
        // > int remainder;
        // > int q = Math.DivRem(dividend, divisor, out remainder);
        public static int DivRem(int dividend, int divisor, out int remainder)
        {
            Debug.Assert(divisor != 0);

            int q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }

        public static long DivRem(long dividend, long divisor, out long remainder)
        {
            Debug.Assert(divisor != 0);

            long q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }

        public static Allocation<int> Allocate(int value, int count)
        {
            int q = DivRem(value, count, out int rem);

            //var seq = Enumerable.Repeat(q, count);

            return Allocation.Create(value, q, rem, count);
        }

        public static Allocation<long> Allocate(long value, int count)
        {
            long q = DivRem(value, count, out long rem);

            //var seq = Enumerable.Repeat(q, count);

            return Allocation.Create(value, q, rem, count);
        }

        // Divide value into n copies of value / n:
        //   value = nq + r = (n - r) q + r (q + 1) where q = value / n.
        // First returns the high value r times, then the low value n - r times.
        public static IEnumerable<int> DivideEvenly(int value, int count)
        {
            Debug.Assert(count > 1);

            int q = DivRem(value, count, out int rem);
            int h = q + 1;

            for (var i = 0; i < count; i++)
            {
                yield return i < rem ? h : q;
            }
        }

        public static IEnumerable<long> DivideEvenly(long value, int count)
        {
            Debug.Assert(count > 1);

            long q = DivRem(value, count, out long rem);
            long h = q + 1;

            for (var i = 0; i < count; i++)
            {
                yield return i < rem ? h : q;
            }
        }
    }
}
