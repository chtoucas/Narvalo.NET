// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    using Calculator = Narvalo.Finance.MoneyCalculator;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money @this, decimal amount, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            if (amount == 0m) { return @this; }
            return MoneyCreator.Create(@this.Amount + amount, @this.Currency, adjuster);
        }

        public static Money Add(this Money @this, Money other, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
                : MoneyCreator.Create(amount, @this.Currency, adjuster);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money @this, decimal amount, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return Add(@this, -amount, adjuster);
        }

        public static Money Subtract(this Money @this, Money other, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
                : MoneyCreator.Create(amount, @this.Currency, adjuster);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money @this, decimal multiplier, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(multiplier * @this.Amount, @this.Currency, adjuster);
        }
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(@this.Amount / divisor, @this.Currency, adjuster);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(@this.Amount % divisor, @this.Currency, adjuster);
        }
    }

    // LINQ-like Sum().
    public static partial class MoneyCalculator
    {
        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Sum().
        public static Money Sum(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, adjuster);

                while (it.MoveNext())
                {
                    item = it.Current;

                    Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, adjuster);
                }

                return Money.OfMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Sum().
        public static Money Sum(this IEnumerable<Money?> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, adjuster);

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, adjuster);
                        }
                    }

                    return Money.OfMajor(sum, currency);
                }
            }

            return Money.OfMajor(0, Currency.None);
        }
    }

    // LINQ-like Average().
    public static partial class MoneyCalculator
    {
        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, adjuster);
                long count = 1;

                while (it.MoveNext())
                {
                    item = it.Current;

                    Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, adjuster);
                    count++;
                }

                return MoneyCreator.Create(sum / count, currency, adjuster);
            }
        }

        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, adjuster);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, adjuster);
                            count++;
                        }
                    }

                    return MoneyCreator.Create(sum / count, currency, adjuster);
                }
            }

            return null;
        }
    }

    // Distribute.
    public static partial class MoneyCalculator
    {
        public static IEnumerable<Money> Distribute(this Money @this, int count, IRoundingAdjuster adjuster)
        {
            Require.Range(count > 1, nameof(count));
            Require.NotNull(adjuster, nameof(adjuster));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = @this.Currency;
            decimal total = @this.Amount;

            decimal q = adjuster.Round(total / count, @this.Currency.DecimalPlaces);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return MoneyCreator.Create(total - (count - 1) * q, currency, adjuster);
        }

        public static IEnumerable<Money> Distribute(this Money @this, RatioArray ratios, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));

            Currency currency = @this.Currency;
            decimal total = @this.Amount;
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

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static decimal NormalizeAmount(Money money, IRoundingAdjuster adjuster)
            => money.IsNormalized
            ? money.Amount
            : adjuster.Round(money.Amount, money.Currency.DecimalPlaces);
    }
}
