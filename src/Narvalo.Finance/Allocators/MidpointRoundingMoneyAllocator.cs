// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System;
    using System.Collections.Generic;

    // Allocator with standard midpoint rounding.
    public sealed class MidpointRoundingMoneyAllocator : IMoneyAllocator
    {
        public MidpointRoundingMoneyAllocator(MidpointRounding mode)
        {
            RoundingMode = mode;
        }

        public MidpointRounding RoundingMode { get; }

        public IEnumerable<Money> Allocate(Money money, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            var currency = money.Currency;
            decimal total = money.Amount;

            decimal q = Math.Round(total / count, currency.DecimalPlaces, RoundingMode);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency, RoundingMode);
        }

        public IEnumerable<Money> Allocate(Money money, RatioArray ratios)
        {
            Warrant.NotNull<IEnumerable<Money>>();

            var currency = money.Currency;
            decimal total = money.Amount;
            int decimalPlaces = currency.DecimalPlaces;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = Math.Round(ratios[i] * total, decimalPlaces, RoundingMode);
                last -= amount;
                yield return Money.OfMajor(amount, currency);
            }

            yield return new Money(last, currency, RoundingMode);
        }
    }
}
