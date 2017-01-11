// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System.Diagnostics.CodeAnalysis;

    internal static class IntegerCalculator
    {
        // Reproduces the Math.DivRem() method which is not available with PCL:
        // > int remainder;
        // > int q = Math.DivRem(dividend, divisor, out remainder);
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Mimic the behaviour of Math.DivRem().")]
        public static int DivRem(int dividend, int divisor, out int remainder)
        {
            int q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Mimic the behaviour of Math.DivRem().")]
        public static long DivRem(long dividend, long divisor, out long remainder)
        {
            long q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }
    }
}
