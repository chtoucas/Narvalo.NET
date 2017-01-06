// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    public sealed class MoneyRounding : IMoneyRounding
    {
        public MoneyRounding(RoundingMode mode)
        {
            Require.Range(mode != RoundingMode.None, nameof(mode));

            Mode = mode;
        }

        public RoundingMode Mode { get; }

        public decimal Round(decimal amount, Currency currency)
            => Round(amount, currency, Mode);

        public static decimal Round(decimal amount, Currency currency, RoundingMode mode)
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
    }
}
