// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    using System.Diagnostics.CodeAnalysis;

    internal static class Int32Calculator
    {
        // Reproduces the Math.DivRem() method which is not available with PCL:
        // > int rem;
        // > int div = Math.DivRem(m, n, out rem);
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Mimic the behaviour of Math.DivRem().")]
        public static int DivRem(int dividend, int divisor, out int remainder)
        {
            int q = dividend / divisor;
            // NB: remainder = m % n is slower.
            remainder = dividend - q * divisor;
            return q;
        }
    }
}
