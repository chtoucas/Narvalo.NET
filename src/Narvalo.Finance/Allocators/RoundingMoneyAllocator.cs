// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System.Collections.Generic;

    using Narvalo.Finance.Rounding;

    // Allocator with custom rounding.
    public sealed class RoundingMoneyAllocator : IMoneyAllocator
    {
        public RoundingMoneyAllocator(IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));

            RoundingAdjuster = adjuster;
        }

        public IRoundingAdjuster RoundingAdjuster { get; }

        public IEnumerable<Money> Allocate(Money money, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            var currency = money.Currency;
            decimal total = money.Amount;

            decimal q = RoundingAdjuster.Round(total / count, money.Currency.DecimalPlaces);
            var part = Money.FromMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return Money.FromMajor(total - (count - 1) * q, currency, RoundingAdjuster);
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
                decimal amount = RoundingAdjuster.Round(ratios[i] * total, decimalPlaces);
                last -= amount;
                yield return Money.FromMajor(amount, currency);
            }

            yield return Money.FromMajor(last, currency, RoundingAdjuster);
        }
    }
}
