// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    // WARNING: This class is not thread-safe and not truly stochastic.
    public sealed class StochasticDecimalRounding : IDecimalRounding
    {
        private static Random s_Random = new Random();

        private static bool UpOrDown => s_Random.Next(0, 2) == 0;

        public decimal Round(decimal value)
            => UpOrDown
            ? DecimalRounding.Round(value, NumberRounding.HalfUp)
            : DecimalRounding.Round(value, NumberRounding.HalfDown);

        public decimal Round(decimal value, int decimals)
        {
            Require.Range(0 <= decimals && decimals <= DecimalRounding.MaxScale, nameof(decimals));

            return UpOrDown
                ? DecimalRounding.Round(value, decimals, NumberRounding.HalfUp)
                : DecimalRounding.Round(value, decimals, NumberRounding.HalfDown);
        }
    }
}
