// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Rounding
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Allocators;
    using Narvalo.Finance.Internal;

    // Allocation.
    public static partial class MoneyExtensions
    {
        public static IEnumerable<Money> Allocate(this Money @this, int count, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return new RoundingMoneyAllocator(adjuster).Allocate(@this, count);
        }

        public static IEnumerable<Money> Allocate(this Money @this, RatioArray ratios, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return new RoundingMoneyAllocator(adjuster).Allocate(@this, ratios);
        }
    }

    // Standard binary math operators.
    public static partial class MoneyExtensions
    {
        public static Money Plus(this Money @this, decimal amount, MidpointRounding mode)
            => MoneyCalculator.Add(@this, amount, mode);

        public static Money Plus(this Money @this, decimal amount, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            if (amount == 0m) { return @this; }
            return Money.FromMajor(@this.Amount + amount, @this.Currency, adjuster);
        }

        public static Money Plus(this Money @this, Money other, MidpointRounding mode)
            => MoneyCalculator.Add(@this, other, mode);

        public static Money Plus(this Money @this, Money other, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, @this.Currency)
                : Money.FromMajor(amount, @this.Currency, adjuster);
        }

        public static Money Minus(this Money @this, decimal amount, MidpointRounding mode)
            => MoneyCalculator.Subtract(@this, amount, mode);

        public static Money Minus(this Money @this, decimal amount, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return Plus(@this, -amount, adjuster);
        }

        public static Money Minus(this Money @this, Money other, MidpointRounding mode)
            => MoneyCalculator.Subtract(@this, other, mode);

        public static Money Minus(this Money @this, Money other, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, @this.Currency)
                : Money.FromMajor(amount, @this.Currency, adjuster);
        }

        public static Money MultiplyBy(this Money @this, decimal multiplier, MidpointRounding mode)
            => MoneyCalculator.Multiply(@this, multiplier, mode);

        public static Money MultiplyBy(this Money @this, decimal multiplier, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return Money.FromMajor(multiplier * @this.Amount, @this.Currency, adjuster);
        }

        public static Money DivideBy(this Money @this, decimal divisor, MidpointRounding mode)
            => MoneyCalculator.Divide(@this, divisor, mode);

        public static Money DivideBy(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return Money.FromMajor(@this.Amount / divisor, @this.Currency, adjuster);
        }

        public static Money Mod(this Money @this, decimal divisor, MidpointRounding mode)
            => MoneyCalculator.Remainder(@this, divisor, mode);

        public static Money Mod(this Money @this, decimal divisor, IRoundingAdjuster adjuster)
        {
            Expect.NotNull(adjuster);
            return Money.FromMajor(@this.Amount % divisor, @this.Currency, adjuster);
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

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, adjuster);
                }

                return Money.FromMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.FromMajor(0, Currency.None);
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

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, adjuster);
                        }
                    }

                    return Money.FromMajor(sum, currency);
                }
            }

            return Money.FromMajor(0, Currency.None);
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

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, adjuster);
                    count++;
                }

                return Money.FromMajor(sum / count, currency, adjuster);
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

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, adjuster);
                            count++;
                        }
                    }

                    return Money.FromMajor(sum / count, currency, adjuster);
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
