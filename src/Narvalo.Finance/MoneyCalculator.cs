// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Numerics;

    public static partial class MoneyCalculator
    {
        internal static decimal Round(decimal amount, Currency currency, RoundingMode mode)
        {
            switch (mode)
            {
                case RoundingMode.AwayFromZero:
                    return Math.Round(amount, currency.DecimalPlaces, MidpointRounding.AwayFromZero);
                case RoundingMode.ToEven:
                    return Math.Round(amount, currency.DecimalPlaces, MidpointRounding.ToEven);
                case RoundingMode.None:
                case RoundingMode.Unnecessary:
                    return amount;
                default:
                    throw Check.Unreachable("XXX");
            }
        }

        public static IEnumerable<decimal> Distribute(this Money @this, int decimalPlaces, int parts)
            => Distribute(@this, decimalPlaces, parts, RoundingMode.Default);

        public static IEnumerable<decimal> Distribute(
            this Money @this,
            int decimalPlaces,
            int parts,
            RoundingMode mode)
        {
            //return DecimalCalculator.Distribute(@this.Amount, decimalPlaces, parts, rounding);
            throw new NotImplementedException();
        }

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int percentage)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentage(percentage), RoundingMode.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int[] percentages)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentages(percentages), RoundingMode.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal ratio)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratio), RoundingMode.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal[] ratios)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratios), RoundingMode.Default);

        public static IEnumerable<decimal> Allocate(
            this Money @this,
            int decimalPlaces,
            RatioArray ratios,
            RoundingMode mode)
        {
            //return DecimalCalculator.Allocate(@this.Amount, decimalPlaces, ratios, rounding);
            throw new NotImplementedException();
        }
    }
}
