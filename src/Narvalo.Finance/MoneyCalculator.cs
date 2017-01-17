// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Internal;

    // Standard binary math operators.
    public static partial class MoneyCalculator
    {
        #region Add()

        public static Money Add(Money left, Money right)
        {
            left.ThrowIfCurrencyMismatch(right, nameof(right));

            if (left.Amount == 0m) { return right; }
            if (right.Amount == 0m) { return left; }
            return new Money(left.Amount + right.Amount, left.Currency, left.IsNormalized && right.IsNormalized);
        }

        [CLSCompliant(false)]
        public static Money Add(Money money, uint amount)
        {
            if (amount == 0) { return money; }
            return new Money(money.Amount + amount, money.Currency, money.IsNormalized);
        }

        [CLSCompliant(false)]
        public static Money Add(Money money, ulong amount)
        {
            if (amount == 0UL) { return money; }
            return new Money(money.Amount + amount, money.Currency, money.IsNormalized);
        }

        public static Money Add(Money money, int amount)
        {
            if (amount == 0) { return money; }
            return new Money(money.Amount + amount, money.Currency, money.IsNormalized);
        }

        public static Money Add(Money money, long amount)
        {
            if (amount == 0L) { return money; }
            return new Money(money.Amount + amount, money.Currency, money.IsNormalized);
        }

        public static Money Add(Money money, decimal amount)
        {
            if (amount == 0m) { return money; }
            return new Money(money.Amount + amount, money.Currency, false);
        }

        #endregion

        #region Substract()

        public static Money Subtract(Money left, Money right)
        {
            left.ThrowIfCurrencyMismatch(right, nameof(right));

            if (left.Amount == 0m) { return right.Negate(); }
            if (right.Amount == 0m) { return left; }
            return new Money(left.Amount - right.Amount, left.Currency, left.IsNormalized && right.IsNormalized);
        }

        [CLSCompliant(false)]
        public static Money Subtract(Money money, uint amount)
        {
            if (amount == 0) { return money; }
            return new Money(money.Amount - amount, money.Currency, money.IsNormalized);
        }

        [CLSCompliant(false)]
        public static Money Subtract(Money money, ulong amount)
        {
            if (amount == 0UL) { return money; }
            return new Money(money.Amount - amount, money.Currency, money.IsNormalized);
        }

        public static Money Subtract(Money money, int amount) => Add(money, -amount);

        public static Money Subtract(Money money, long amount) => Add(money, -amount);

        public static Money Subtract(Money money, decimal amount) => Add(money, -amount);

        #region Subtraction where the Money object is on the right.

        [CLSCompliant(false)]
        public static Money Subtract(uint amount, Money money)
            => new Money(amount - money.Amount, money.Currency, money.IsNormalized);

        [CLSCompliant(false)]
        public static Money Subtract(ulong amount, Money money)
            => new Money(amount - money.Amount, money.Currency, money.IsNormalized);

        public static Money Subtract(int amount, Money money)
            => new Money(amount - money.Amount, money.Currency, money.IsNormalized);

        public static Money Subtract(long amount, Money money)
            => new Money(amount - money.Amount, money.Currency, money.IsNormalized);

        public static Money Subtract(decimal amount, Money money)
        {
            if (amount == 0m) { return money.Negate(); }
            return new Money(amount - money.Amount, money.Currency, false);
        }

        #endregion

        #endregion

        #region Multiply()

        [CLSCompliant(false)]
        public static Money Multiply(Money money, uint multiplier)
            => new Money(multiplier * money.Amount, money.Currency, money.IsNormalized);

        [CLSCompliant(false)]
        public static Money Multiply(Money money, ulong multiplier)
            => new Money(multiplier * money.Amount, money.Currency, money.IsNormalized);

        public static Money Multiply(Money money, int multiplier)
            => new Money(multiplier * money.Amount, money.Currency, money.IsNormalized);

        public static Money Multiply(Money money, long multiplier)
            => new Money(multiplier * money.Amount, money.Currency, money.IsNormalized);

        public static Money Multiply(Money money, decimal multiplier)
            => new Money(multiplier * money.Amount, money.Currency, false);

        #endregion

        public static Money Divide(Money dividend, decimal divisor)
            => new Money(dividend.Amount / divisor, dividend.Currency, false);

        public static Money Modulo(Money dividend, decimal divisor)
            => new Money(dividend.Amount % divisor, dividend.Currency, false);
    }

    // Standard binary math operators under which the Money type is not closed.
    public static partial class MoneyCalculator
    {
        // This division returns a decimal (we lost the currency unit).
        // It is a lot like computing a percentage (if multiplied by 100, of course).
        public static decimal Divide(Money dividend, Money divisor) => dividend.Amount / divisor.Amount;
    }

    // Other math operators.
    public static partial class MoneyCalculator
    {
        public static Money Abs(Money money) => money.IsPositiveOrZero ? money : money.Negate();

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
        public static Money DivRem(Money dividend, long divisor, out Money remainder)
        {
            // REVIEW: remainder = dividend % divisor is slower for integers. What about decimals?
            // > var q = dividend.DivideBy(divisor);
            // > remainder = dividend.Modulo(divisor);
            // REVIEW: for doubles, .NET uses:
            // > Modulus = (Math.Abs(dividend) - (Math.Abs(divisor)
            // >   * (Math.Floor(Math.Abs(dividend) / Math.Abs(divisor)))))
            // >   * Math.Sign(dividend)
            decimal q = dividend.Amount / divisor;
            decimal rem = dividend.Amount - q * divisor;
            remainder = new Money(rem, dividend.Currency);
            return new Money(q, dividend.Currency);
        }
    }

    // Rouding operators.
    public static partial class MoneyCalculator
    {
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
            // If the amount is already rounded to decimalPlaces, do nothing.
            if (money.IsRounded && money.Currency.DecimalPlaces == decimalPlaces) { return money.Amount; }
            return Math.Round(money.Amount, decimalPlaces, mode);
        }

        private static decimal Round(Money money, Func<decimal, decimal> thunk)
        {
            Demand.NotNull(thunk);
            // If the amount is already rounded to 0, do nothing.
            if (money.IsRounded && money.Currency.DecimalPlaces == 0) { return money.Amount; }
            return thunk.Invoke(money.Amount);
        }
    }

    // Standard binary math operators with rounding.
    public static partial class MoneyCalculator
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

        public static Money Modulo(Money dividend, decimal divisor, MidpointRounding mode)
            => Money.FromMajor(dividend.Amount % divisor, dividend.Currency, mode);
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
            return Money.FromMajor(0, Currency.None);
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
            return Money.FromMajor(0, Currency.None);
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

                return Money.FromMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.FromMajor(0, Currency.None);
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

                    return Money.FromMajor(sum, currency);
                }
            }

            return Money.FromMajor(0, Currency.None);
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

                return Money.FromMajor(sum / count, currency, mode);
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

                    return Money.FromMajor(sum / count, currency, mode);
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
