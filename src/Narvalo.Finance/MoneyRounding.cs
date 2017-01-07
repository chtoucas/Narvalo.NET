// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
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
            if (mode == RoundingMode.ToEven)
            {
                return DecimalRounding.RoundToEven(amount, currency.DecimalPlaces);
            }
            else if (mode == RoundingMode.AwayFromZero)
            {
                return DecimalRounding.RoundHalfAwayFromZero(amount, currency.DecimalPlaces);
            }
            else
            {
                return amount;
            }
        }
    }
}
