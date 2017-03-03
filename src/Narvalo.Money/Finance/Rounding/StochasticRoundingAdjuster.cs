// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;

    // WARNING: This class is not thread-safe and not truly stochastic.
    public sealed class StochasticRoundingAdjuster : IRoundingAdjuster
    {
        private static Random s_Random = new Random();

        private static bool UpOrDown => s_Random.Next(0, 2) == 0;

        public decimal Round(decimal value)
            => UpOrDown
            ? RoundingAdjusters.HalfUp(value)
            : RoundingAdjusters.HalfDown(value);

        public decimal Round(decimal value, int decimalPlaces)
            => UpOrDown
            ? RoundingAdjusters.HalfUp(value, decimalPlaces)
            : RoundingAdjusters.HalfDown(value, decimalPlaces);
    }
}
