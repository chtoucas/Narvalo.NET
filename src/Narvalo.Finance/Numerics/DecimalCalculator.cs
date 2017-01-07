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

        public static IEnumerable<decimal> Distribute(decimal value, int parts)
        {
            if (parts == 0) { throw new DivideByZeroException(); }

            decimal q = value / parts;

            for (var i = 0; i < parts - 1; i++)
            {
                yield return q;
            }

            var last = value - (parts - 1) * q;

            yield return last;
        }

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int decimalPlaces,
            int parts,
            RoundingMode mode)
        {
            if (parts == 0) { throw new DivideByZeroException(); }

            decimal q = DecimalRounding.Round(value / parts, decimalPlaces, mode);

            for (var i = 0; i < parts - 1; i++)
            {
                yield return q;
            }

            var last = value - (parts - 1) * q;

            yield return last;
        }

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int decimalPlaces,
            int parts,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            if (parts == 0) { throw new DivideByZeroException(); }

            decimal q = rounding.Round(value / parts, decimalPlaces);

            for (var i = 0; i < parts - 1; i++)
            {
                yield return q;
            }

            var last = value - (parts - 1) * q;

            yield return last;
        }

        public static IEnumerable<decimal> Allocate(decimal amount, RatioArray ratios)
        {
            var len = ratios.Length;
            var dist = new decimal[len];
            var last = amount;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = ratios[i] * amount;
                last -= next;
                yield return next;
            }

            yield return last;
        }

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            int decimalPlaces,
            RatioArray ratios,
            RoundingMode mode)
        {
            var len = ratios.Length;
            var dist = new decimal[len];
            var last = amount;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = DecimalRounding.Round(ratios[i] * amount, decimalPlaces, mode);
                last -= next;
                yield return next;
            }

            yield return last;
        }

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            int decimalPlaces,
            RatioArray ratios,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            var len = ratios.Length;
            var dist = new decimal[len];
            var last = amount;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = rounding.Round(ratios[i] * amount, decimalPlaces);
                last -= next;
                yield return next;
            }

            yield return last;
        }

        #endregion
    }
}
