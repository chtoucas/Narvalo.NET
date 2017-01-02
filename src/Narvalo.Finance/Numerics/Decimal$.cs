// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    internal static class DecimalExtensions
    {
        public static int GetScale(this decimal @this)
        {
            int flags = Decimal.GetBits(@this)[3];
            // Bits 16 to 23 contains an exponent between 0 and 28, which indicates the power
            // of 10 to divide the integer number.
            return (flags & 0x00FF0000) >> 16;
        }
    }
}
