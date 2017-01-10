// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

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
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
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
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
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

    // Operators found in the class Math.
    public static partial class MoneyCalculator
    {
        public static Money Abs(Money money) => money.IsPositiveOrZero ? money : money.Negate();

        public static int Sign(Money money) => money < 0 ? -1 : (money > 0 ? 1 : 0);

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

        public static decimal Ceiling(Money money) => Round(money, Math.Ceiling);

        public static decimal Floor(Money money) => Round(money, Math.Floor);

        public static decimal Truncate(Money money) => Round(money, Math.Truncate);

        public static decimal Round(Money money) => Round(money, Math.Round);

        public static decimal Round(Money money, MidpointRounding mode)
            => Round(money, _ => Math.Round(_, mode));

        public static decimal Round(Money money, int decimalPlaces)
            => Round(money, decimalPlaces, MidpointRounding.ToEven);

        public static decimal Round(Money money, int decimalPlaces, MidpointRounding mode)
        {
            if (money.IsNormalized && money.Currency.DecimalPlaces == decimalPlaces) { return money.Amount; }
            return Math.Round(money.Amount, decimalPlaces, mode);
        }

        private static decimal Round(Money money, Func<decimal, decimal> round)
        {
            Demand.NotNull(round);
            if (money.IsNormalized && money.Currency.DecimalPlaces == 0) { return money.Amount; }
            return round.Invoke(money.Amount);
        }
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
        public static Money Sum(this IEnumerable<Money> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                // The main purpose for the separate treatment of the first element is to get a
                // hand on its underlying currency which will serve as a reference when we shall
                // check that all elements of the collection use the same currency.
                Money item = it.Current;
                Currency currency = item.Currency;
                bool normalized = item.IsNormalized;
                decimal sum = item.Amount;

                while (it.MoveNext())
                {
                    item = it.Current;

                    ThrowIfCurrencyMismatch(item.Currency, currency);

                    normalized = normalized && item.IsNormalized;
                    sum += item.Amount;
                }

                return new Money(sum, currency, normalized);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
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
                    Money? current = it.Current;
                    // If all elements are null, we never pass this point.
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    bool normalized = item.IsNormalized;
                    decimal sum = item.Amount;

                    // Loop over the remaining elements, if any.
                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            ThrowIfCurrencyMismatch(item.Currency, currency);

                            normalized = normalized && item.IsNormalized;
                            sum += item.Amount;
                        }
                    }

                    return new Money(sum, currency, normalized);
                }
            }

            // For an empty collection or a collection of nulls, we return zero.
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, mode);

                while (it.MoveNext())
                {
                    item = it.Current;

                    ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, mode);
                }

                return Money.OfMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Sum().
        public static Money Sum(this IEnumerable<Money?> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, mode);

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, mode);
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
        public static Money Average(this IEnumerable<Money> @this)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = item.Amount;
                long count = 1;

                while (it.MoveNext())
                {
                    item = it.Current;

                    ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += item.Amount;
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
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = item.Amount;
                    long count = 1;

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += item.Amount;
                            count++;
                        }
                    }

                    return new Money(sum / count, currency);
                }
            }

            return null;
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, mode);
                long count = 1;

                while (it.MoveNext())
                {
                    item = it.Current;

                    ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, mode);
                    count++;
                }

                return new Money(sum / count, currency, mode);
            }
        }

        // Optimized version of: @this.Select(_ => _.Normalize(mode)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> @this, MidpointRounding mode)
        {
            Require.NotNull(@this, nameof(@this));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, mode);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, mode);
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
    public static partial class MoneyCalculator
    {
        public static IEnumerable<Money> Distribute(this Money @this, int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = @this.Currency;
            decimal total = @this.Amount;

            decimal q = total / count;
            Money part = new Money(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Money(total - (count - 1) * q, currency);
        }

        public static IEnumerable<Money> Distribute(this Money @this, int count, MidpointRounding mode)
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

        public static IEnumerable<Money> Distribute(this Money @this, RatioArray ratios)
        {
            Currency currency = @this.Currency;
            decimal total = @this.Amount;

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

        public static IEnumerable<Money> Distribute(this Money @this, RatioArray ratios, MidpointRounding mode)
        {
            Currency currency = @this.Currency;
            decimal total = @this.Amount;
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

    // Helpers.
    public static partial class MoneyCalculator
    {
        internal static void ThrowIfCurrencyMismatch(Currency cy1, Currency cy2)
        {
            if (cy1 != cy2)
            {
                throw new InvalidOperationException("XXX");
            }
        }

        private static decimal NormalizeAmount(Money money, MidpointRounding mode)
            => money.IsNormalized
            ? money.Amount
            : Math.Round(money.Amount, money.Currency.DecimalPlaces, mode);
    }
}
