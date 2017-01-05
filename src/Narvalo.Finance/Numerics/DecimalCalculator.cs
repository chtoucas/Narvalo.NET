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
    // - Double.MaxValue > Decimal.MaxValue > Int64.MaxValue > Int32.MaxValue
    // - Check if this is correct for negative values.
    //
    // Remark: Integer division rounds toward zero.
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

        #region Rounding.

        // Directed rouding
        // ----------------
        //
        // - Down, aka "rounding towards minus infinity".       -infty <-
        // - Up, aka "rounding towards plus infinity".                      -> +infty
        // - TowardsZero, aka "rounding away from infinity".           -> 0 <-
        // - AwayFromZero, aka "rounding towards infinity".            <- 0 ->
        //
        // Mathematically speaking:
        // - Down,         floor(x) = n where n <= x < n + 1. En notation française, [x].
        // - Up,           ceiling(x) = n where n - 1 < x <= n.
        // - TowardsZero,  truncate(x) = sign(x) [|x|].
        // - AwayFromZero, n = - sign(x) [-|x|]
        // NB:
        // - for x > 0, Down = TowardsZero and Up = AwayFromZero.
        // - for x < 0, Down = AwayFromZero and Up = TowardsZero.
        //
        //       Down Up   TowardsZero AwayFromZero Nearest (see below for ambiguous points)
        //  1.6   1    2    1           2            2
        //  1.4                                      1
        //  0.6   0    1    0           1
        //  0.4                                      0
        // -0.4  -1    0    0           -1
        // -0.6                                     -1
        // -1.4  -2   -1   -1           -2
        // -1.6                                     -2
        //
        // Rounding to the nearest integer
        // -------------------------------
        //
        // When the fractional part is half-way of two integers, we need a tie-breaking rule.
        //
        // If x is of the form x = i + 0.5 where i is an integer.
        // - HalfDown,         n = ceiling(x - 0.5)
        // - HalfUp,           n = [x + 0.5]
        // - HalfTowardsZero,  n = - sign(x) [-|x| + 0.5]
        // - HalfAwayFromZero, n = sign(x) [|x| + 0.5]
        //
        // NB: floor(-x) = - ceiling(x), therefore
        //       HalfDown HalfUp HalfTowardsZero HalfAwayFromZero ToEven ToOdd
        //  1.5   1        2      1               2                2      1
        //  0.5   0        1      0               1                0      1
        // -0.5  -1        0      0              -1                0     -1
        // -1.5  -2       -1     -1              -2               -2     -1
        //
        // NB:
        // - for HalfUp and positive numbers, we just need to examine the first digit of the
        //   fractional part.
        // - HalfAwayFromZero, aka called commercial rounding,
        //   we just need to examine the first digit of the fractional part.
        // - ToEven, aka bankers' rounding. IEEE 754 default rounding mode.
        //
        // A rule is said to be
        // - balanced if the effect of rounding is statistically cancelled.
        // - symmetric if it treats symmetrically positive and negative numbers.

        // WARNING: It only works for representable values, ie those that has not been
        // silently rounded using the default rounding mode (MidpointRounding.ToEven).
        // Another problem is that we might compute value -/+ 0.5m.
        public static decimal Round(decimal value, NumberRounding rounding)
        {
            switch (rounding)
            {
                case NumberRounding.Down:
                    return Decimal.Floor(value);

                case NumberRounding.Up:
                    return Decimal.Ceiling(value);

                case NumberRounding.TowardsZero:
                    // Equivalent to: x > 0 ? floor(x) : ceiling(x)
                    return Decimal.Truncate(value);

                case NumberRounding.AwayFromZero:
                    return value > 0 ? Decimal.Ceiling(value) : Decimal.Floor(value);

                case NumberRounding.HalfDown:
                    // We treat Decimal.MinValue separately to avoid a stack overflow.
                    // NB: Decimal.MinValue is an integer.
                    if (value == Decimal.MinValue) { return value; }
                    return Decimal.Ceiling(value - 0.5m);

                case NumberRounding.HalfUp:
                    // We treat Decimal.MaxValue separately to avoid a stack overflow.
                    // NB: Decimal.MaxValue is an integer.
                    if (value == Decimal.MaxValue) { return value; }
                    return Decimal.Floor(value + 0.5m);

                case NumberRounding.HalfTowardsZero:
                    return value > 0 ? Decimal.Ceiling(value - .5m) : Decimal.Floor(value + .5m);

                case NumberRounding.HalfAwayFromZero:
                    // Equivalent to: x > 0 ? floor(x + .5) : ceiling(x - .5)
                    return Math.Round(value, 0, MidpointRounding.AwayFromZero);

                case NumberRounding.ToEven:
                    return Math.Round(value, 0, MidpointRounding.ToEven);

                case NumberRounding.ToOdd:
                    var n = Math.Round(value, 0, MidpointRounding.AwayFromZero);
                    return n % 2 == 0 ? (n > 0 ? --n : ++n) : n;

                default: throw Check.Unreachable("XXX");
            }
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //internal static decimal NearestTowardsZero(decimal x)
        //{
        //    decimal n = Decimal.Floor(x);
        //    decimal r = x - n - 0.5M;

        //    if (r < 0M) { return n; }
        //    if (r > 0M) { return n + 1; }

        //    return x > 0 ? n : Decimal.Truncate(x);
        //}

        #endregion

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
