// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;
    using System.Collections.Generic;

    // Normalization.
    public static partial class MoneyExtensions
    {
        public static Money Normalize(this Money @this, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            if (@this.IsNormalized) { return @this; }
            return MoneyCreator.Create(@this.Amount, @this.Currency, adjuster);
        }
    }

    // Addition with rounding.
    public static partial class MoneyExtensions
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
    public static partial class MoneyExtensions
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
    public static partial class MoneyExtensions
    {
        public static Money Multiply(this Money @this, decimal multiplier, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(multiplier * @this.Amount, @this.Currency, adjuster);
        }
    }

    // Division with rounding.
    public static partial class MoneyExtensions
    {
        public static Money Divide(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(@this.Amount / divisor, @this.Currency, adjuster);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyExtensions
    {
        public static Money Remainder(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return MoneyCreator.Create(@this.Amount % divisor, @this.Currency, adjuster);
        }
    }

    // LINQ-like Sum().
    public static partial class MoneyExtensions
    {
        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Sum().
        public static Money Sum(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money mny = it.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, adjuster);

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyCalculator.ThrowIfCurrencyMismatch(mny.Currency, currency);

                    sum += NormalizeAmount(mny, adjuster);
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
                    Money? item = it.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, adjuster);

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyCalculator.ThrowIfCurrencyMismatch(mny.Currency, currency);

                            sum += NormalizeAmount(mny, adjuster);
                        }
                    }

                    return Money.OfMajor(sum, currency);
                }
            }

            return Money.OfMajor(0, Currency.None);
        }
    }

    // LINQ-like Average().
    public static partial class MoneyExtensions
    {
        // Optimized version of: @this.Select(_ => _.Normalize(adjuster)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> @this, IRoundingAdjuster adjuster)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(adjuster, nameof(adjuster));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money mny = it.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, adjuster);
                long count = 1;

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyCalculator.ThrowIfCurrencyMismatch(mny.Currency, currency);

                    sum += NormalizeAmount(mny, adjuster);
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
                    Money? item = it.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, adjuster);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyCalculator.ThrowIfCurrencyMismatch(mny.Currency, currency);

                            sum += NormalizeAmount(mny, adjuster);
                            count++;
                        }
                    }

                    return MoneyCreator.Create(sum / count, currency, adjuster);
                }
            }

            return null;
        }
    }

    // Helpers.
    public static partial class MoneyExtensions
    {
        private static decimal NormalizeAmount(Money money, IRoundingAdjuster adjuster)
            => money.IsNormalized
            ? money.Amount
            : adjuster.Round(money.Amount, money.Currency.DecimalPlaces);
    }
}
