// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;

    using Calculator = Narvalo.Finance.MoneyCalculator;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            if (amount == 0m) { return @this; }
            return MoneyFactory.Create(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
                : MoneyFactory.Create(amount, @this.Currency, rounding);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return Add(@this, -amount, rounding);
        }

        public static Money Subtract(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            @this.ThrowIfCurrencyMismatch(other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfMajor(amount, @this.Currency)
                : MoneyFactory.Create(amount, @this.Currency, rounding);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money @this, decimal multiplier, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return MoneyFactory.Create(multiplier * @this.Amount, @this.Currency, rounding);
        }
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return MoneyFactory.Create(@this.Amount / divisor, @this.Currency, rounding);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return MoneyFactory.Create(@this.Amount % divisor, @this.Currency, rounding);
        }
    }

    // LINQ-like Sum().
    public static partial class MoneyCalculator
    {
        // Optimized version of: @this.Select(_ => _.Normalize(rounding)).Sum().
        public static Money Sum(this IEnumerable<Money> @this, IDecimalRounding rounding)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(rounding, nameof(rounding));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, rounding);

                while (it.MoveNext())
                {
                    item = it.Current;

                    Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, rounding);
                }

                return Money.OfMajor(sum, currency);
            }

            EMPTY_COLLECTION:
            return Money.OfMajor(0, Currency.None);
        }

        // Optimized version of: @this.Select(_ => _.Normalize(rounding)).Sum().
        public static Money Sum(this IEnumerable<Money?> @this, IDecimalRounding rounding)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(rounding, nameof(rounding));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, rounding);

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, rounding);
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
        // Optimized version of: @this.Select(_ => _.Normalize(rounding)).Average().Normalize(mode).
        public static Money Average(this IEnumerable<Money> @this, IDecimalRounding rounding)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(rounding, nameof(rounding));

            using (IEnumerator<Money> it = @this.GetEnumerator())
            {
                if (!it.MoveNext()) { throw new InvalidOperationException("XXX"); }

                Money item = it.Current;
                Currency currency = item.Currency;
                decimal sum = NormalizeAmount(item, rounding);
                long count = 1;

                while (it.MoveNext())
                {
                    item = it.Current;

                    Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                    sum += NormalizeAmount(item, rounding);
                    count++;
                }

                return MoneyFactory.Create(sum / count, currency, rounding);
            }
        }

        // Optimized version of: @this.Select(_ => _.Normalize(rounding)).Average().Normalize(mode).
        public static Money? Average(this IEnumerable<Money?> @this, IDecimalRounding rounding)
        {
            Require.NotNull(@this, nameof(@this));
            Require.NotNull(rounding, nameof(rounding));

            using (IEnumerator<Money?> it = @this.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Money? current = it.Current;
                    if (!current.HasValue) { continue; }

                    Money item = current.Value;
                    Currency currency = item.Currency;
                    decimal sum = NormalizeAmount(item, rounding);
                    long count = 1;

                    while (it.MoveNext())
                    {
                        current = it.Current;

                        if (current.HasValue)
                        {
                            item = current.Value;

                            Calculator.ThrowIfCurrencyMismatch(item.Currency, currency);

                            sum += NormalizeAmount(item, rounding);
                            count++;
                        }
                    }

                    return MoneyFactory.Create(sum / count, currency, rounding);
                }
            }

            return null;
        }
    }

    // Distribute.
    public static partial class MoneyCalculator
    {
        public static IEnumerable<Money> Distribute(this Money @this, int count, IDecimalRounding rounding)
        {
            Require.Range(count > 1, nameof(count));
            Require.NotNull(rounding, nameof(rounding));
            Warrant.NotNull<IEnumerable<Money>>();

            Currency currency = @this.Currency;
            decimal total = @this.Amount;

            decimal q = rounding.Round(total / count, @this.Currency.DecimalPlaces);
            Money part = Money.OfMajor(q, currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return MoneyFactory.Create(total - (count - 1) * q, currency, rounding);
        }

        public static IEnumerable<Money> Distribute(this Money @this, RatioArray ratios, IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            Currency currency = @this.Currency;
            decimal total = @this.Amount;
            short decimalPlaces = currency.DecimalPlaces;

            int len = ratios.Length;
            var dist = new decimal[len];
            decimal last = total;

            for (var i = 0; i < len - 1; i++)
            {
                decimal amount = rounding.Round(ratios[i] * total, decimalPlaces);
                last -= amount;
                yield return Money.OfMajor(amount, currency);
            }

            yield return MoneyFactory.Create(last, currency, rounding);
        }
    }

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static decimal NormalizeAmount(Money money, IDecimalRounding rounding)
            => money.IsNormalized
            ? money.Amount
            : rounding.Round(money.Amount, money.Currency.DecimalPlaces);
    }
}
