// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Rounding;

    // Allocator with standard rounding.
    public static partial class MoneyAllocators
    {
        public static IEnumerable<Money> Allocate(this Money @this, int count, MidpointRounding mode)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = @this.Currency;
            decimal total = @this.Amount;

            decimal q = Math.Round(total / count, currency.DecimalPlaces, mode);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency, mode);
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, MidpointRounding mode)
        {
            Currency currency = @this.Currency;
            decimal total = @this.Amount;
            int decimalPlaces = currency.DecimalPlaces;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = Math.Round(ratios[i] * total, decimalPlaces, mode);
                last -= amount;
                yield return Money.OfMajor(amount, currency);
            }

            yield return new Money(last, currency, mode);
        }
    }

    public static partial class MoneyAllocators
    {
        public static IEnumerable<Money> Allocate(this Money @this, int count, IRoundingAdjuster adjuster)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = @this.Currency;
            decimal total = @this.Amount;

            decimal q = adjuster.Round(total / count, @this.Currency.DecimalPlaces);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return AdjustedMoneyFactory.Create(total - (count - 1) * q, currency, adjuster);
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, IRoundingAdjuster adjuster)
        {
            Currency currency = @this.Currency;
            decimal total = @this.Amount;
            int decimalPlaces = currency.DecimalPlaces;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = adjuster.Round(ratios[i] * total, decimalPlaces);
                last -= amount;
                yield return Money.OfMajor(amount, currency);
            }

            yield return AdjustedMoneyFactory.Create(last, currency, adjuster);
        }
    }
}
