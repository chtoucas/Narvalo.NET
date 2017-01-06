// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Numerics;

    public sealed class MoneyRounding : IMoneyRounding
    {
        private readonly IDecimalRounding _inner;

        public MoneyRounding(IDecimalRounding inner)
        {
            Require.NotNull(inner, nameof(inner));
            _inner = inner;
        }

        public decimal Round(decimal amount, Currency currency)
            => _inner.Round(amount, currency.DecimalPlaces);

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
