// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;

    internal static class MidpointRoundingExtensions
    {
        public static decimal Round(this MidpointRounding @this, decimal amount, int decimalPlaces)
            => Math.Round(amount, decimalPlaces, @this);
    }
}
