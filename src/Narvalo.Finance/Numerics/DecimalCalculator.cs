// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    public enum BiasAllocation
    {
        First,
        Last,
        Lowest,
        Highest,
        Pseudorandom,
    }

    // Spread? Partition? Evenly?
    //public enum Distribution
    //{
    //    PseudoUniform,
    //    Single,
    //}

    // - Check if this is correct for negative values.
    // - Explain how to reverse or randomize the distribution.
    public static class DecimalCalculator
    {
        internal static int GetScale(this decimal @this)
        {
            int flags = Decimal.GetBits(@this)[3];
            // Bits 16 to 23 contains an exponent between 0 and 28, which indicates the power
            // of 10 to divide the integer number.
            return (flags & 0x00FF0000) >> 16;
        }

        #region Distribution/Allocation.

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int decimalPlaces,
            int parts,
            MidpointRounding rounding)
        {
            decimal q = Math.Round(value / parts, decimalPlaces, rounding);

            for (var i = 0; i < parts - 1; i++)
            {
                yield return q;
            }

            var last = value - (parts - 1) * q;

            yield return last;
        }

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            int decimalPlaces,
            RatioArray ratios,
            MidpointRounding rounding)
        {
            var len = ratios.Length;
            var dist = new decimal[len];
            var last = amount;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = Math.Round(ratios[i] * amount, decimalPlaces, rounding);
                last -= next;
                yield return next;
            }

            yield return last;
        }

        #endregion
    }
}
