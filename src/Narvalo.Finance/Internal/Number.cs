// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Diagnostics;

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
    }
}
