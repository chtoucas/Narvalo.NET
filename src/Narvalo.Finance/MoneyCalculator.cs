// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Numerics;

    public static partial class MoneyCalculator
    {
        internal static decimal Round(decimal amount, Currency currency, MoneyRounding rounding)
        {
            switch (rounding)
            {
                case MoneyRounding.AwayFromZero:
                    return Math.Round(amount, currency.DecimalPlaces, MidpointRounding.AwayFromZero);
                case MoneyRounding.ToEven:
                    return Math.Round(amount, currency.DecimalPlaces, MidpointRounding.ToEven);
                case MoneyRounding.None:
                case MoneyRounding.Unnecessary:
                    return amount;
                default:
                    throw Check.Unreachable("XXX");
            }
        }

        public static IEnumerable<decimal> Distribute(this Money @this, int decimalPlaces, int parts)
            => Distribute(@this, decimalPlaces, parts, MoneyRounding.Default);

        public static IEnumerable<decimal> Distribute(
            this Money @this,
            int decimalPlaces,
            int parts,
            MoneyRounding rounding)
        {
            //return DecimalCalculator.Distribute(@this.Amount, decimalPlaces, parts, rounding);
            throw new NotImplementedException();
        }

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int percentage)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentage(percentage), MoneyRounding.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int[] percentages)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentages(percentages), MoneyRounding.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal ratio)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratio), MoneyRounding.Default);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal[] ratios)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratios), MoneyRounding.Default);

        public static IEnumerable<decimal> Allocate(
            this Money @this,
            int decimalPlaces,
            RatioArray ratios,
            MoneyRounding rounding)
        {
            //return DecimalCalculator.Allocate(@this.Amount, decimalPlaces, ratios, rounding);
            throw new NotImplementedException();
        }
    }
}
