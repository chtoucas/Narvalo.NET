// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Utilities;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money money, decimal amount, MidpointRounding mode)
        {
            if (amount == 0m) { return money; }
            return new Money(money.Amount + amount, money.Currency, mode);
        }

        public static Money Add(this Money money, Money other, MidpointRounding mode)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount + other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, money.Currency)
                : new Money(amount, money.Currency, mode);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money money, decimal amount, MidpointRounding mode)
            => Add(money, -amount, mode);

        public static Money Subtract(this Money money, Money other, MidpointRounding mode)
        {
            money.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = money.Amount - other.Amount;

            return money.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, money.Currency)
                : new Money(amount, money.Currency, mode);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money money, decimal multiplier, MidpointRounding mode)
            => new Money(multiplier * money.Amount, money.Currency, mode);
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money dividend, decimal divisor, MidpointRounding mode)
            => new Money(dividend.Amount / divisor, dividend.Currency, mode);
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money dividend, decimal divisor, MidpointRounding mode)
            => new Money(dividend.Amount % divisor, dividend.Currency, mode);
    }

    // LINQ-like Sum().
    //
    // For collections of nullable moneys, the signature of Sum() differs from the one found
    // in .NET; instead of returning a nullable money, we return a money. Anyway, despite what it
    // advertises, Sum() from LINQ never returns null.
    //
    // We do not include all variants of Sum() since they are easy to implement: if source is
    // of type IEnumerable<TSource>:
    // - source.Sum(Func<TSource, Money> selector)
    //   == source.Select(selector).Sum()
    // - source.Sum(Func<TSource, Money> selector, MidpointRounding mode)
    //   == source.Select(selector).Sum(mode)
    // - source.Sum(Func<TSource, Money?> selector)
    //   ==  source.Select(selector).Sum()
    // - source.Sum(Func<TSource, Money?> selector, MidpointRounding mode)
    //   == source.Select(selector).Sum(mode)
    //
    // Below, we do not use the various overloads for the addition. It would be a waste of
    // resources to create many short-lived Money objects, precisely one for each addition, while
    // we can sum the amounts then construct a single Money object.
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
    public static partial class MoneyCalculator
    {
        public static Money Sum(this IEnumerable<Money> monies)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money> it = monies.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                // The main purpose for the separate treatment of the first element is to get a
                // hand on its underlying currency which will serve as a reference when we shall
                // check that all elements of the collection use the same currency.
                Money mny = it.Current;
                Currency currency = mny.Currency;
                bool normalized = mny.IsNormalized;
                decimal sum = mny.Amount;

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                    normalized = normalized && mny.IsNormalized;
                    sum += mny.Amount;
                }

                return new Money(sum, currency, normalized);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
        }

        public static Money Sum(this IEnumerable<Money?> monies)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money?> it = monies.GetEnumerator())
            {
                // If the sequence is empty, we never enter this loop.
                // Actually, this is not really a loop, as it executes only once.
                while (it.MoveNext())
                {
                    Money? item = it.Current;
                    // If all elements are null, we never pass this point.
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    bool normalized = mny.IsNormalized;
                    decimal sum = mny.Amount;

                    // Loop over the remaining elements, if any.
                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                            normalized = normalized && mny.IsNormalized;
                            sum += mny.Amount;
                        }
                    }

                    return new Money(sum, currency, normalized);
                }
            }

            // For an empty collection or a collection of nulls, we return zero.
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money> it = monies.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money mny = it.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, mode);

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, mode);
                }

                return Money.OfMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money?> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money?> it = monies.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? item = it.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, mode);

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, mode);
                        }
                    }

                    return Money.OfMajor(sum, currency);
                }
            }

            return Money.OfMajor(0, Currency.None);
        }
    }

    // LINQ-like Average().
    //
    // As for Sum(), we do not implement all variants of Average(): if source is of type
    // IEnumerable<TSource>:
    // - source.Average(Func<TSource, Money> selector) == source.Select(selector).Average()
    // - source.Average(Func<TSource, Money> selector, MidpointRounding mode) == source.Select(selector).Average(mode)
    // - source.Average(Func<TSource, Money?> selector) ==  source.Select(selector).Average()
    // - source.Average(Func<TSource, Money?> selector, MidpointRounding mode) == source.Select(selector).Average(mode)
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
    public static partial class MoneyCalculator
    {
        // NB: The result is ALWAYS denormalized.
        public static Money Average(this IEnumerable<Money> monies)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money> it = monies.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money mny = it.Current;
                Currency currency = mny.Currency;
                decimal sum = mny.Amount;
                long count = 1;

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                    sum += mny.Amount;
                    count++;
                }

                return new Money(sum / count, currency);
            }
        }

        // NB: The result is ALWAYS denormalized.
        public static Money? Average(this IEnumerable<Money?> monies)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money?> it = monies.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? item = it.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = mny.Amount;
                    long count = 1;

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                            sum += mny.Amount;
                            count++;
                        }
                    }

                    return new Money(sum / count, currency);
                }
            }

            return null;
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money> it = monies.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money mny = it.Current;
                Currency currency = mny.Currency;
                decimal sum = NormalizeAmount(mny, mode);
                long count = 1;

                while (it.MoveNext())
                {
                    mny = it.Current;

                    MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                    sum += NormalizeAmount(mny, mode);
                    count++;
                }

                return new Money(sum / count, currency, mode);
            }
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> monies, MidpointRounding mode)
        {
            Require.NotNull(monies, nameof(monies));

            using (IEnumerator<Money?> it = monies.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? item = it.Current;
                    if (!item.HasValue) { continue; }

                    Money mny = item.Value;
                    Currency currency = mny.Currency;
                    decimal sum = NormalizeAmount(mny, mode);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            mny = item.Value;

                            MoneyChecker.ThrowIfCurrencyMismatch(mny, currency);

                            sum += NormalizeAmount(mny, mode);
                            count++;
                        }
                    }

                    return new Money(sum / count, currency, mode);
                }
            }

            return null;
        }
    }

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static decimal NormalizeAmount(Money money, MidpointRounding mode)
            => money.IsNormalized
            ? money.Amount
            : Math.Round(money.Amount, money.Currency.DecimalPlaces, mode);
    }
}
