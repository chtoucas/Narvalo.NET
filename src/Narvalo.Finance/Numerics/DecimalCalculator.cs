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

    // Notes:
    // With a PCL, we can not use Decimal.Round, instead we have Math.Round.
    //
    // - PEAA p.494
    // - Double.MaxValue > Decimal.MaxValue > Int64.MaxValue > Int32.MaxValue
    // -  Check if this is correct for negative values.
    //
    // Math.Floor (down), Math.Ceiling (up), Math.Truncate (zero), Math.Round?
    // - Floor = partie entière (n <= x < n +1); partie fractionnaire ({x} = x - |x|)
    // - Ceiling (n - 1 < x <= n)
    // - Truncate
    // - Round(ToEven)
    // - Round(AwayFromZero)
    // Remark: Integer division rounds toward zero.
    // - Explain how to reverse or randomize the distribution.
    public static class DecimalCalculator
    {
        //private static readonly Dictionary<int, decimal> s_Corrections = new Dictionary<int, decimal>
        //{
        //    { 0, 0M },
        //    { 1, 0.1M },
        //    { 2, 0.01M },
        //    { 3, 0.001M },
        //    { 4, 0.0001M },
        //    { 5, 0.00001M },
        //};

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
    }
}
