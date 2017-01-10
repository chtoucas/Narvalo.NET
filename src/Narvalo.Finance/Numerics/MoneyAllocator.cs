// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    // Distribute.
    //
    // The operation is not reversible, that is (in general):
    // > money.Distribute(count).Sum() != money;
    //
    // The decimal type is a floating number (even if .NET does not call it so, it still is).
    //
    // With or without rounding, the last element of the resulting collection is calculated
    // differently.
    // If you prefer:
    // > seq = money.Distribute(count).Reverse();
    public static partial class MoneyAllocator
    {
        public static IEnumerable<Money> Distribute(Money money, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = money.Currency;
            decimal total = money.Amount;

            decimal q = total / count;
            Money part = new Money(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency);
        }

        public static IEnumerable<Money> Distribute(Money money, RatioArray ratios)
        {
            Currency currency = money.Currency;
            decimal total = money.Amount;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = ratios[i] * total;
                last -= amount;
                yield return new Money(amount, currency);
            }

            yield return new Money(last, currency);
        }
    }

    // Distribute with standard rounding.
    public static partial class MoneyAllocator
    {
        public static IEnumerable<Money> Distribute(Money money, int count, MidpointRounding mode)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = money.Currency;
            decimal total = money.Amount;

            decimal q = Math.Round(total / count, currency.DecimalPlaces, mode);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency, mode);
        }

        public static IEnumerable<Money> Distribute(Money money, RatioArray ratios, MidpointRounding mode)
        {
            Currency currency = money.Currency;
            decimal total = money.Amount;
            short decimalPlaces = currency.DecimalPlaces;

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

    // Distribute with custom rounding.
    public static partial class MoneyAllocator
    {
        public static IEnumerable<Money> Distribute(Money money, int count, IRoundingAdjuster adjuster)
        {
            Require.Range(count > 1, nameof(count));
            Require.NotNull(adjuster, nameof(adjuster));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = money.Currency;
            decimal total = money.Amount;

            decimal q = adjuster.Round(total / count, money.Currency.DecimalPlaces);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return MoneyCreator.Create(total - (count - 1) * q, currency, adjuster);
        }

        public static IEnumerable<Money> Distribute(Money money, RatioArray ratios, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));

            Currency currency = money.Currency;
            decimal total = money.Amount;
            short decimalPlaces = currency.DecimalPlaces;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = adjuster.Round(ratios[i] * total, decimalPlaces);
                last -= amount;
                yield return Money.OfMajor(amount, currency);
            }

            yield return MoneyCreator.Create(last, currency, adjuster);
        }
    }
}
