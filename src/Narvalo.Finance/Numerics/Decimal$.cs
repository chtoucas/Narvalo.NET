// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    internal static class DecimalExtensions
    {
        //private const int SIGN_MASK = unchecked((int)0x80000000);
        //private const byte DECIMAL_NEG = 0x80;
        //private const byte DECIMAL_ADD = 0x00;
        private const int SCALE_MASK = 0x00FF0000;
        private const int SCALE_SHIFT = 16;
        // The maximum power of 10 that a 32 bit integer can store.
        //private const int MAX_INT_SCALE = 9;

        // https://msdn.microsoft.com/en-us/library/system.decimal.getbits.aspx
        // GetBits() returns an array
        // - The first, second and third elements represent the low, middle, and high 32 bits
        //   of the 96-bit integer number.
        // - The fourth element contains the scale factor and sign:
        //   * bits 0 to 15, the lower word, are unused and must be zero.
        //   * bits 16 to 23 must contain an exponent between 0 and 28 inclusive,
        //     which indicates the power of 10 to divide the integer number.
        //   * bits 24 to 30 are unused and must be zero.
        //   * bit 31 contains the sign: 0 mean positive, and 1 means negative.
        public static int GetScale(this decimal @this)
        {
            int flags = Decimal.GetBits(@this)[3];
            // Bits 16 to 23 contains an exponent between 0 and 28, which indicates the power
            // of 10 to divide the integer number.
            //return (flags >> 16) & 0xFF;
            return (flags & SCALE_MASK) >> SCALE_SHIFT;
        }

        //public static bool GetSign(this decimal @this)
        //{
        //    return @this > 0;
        //    //int flags = Decimal.GetBits(@this)[3];
        //    //// The last bit contains the sign: 0 means positive, and 1 means negative.
        //    //return (flags & SIGN_MASK) != 0;
        //}
    }
}
