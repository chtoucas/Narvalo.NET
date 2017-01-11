// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    internal static class Integer
    {
        // Reproduces the Math.DivRem() method which is not available with PCL:
        // > int remainder;
        // > int q = Math.DivRem(dividend, divisor, out remainder);
        public static int DivRem(int dividend, int divisor, out int remainder)
        {
            int q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }

        public static long DivRem(long dividend, long divisor, out long remainder)
        {
            long q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower.
            remainder = dividend - q * divisor;
            return q;
        }
    }
}
