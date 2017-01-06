// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    // WARNING: This class is not thread-safe and not truly stochastic.
    public sealed class HalfDitheringDecimalRounding : IDecimalRounding
    {
        private static Random s_Random = new Random();

        private double Next => s_Random.NextDouble();

        public decimal Round(decimal value)
        {
            throw new NotImplementedException();
        }

        public decimal Round(decimal value, int decimals)
        {
            throw new NotImplementedException();
        }
    }
}
