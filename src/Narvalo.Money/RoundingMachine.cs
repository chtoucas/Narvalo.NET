// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using Narvalo.Internal;
    using Narvalo.Finance.Rounding;

    // Core rounding methods + MidpointRounding.
    public static partial class RoundingMachine
    {
        public static decimal Ceiling(Money money) => RoundImpl(money, Math.Ceiling);

        public static decimal Floor(Money money) => RoundImpl(money, Math.Floor);

        public static decimal Truncate(Money money) => RoundImpl(money, Math.Truncate);

        public static decimal Round(Money money) => RoundImpl(money, Math.Round);

        public static decimal Round(Money money, MidpointRounding mode)
            => RoundImpl(money, _ => Math.Round(_, mode));

        public static decimal Round(Money money, int decimalPlaces)
            => Round(money, decimalPlaces, MidpointRounding.ToEven);

        public static decimal Round(Money money, int decimalPlaces, MidpointRounding mode)
        {
            // If the amount is already rounded to decimalPlaces, do nothing.
            if (money.IsRounded && money.Currency.DecimalPlaces == decimalPlaces) { return money.Amount; }
            return Math.Round(money.Amount, decimalPlaces, mode);
        }
    }

    // Core rounding methods + IRoundingAjduster.
    public static partial class RoundingMachine
    {
        public static decimal Round(Money money, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));
            return RoundImpl(money, _ => adjuster.Round(_));
        }

        public static decimal Round(Money money, int decimalPlaces, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));
            // If the amount is already rounded to decimalPlaces, do nothing.
            if (money.IsRounded && money.Currency.DecimalPlaces == decimalPlaces) { return money.Amount; }
            return adjuster.Round(money.Amount, decimalPlaces);
        }
    }

    // Standard binary math operators + MidpointRounding.
    public static partial class RoundingMachine
    {
        public static Money Add(Money money, decimal amount, MidpointRounding mode)
        {
            if (amount == 0m) { return money; }
            return Money.FromMajor(money.Amount + amount, money.Currency, mode);
        }

        public static Money Add(Money money, Money other, MidpointRounding mode)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount + other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, money.Currency)
                : Money.FromMajor(amount, money.Currency, mode);
        }

        public static Money Subtract(Money money, decimal amount, MidpointRounding mode)
            => Add(money, -amount, mode);

        public static Money Subtract(Money money, Money other, MidpointRounding mode)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount - other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, money.Currency)
                : Money.FromMajor(amount, money.Currency, mode);
        }

        public static Money Multiply(Money money, decimal multiplier, MidpointRounding mode)
            => Money.FromMajor(multiplier * money.Amount, money.Currency, mode);

        public static Money Divide(Money dividend, decimal divisor, MidpointRounding mode)
            => Money.FromMajor(dividend.Amount / divisor, dividend.Currency, mode);

        public static Money Remainder(Money dividend, decimal divisor, MidpointRounding mode)
            => Money.FromMajor(dividend.Amount % divisor, dividend.Currency, mode);
    }

    // Standard binary math operators + IRoundingAdjuster.
    public static partial class RoundingMachine
    {
        public static Money Add(Money money, decimal amount, IRoundingAdjuster adjuster)
        {
            if (amount == 0m) { return money; }
            return Money.FromMajor(money.Amount + amount, money.Currency, adjuster);
        }

        public static Money Add(Money money, Money other, IRoundingAdjuster adjuster)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount + other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, money.Currency)
                : Money.FromMajor(amount, money.Currency, adjuster);
        }

        public static Money Subtract(Money money, decimal amount, IRoundingAdjuster adjuster)
            => Add(money, -amount, adjuster);

        public static Money Subtract(Money money, Money other, IRoundingAdjuster adjuster)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount - other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.FromMajor(amount, money.Currency)
                : Money.FromMajor(amount, money.Currency, adjuster);
        }

        public static Money Multiply(Money money, decimal multiplier, IRoundingAdjuster adjuster)
            => Money.FromMajor(multiplier * money.Amount, money.Currency, adjuster);

        public static Money Divide(Money money, decimal divisor, IRoundingAdjuster adjuster)
            => Money.FromMajor(money.Amount / divisor, money.Currency, adjuster);

        public static Money Remainder(Money money, decimal divisor, IRoundingAdjuster adjuster)
            => Money.FromMajor(money.Amount % divisor, money.Currency, adjuster);
    }

    // LINQ-like Sum() + MidpointRounding.
    //
    // There are three ways to compute a sum with rounding in mind (NB: the sum of two rounded
    // values is always rounded):
    // 1. Compute the sum, then round.
    // 2. Round before each intermediate addition.
    // 3. Round after each intermediate addition.
    // I do not implement the third one, since I don't see any use cases for it (but I might be
    // wrong); the two others don't need any new operators:
    // 1. round after the summation:
    //    > source.Sum().Normalize(mode);
    // 2. round an amount before adding it to the cumulative sum:
    //    > source.Select(_ => _.Normalize(mode)).Sum();
    // For the later, we provides a custom implementation which tries to avoid to create any
    // unnecessary temporary objects due to the use of Normalize().
    // WARNING: To solve the second case, do not use
    // > source.Select(_ => Math.Round(_.Amount, _.Currency.DecimalPlaces, mode)).Sum();
    // it will not fail but won't give the correct result in case all elements do not use the
    // same currency, it should rather throw.
    public static partial class RoundingMachine
    {
        // Optimized version of: monies.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (var iter = monies.GetEnumerator())
            {
                if (!iter.MoveNext()) { goto EMPTY_COLLECTION; }

                Money mny = iter.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, mode);

                while (iter.MoveNext())
                {
                    mny = iter.Current;

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, mode);
                }

                return Money.FromMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.FromMajor(0, Currency.None);
        }

        // Optimized version of: monies.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money?> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (var iter = monies.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    Money? item = iter.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, mode);

                    while (iter.MoveNext())
                    {
                        item = iter.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, mode);
                        }
                    }

                    return Money.FromMajor(sum, currency);
                }
            }

            return Money.FromMajor(0, Currency.None);
        }
    }

    // LINQ-like Sum() + IRoundingAdjuster.
    public static partial class RoundingMachine
    {
        // Optimized version of: monies.Select(_ => _.Normalize(adjuster)).Sum().
        public static Money Sum(this IEnumerable<Money> monies, IRoundingAdjuster adjuster)
        {
            Require.NotNull(monies, nameof(monies));
            Require.NotNull(adjuster, nameof(adjuster));

            using (var iter = monies.GetEnumerator())
            {
                if (!iter.MoveNext()) { goto EMPTY_COLLECTION; }

                Money mny = iter.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, adjuster);

                while (iter.MoveNext())
                {
                    mny = iter.Current;

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, adjuster);
                }

                return Money.FromMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.FromMajor(0, Currency.None);
        }

        // Optimized version of: monies.Select(_ => _.Normalize(adjuster)).Sum().
        public static Money Sum(this IEnumerable<Money?> monies, IRoundingAdjuster adjuster)
        {
            Require.NotNull(monies, nameof(monies));
            Require.NotNull(adjuster, nameof(adjuster));

            using (var iter = monies.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    Money? item = iter.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, adjuster);

                    while (iter.MoveNext())
                    {
                        item = iter.Current;

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

    // LINQ-like Average() + MidpointRounding.
    //
    // As for the sum, there are many ways to compute an average with rounding in mind.
    // 1. Compute the average, then round:
    //   > source.Average().Normalize(mode);
    // 2. Round before each intermediate addition, take the average then round:
    //   > source.Select(_ => _.Normalize(mode)).Average().Normalize(mode);
    // These are the only ones we shall consider, and we will provide an optimized version for the
    // later. For information only, you might also consider to:
    // - compute the sum, round, take the average then round again.
    // - round before each intermediate addition, take the average then round the result.
    public static partial class RoundingMachine
    {
        // Optimized version of: monies.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (var iter = monies.GetEnumerator())
            {
                if (!iter.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money mny = iter.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, mode);
                long count = 1;

                while (iter.MoveNext())
                {
                    mny = iter.Current;

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, mode);
                    count++;
                }

                return Money.FromMajor(sum / count, currency, mode);
            }
        }

        // Optimized version of: monies.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (var iter = monies.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    Money? item = iter.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, mode);
                    long count = 1;

                    while (iter.MoveNext())
                    {
                        item = iter.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, mode);
                            count++;
                        }
                    }

                    return Money.FromMajor(sum / count, currency, mode);
                }
            }

            return null;
        }
    }

    // LINQ-like Average() + IRoundingAdjuster.
    public static partial class RoundingMachine
    {
        // Optimized version of: monies.Select(_ => _.Normalize(adjuster)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> monies, IRoundingAdjuster adjuster)
        {
            Require.NotNull(monies, nameof(monies));
            Require.NotNull(adjuster, nameof(adjuster));

            using (var iter = monies.GetEnumerator())
            {
                if (!iter.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money mny = iter.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, adjuster);
                long count = 1;

                while (iter.MoveNext())
                {
                    mny = iter.Current;

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, adjuster);
                    count++;
                }

                return Money.FromMajor(sum / count, currency, adjuster);
            }
        }

        // Optimized version of: monies.Select(_ => _.Normalize(adjuster)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> monies, IRoundingAdjuster adjuster)
        {
            Require.NotNull(monies, nameof(monies));
            Require.NotNull(adjuster, nameof(adjuster));

            using (var iter = monies.GetEnumerator())
            {
                while (iter.MoveNext())
                {
                    Money? item = iter.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, adjuster);
                    long count = 1;

                    while (iter.MoveNext())
                    {
                        item = iter.Current;

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
    public static partial class RoundingMachine
    {
        private static decimal RoundImpl(Money money, Func<decimal, decimal> func)
        {
            Debug.Assert(func != null);

            // If the amount is already rounded to 0, do nothing.
            if (money.IsRounded && money.Currency.DecimalPlaces == 0) { return money.Amount; }
            return func(money.Amount);
        }

        private static decimal NormalizeAmount(Money money, MidpointRounding mode)
            => money.IsNormalized
            ? money.Amount
            : Math.Round(money.Amount, money.Currency.DecimalPlaces, mode);

        private static decimal NormalizeAmount(Money money, IRoundingAdjuster adjuster)
            => money.IsNormalized
            ? money.Amount
            : adjuster.Round(money.Amount, money.Currency.DecimalPlaces);
    }
}
