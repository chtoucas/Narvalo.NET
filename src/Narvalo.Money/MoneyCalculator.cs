// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Internal;

    // Standard binary math operators.
    public static partial class MoneyCalculator
    {
        #region Add()

        public static Money Add(Money left, Money right) => left.Plus(right);

        [CLSCompliant(false)]
        public static Money Add(Money money, uint amount) => money.Plus(amount);

        [CLSCompliant(false)]
        public static Money Add(Money money, ulong amount) => money.Plus(amount);

        public static Money Add(Money money, int amount) => money.Plus(amount);

        public static Money Add(Money money, long amount) => money.Plus(amount);

        public static Money Add(Money money, decimal amount) => money.Plus(amount);

        #endregion

        #region Substract()

        public static Money Subtract(Money left, Money right) => left.Minus(right);

        [CLSCompliant(false)]
        public static Money Subtract(Money money, uint amount) => money.Minus(amount);

        [CLSCompliant(false)]
        public static Money Subtract(Money money, ulong amount) => money.Minus(amount);

        public static Money Subtract(Money money, int amount) => money.Minus(amount);

        public static Money Subtract(Money money, long amount) => money.Minus(amount);

        public static Money Subtract(Money money, decimal amount) => money.Minus(amount);

        #endregion

        #region Multiply()

        [CLSCompliant(false)]
        public static Money Multiply(Money money, uint multiplier) => money.MultiplyBy(multiplier);

        [CLSCompliant(false)]
        public static Money Multiply(Money money, ulong multiplier) => money.MultiplyBy(multiplier);

        public static Money Multiply(Money money, int multiplier) => money.MultiplyBy(multiplier);

        public static Money Multiply(Money money, long multiplier) => money.MultiplyBy(multiplier);

        public static Money Multiply(Money money, decimal multiplier) => money.MultiplyBy(multiplier);

        #endregion

        public static Money Divide(Money dividend, decimal divisor) => dividend.DivideBy(divisor);

        public static Money Remainder(Money dividend, decimal divisor) => dividend.Mod(divisor);
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
            // > remainder = dividend.Remainder(divisor);
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

    // LINQ-like Sum().
    //
    // For collections of nullable moneys, the signature of Sum() differs from the one found
    // in .NET; instead of returning a nullable money, we return a money. Anyway, despite what it
    // advertises, Sum() from LINQ never returns null.
    //
    // We do not include all variants of Sum() since they are easy to implement: if source is
    // of type IEnumerable<TSource> (the rounding variants are defined in RoudingMachine):
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
    public static partial class MoneyCalculator
    {
        public static Money Sum(IEnumerable<Money> monies)
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

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    normalized = normalized && mny.IsNormalized;
                    sum += mny.Amount;
                }

                return new Money(sum, currency, normalized);
            }

            EMPTY_COLLECTION:
            return Money.FromMajor(0, Currency.None);
        }

        public static Money Sum(IEnumerable<Money?> monies)
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

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

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
    }

    // LINQ-like Average().
    //
    // As for Sum(), we do not implement all variants of Average(): if source is of type
    // IEnumerable<TSource> (the rounding variants are defined in RoudingMachine):
    // - source.Average(Func<TSource, Money> selector) == source.Select(selector).Average()
    // - source.Average(Func<TSource, Money> selector, MidpointRounding mode) == source.Select(selector).Average(mode)
    // - source.Average(Func<TSource, Money?> selector) ==  source.Select(selector).Average()
    // - source.Average(Func<TSource, Money?> selector, MidpointRounding mode) == source.Select(selector).Average(mode)
    public static partial class MoneyCalculator
    {
        // NB: The result is ALWAYS denormalized.
        public static Money Average(IEnumerable<Money> monies)
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

                    MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                    sum += mny.Amount;
                    count++;
                }

                return new Money(sum / count, currency);
            }
        }

        // NB: The result is ALWAYS denormalized.
        public static Money? Average(IEnumerable<Money?> monies)
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

                            MoneyHelpers.ThrowIfCurrencyMismatch(mny, currency);

                            sum += mny.Amount;
                            count++;
                        }
                    }

                    return new Money(sum / count, currency);
                }
            }

            return null;
        }
    }
}
