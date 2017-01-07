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

        #region Distribution.

        public static IEnumerable<decimal> Distribute(decimal value, int parts)
        {
            Require.Range(parts >= 0, nameof(parts));
            if (parts == 0) { throw new DivideByZeroException(); }

            var seq = DistributeCore(value, parts);

            foreach (var _ in seq) yield return _;
        }

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int decimalPlaces,
            int parts,
            RoundingMode mode)
        {
            Require.Range(parts >= 0, nameof(parts));
            if (parts == 0) { throw new DivideByZeroException(); }

            var seq = DistributeCore(value, parts);

            foreach (var _ in seq) yield return DecimalRounding.Round(_, decimalPlaces, mode);
        }

        public static IEnumerable<decimal> Distribute(
            decimal value,
            int decimalPlaces,
            int parts,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            Require.Range(parts >= 0, nameof(parts));
            if (parts == 0) { throw new DivideByZeroException(); }

            var seq = DistributeCore(value, parts);

            foreach (var _ in seq) yield return rounding.Round(_, decimalPlaces);
        }

        private static IEnumerable<decimal> DistributeCore(decimal value, int parts)
        {
            Demand.Range(parts > 0);

            decimal q = value / parts;

            for (var i = 0; i < parts - 1; i++)
            {
                yield return q;
            }

            var last = value - (parts - 1) * q;

            yield return last;
        }

        #endregion

        #region Allocation.

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
            var seq = Allocate(amount, ratios);

            foreach (var _ in seq) yield return DecimalRounding.Round(_, decimalPlaces, mode);
        }

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            int decimalPlaces,
            RatioArray ratios,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            var seq = Allocate(amount, ratios);

            foreach (var _ in seq) yield return rounding.Round(_, decimalPlaces);
        }

        #endregion
    }
}
