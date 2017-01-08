// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

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

    public static class DecimalCalculator
    {
        private static readonly Func<decimal, decimal> s_Id = _ => _;

        //internal static int GetScale(this decimal @this)
        //{
        //    int flags = Decimal.GetBits(@this)[3];
        //    // Bits 16 to 23 contains an exponent between 0 and 28, which indicates the power
        //    // of 10 to divide the integer number.
        //    return (flags & 0x00FF0000) >> 16;
        //}

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Math.DivRem().")]
        public static decimal DivRem(decimal dividend, decimal divisor, out decimal remainder)
        {
            decimal q = dividend / divisor;
            // NB: remainder = dividend % divisor is slower? Sign?
            remainder = dividend - q * divisor;
            return q;
        }

        public static DivisionCollection<decimal> Divide(decimal dividend, int divisor)
        {
            Require.Range(divisor > 0, nameof(divisor));

            decimal rem;
            decimal q = DivRem(dividend, divisor, out rem);

            return DivisionCollection<decimal>.Create(q, rem, divisor);
        }

        #region Allocation.

        public static IEnumerable<decimal> Allocate(decimal amount, RatioArray ratios)
            => AllocateImpl(amount, ratios, s_Id);

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            RatioArray ratios,
            int decimalPlaces,
            RoundingMode mode)
            => AllocateImpl(amount, ratios, _ => DecimalRounding.Round(_, decimalPlaces, mode));

        public static IEnumerable<decimal> Allocate(
            decimal amount,
            RatioArray ratios,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            return AllocateImpl(amount, ratios, _ => rounding.Round(_, decimalPlaces));
        }

        private static IEnumerable<decimal> AllocateImpl(
            decimal amount,
            RatioArray ratios,
            Func<decimal, decimal> round)
        {
            var len = ratios.Length;
            var dist = new decimal[len];
            var last = amount;

            for (var i = 0; i < len - 1; i++)
            {
                decimal next = round.Invoke(ratios[i] * amount);
                last -= next;
                yield return next;
            }

            yield return last;
        }

        #endregion
    }
}
