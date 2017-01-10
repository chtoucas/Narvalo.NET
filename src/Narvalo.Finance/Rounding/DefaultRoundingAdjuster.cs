// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    public sealed class DefaultRoundingAdjuster : IRoundingAdjuster
    {
        public DefaultRoundingAdjuster() : this(RoundingMode.ToEven) { }

        public DefaultRoundingAdjuster(RoundingMode mode)
        {
            Mode = mode;
        }

        public RoundingMode Mode { get; }

        public decimal Round(decimal value, int decimalPlaces)
            => RoundingAdjusters.Round(value, decimalPlaces, Mode);
    }
}
