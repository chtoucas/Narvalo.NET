// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Numerics;

    public static partial class MoneyExtensions
    {
        public static IEnumerable<decimal> Distribute(this Money @this, int decimalPlaces, int parts)
            => Distribute(@this, decimalPlaces, parts, Money.DefaultRounding);

        public static IEnumerable<decimal> Distribute(
            this Money @this,
            int decimalPlaces,
            int parts,
            MidpointRounding rounding)
            => DecimalCalculator.Distribute(@this.Amount, decimalPlaces, parts, rounding);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int percentage)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentage(percentage), Money.DefaultRounding);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int[] percentages)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentages(percentages), Money.DefaultRounding);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal ratio)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratio), Money.DefaultRounding);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal[] ratios)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratios), Money.DefaultRounding);

        public static IEnumerable<decimal> Allocate(
            this Money @this,
            int decimalPlaces,
            RatioArray ratios,
            MidpointRounding rounding)
            => DecimalCalculator.Allocate(@this.Amount, decimalPlaces, ratios, rounding);
    }
}
