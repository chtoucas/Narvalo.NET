// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Finance.Numerics;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money @this, decimal amount, MidpointRounding mode)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, mode);
        }

        public static Money Add(this Money @this, Money other, MidpointRounding mode)
        {
            Money.ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfCurrency(amount, @this.Currency)
                : new Money(amount, @this.Currency, mode);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money @this, decimal amount, MidpointRounding mode)
            => Add(@this, -amount, mode);

        public static Money Subtract(this Money @this, Money other, MidpointRounding mode)
        {
            Money.ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfCurrency(amount, @this.Currency)
                : new Money(amount, @this.Currency, mode);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money @this, decimal multiplier, MidpointRounding mode)
            => new Money(multiplier * @this.Amount, @this.Currency, mode);
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money @this, decimal divisor, MidpointRounding mode)
            => new Money(@this.Amount / divisor, @this.Currency, mode);
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, MidpointRounding mode)
            => new Money(@this.Amount % divisor, @this.Currency, mode);
    }

    // Operators normally found in the class Math.
    public static partial class MoneyCalculator
    {
        public static Money Abs(Money money) => money.IsPositiveOrZero ? money : money.Negate();

        public static Money Sign(Money money) => money < 0 ? -1 : (money > 0 ? 1 : 0);

        public static Money Max(Money money1, Money money2) => money1 >= money2 ? money1 : money2;

        public static Money Min(Money money1, Money money2) => money1 <= money2 ? money1 : money2;

        public static Money Clamp(Money money, Money min, Money max)
        {
            Require.True(min <= max, nameof(min));

            return money < min ? min : (money > max ? max : money);
        }

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Math.DivRem().")]
        public static Money DivRem(Money dividend, decimal divisor, out Money remainder)
        {
            // REVIEW: remainder = dividend % divisor is slower for integers. What about decimals?
            // > var q = dividend.Divide(divisor);
            // > remainder = dividend.Remainder(divisor);
            // For doubles, .NET uses:
            // Modulus = (Math.Abs(dividend) - (Math.Abs(divisor)
            //   * (Math.Floor(Math.Abs(dividend) / Math.Abs(divisor)))))
            //   * Math.Sign(dividend)
            decimal q = dividend.Amount / divisor;
            decimal rem = dividend.Amount - q * divisor;
            remainder = new Money(rem, dividend.Currency);
            return new Money(q, dividend.Currency);
        }

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Ceiling(Money money) => money.Round(Math.Ceiling);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Floor(Money money) => money.Round(Math.Floor);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Truncate(Money money) => money.Round(Math.Truncate);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money) => money.Round(Math.Round);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, MidpointRounding mode)
            => money.Round(_ => Math.Round(_, mode));

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, int decimalPlaces)
            => money.Round(decimalPlaces, MidpointRounding.ToEven);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, int decimalPlaces, MidpointRounding mode)
            => money.Round(decimalPlaces, mode);
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
    public static partial class MoneyCalculator
    {
        public static Money Sum(this IEnumerable<Money> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                // The main purpose for the separate treatment of the first element
                // is to get a hand on the (hopefully) common currency.
                Money f = it.Current;
                Currency currency = f.Currency;
                bool normalized = f.IsNormalized;
                decimal sum = f.Amount;

                while (it.MoveNext())
                {
                    Money c = it.Current;

                    ThrowIfCurrencyMismatch(c.Currency, currency);

                    normalized = normalized && c.IsNormalized;
                    sum += c.Amount;
                }

                return new Money(sum, currency, normalized);
            }

            EMPTY_COLLECTION:
            return Money.OfCurrency(0, Currency.None);
        }

        public static Money Sum(this IEnumerable<Money?> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                // If the sequence is empty, we never enter this loop.
                // Actually, this is not really a loop, as it executes only once.
                while (it.MoveNext())
                {
                    Money? nf = it.Current;
                    // If all elements are null, we never pass this point.
                    if (!nf.HasValue) { continue; }

                    Money f = nf.Value;
                    Currency currency = f.Currency;
                    bool normalized = f.IsNormalized;
                    decimal sum = f.Amount;

                    // Loop over the remaining elements, if any.
                    while (it.MoveNext())
                    {
                        Money? nc = it.Current;

                        if (nc.HasValue)
                        {
                            Money c = nc.Value;

                            ThrowIfCurrencyMismatch(c.Currency, currency);

                            normalized = normalized && c.IsNormalized;
                            sum += c.Amount;
                        }
                    }

                    return new Money(sum, currency, normalized);
                }
            }

            // For an empty collection or a collection of nulls, we return zero.
            return Money.OfCurrency(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum();
        public static Money Sum(this IEnumerable<Money> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money f = it.Current;
                Currency currency = f.Currency;
                short decimalPlaces = currency.DecimalPlaces;
                decimal sum = f.IsNormalized ? f.Amount : mode.Round(f.Amount, decimalPlaces);

                while (it.MoveNext())
                {
                    Money c = it.Current;

                    ThrowIfCurrencyMismatch(c.Currency, currency);

                    sum += c.IsNormalized ? c.Amount : mode.Round(c.Amount, decimalPlaces);
                }

                return Money.OfCurrency(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.OfCurrency(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum();
        public static Money Sum(this IEnumerable<Money?> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? nf = it.Current;
                    if (!nf.HasValue) { continue; }

                    Money f = nf.Value;
                    Currency currency = f.Currency;
                    short decimalPlaces = currency.DecimalPlaces;
                    decimal sum = f.IsNormalized ? f.Amount : mode.Round(f.Amount, decimalPlaces);

                    while (it.MoveNext())
                    {
                        Money? nc = it.Current;

                        if (nc.HasValue)
                        {
                            Money c = nc.Value;

                            ThrowIfCurrencyMismatch(c.Currency, currency);

                            sum += c.IsNormalized ? c.Amount : mode.Round(c.Amount, decimalPlaces);
                        }
                    }

                    return Money.OfCurrency(sum, currency);
                }
            }

            return Money.OfCurrency(0, Currency.None);
        }

        private static void ThrowIfCurrencyMismatch(Currency cy1, Currency cy2)
        {
            if (cy1 != cy2)
            {
                throw new InvalidOperationException("XXX");
            }
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
        public static Money Average(this IEnumerable<Money> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money f = it.Current;
                Currency currency = f.Currency;
                decimal sum = f.Amount;
                long count = 1;

                while (it.MoveNext())
                {
                    Money c = it.Current;

                    ThrowIfCurrencyMismatch(c.Currency, currency);

                    sum += c.Amount;
                    count++;
                }

                return new Money(sum / count, currency);
            }
        }

        // NB: The result is ALWAYS denormalized.
        public static Money? Average(this IEnumerable<Money?> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? nf = it.Current;
                    if (!nf.HasValue) { continue; }

                    Money f = nf.Value;
                    Currency currency = f.Currency;
                    decimal sum = f.Amount;
                    long count = 1;

                    while (it.MoveNext())
                    {
                        Money? nc = it.Current;

                        if (nc.HasValue)
                        {
                            Money c = nc.Value;

                            ThrowIfCurrencyMismatch(c.Currency, currency);

                            sum += c.Amount;
                            count++;
                        }
                    }

                    return new Money(sum / count, currency);
                }
            }

            return null;
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode);
        public static Money Average(this IEnumerable<Money> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money f = it.Current;
                Currency currency = f.Currency;
                short decimalPlaces = currency.DecimalPlaces;
                decimal sum = f.IsNormalized ? f.Amount : mode.Round(f.Amount, decimalPlaces);
                long count = 1;

                while (it.MoveNext())
                {
                    Money c = it.Current;

                    ThrowIfCurrencyMismatch(c.Currency, currency);

                    sum += c.IsNormalized ? c.Amount : mode.Round(c.Amount, decimalPlaces);
                    count++;
                }

                return new Money(sum / count, currency, mode);
            }
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode);
        public static Money? Average(this IEnumerable<Money?> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? nf = it.Current;
                    if (!nf.HasValue) { continue; }

                    Money f = nf.Value;
                    Currency currency = f.Currency;
                    short decimalPlaces = currency.DecimalPlaces;
                    decimal sum = f.IsNormalized ? f.Amount : mode.Round(f.Amount, decimalPlaces);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        Money? nc = it.Current;

                        if (nc.HasValue)
                        {
                            Money c = nc.Value;

                            ThrowIfCurrencyMismatch(c.Currency, currency);

                            sum += c.IsNormalized ? c.Amount : mode.Round(c.Amount, decimalPlaces);
                            count++;
                        }
                    }

                    return new Money(sum / count, currency, mode);
                }
            }

            return null;
        }
    }

    // Distribute.
    public static partial class MoneyCalculator
    {
        #region Distribute

        public static IEnumerable<Money> Distribute(Money money, int parts)
        {
            Require.Range(parts > 0, nameof(parts));
            Warrant.NotNull<IEnumerable<Money>>();

            var q = money.Amount / parts;
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select new Money(_, money.Currency);
        }

        public static IEnumerable<Money> Distribute(
            Money money,
            int parts,
            int decimalPlaces,
            MidpointRounding mode)
        {
            Require.Range(parts > 0, nameof(parts));
            Warrant.NotNull<IEnumerable<Money>>();

            var q = mode.Round(money.Amount / parts, decimalPlaces);
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select Money.OfCurrency(_, money.Currency);
        }

        internal static IEnumerable<decimal> GetDistribution(decimal total, int count, decimal part)
        {
            Warrant.NotNull<IEnumerable<decimal>>();

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return total - (count - 1) * part;
        }

        #endregion

        public static IEnumerable<Money> Allocate(Money money, RatioArray ratios)
        {
            return from _ in DecimalCalculator.Allocate(money.Amount, ratios)
                   select new Money(_, money.Currency);
        }

        public static IEnumerable<Money> Allocate(
            Money money,
            RatioArray ratios,
            int decimalPlaces,
            MidpointRounding mode)
        {
            throw new NotImplementedException();
            //return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, mode)
            //       select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }
    }
}
