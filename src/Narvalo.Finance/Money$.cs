// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Numerics;

    public static partial class MoneyExtensions
    {
        public static IEnumerable<decimal> Distribute(this Money @this, int decimalPlaces, int parts)
            => Distribute(@this, decimalPlaces, parts, MidpointRounding.ToEven);

        public static IEnumerable<decimal> Distribute(
            this Money @this,
            int decimalPlaces,
            int parts,
            MidpointRounding rounding)
            => DecimalCalculator.Distribute(@this.Amount, decimalPlaces, parts, rounding);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int percentage)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentage(percentage), MidpointRounding.ToEven);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, int[] percentages)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentages(percentages), MidpointRounding.ToEven);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal ratio)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratio), MidpointRounding.ToEven);

        public static IEnumerable<decimal> Allocate(this Money @this, int decimalPlaces, decimal[] ratios)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratios), MidpointRounding.ToEven);

        public static IEnumerable<decimal> Allocate(
            this Money @this,
            int decimalPlaces,
            RatioArray ratios,
            MidpointRounding rounding)
            => DecimalCalculator.Allocate(@this.Amount, decimalPlaces, ratios, rounding);
    }
}
