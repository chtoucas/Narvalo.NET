// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    // WARNING: This class is not thread-safe.
    public sealed class DitheredRoundingAdjuster : IRoundingAdjuster
    {
        private static Random s_Random = new Random();

        private double Next => s_Random.NextDouble();

        public decimal Round(decimal value, int decimalPlaces)
        {
            throw new NotImplementedException();
        }
    }
}
