// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Finance.Numerics;
    using Narvalo.Finance.Properties;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Plus(this Money @this, decimal amount, MoneyRounding rounding)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Plus(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Plus(this Money @this, Money other, MoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { rounding = MoneyRounding.Unnecessary; }

            return new Money(@this.Amount + other.Amount, @this.Currency, rounding);
        }

        public static Money Plus(this Money @this, Money other, IDecimalRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount + other.Amount, @this.Currency, MoneyRounding.Unnecessary)
                : new Money(@this.Amount + other.Amount, @this.Currency, rounding);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Minus(this Money @this, decimal amount, MoneyRounding rounding)
            => Plus(@this, -amount, rounding);

        public static Money Minus(this Money @this, decimal amount, IDecimalRounding rounding)
            => Plus(@this, -amount, rounding);

        public static Money Minus(this Money @this, Money other, MoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { rounding = MoneyRounding.Unnecessary; }

            return new Money(@this.Amount - other.Amount, @this.Currency, rounding);
        }

        public static Money Minus(this Money @this, Money other, IDecimalRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount - other.Amount, @this.Currency, MoneyRounding.Unnecessary)
                : new Money(@this.Amount - other.Amount, @this.Currency, rounding);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money MultiplyBy(this Money @this, decimal multiplier, MoneyRounding rounding)
            => new Money(multiplier * @this.Amount, @this.Currency, rounding);

        public static Money MultiplyBy(this Money @this, decimal multiplier, IDecimalRounding rounding)
            => new Money(multiplier * @this.Amount, @this.Currency, rounding);
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money DivideBy(this Money @this, decimal divisor, MoneyRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }

        public static Money DivideBy(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }
    }

    // Remainder with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Modulo(this Money @this, decimal divisor, MoneyRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }

        public static Money Modulo(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }
    }

    // Allocation / Distribution.
    public static partial class MoneyCalculator
    {
        private static Func<decimal, decimal> s_Id = _ => _;

        #region Distribute

        public static IEnumerable<Money> Distribute(
            Money money,
            int parts,
            int decimalPlaces,
            MoneyRounding rounding)
        {
            Require.True(rounding != MoneyRounding.Unnecessary, nameof(rounding));
            Require.Range(parts >= 0, nameof(parts));
            if (parts == 0) { throw new DivideByZeroException(); }

            return DistributeImpl(money, parts, decimalPlaces, rounding);
        }

        public static IEnumerable<Money> Distribute(
            Money money,
            int parts,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            Require.Range(parts >= 0, nameof(parts));
            if (parts == 0) { throw new DivideByZeroException(); }

            return DistributeImpl(money, parts, decimalPlaces, rounding);
        }

        private static IEnumerable<Money> DistributeImpl(
            Money money,
            int parts,
            int decimalPlaces,
            MoneyRounding rounding)
        {
            if (rounding == MoneyRounding.None)
            {
                var q = money.Amount / parts;
                var seq = GetDistribution(money.Amount, parts, q);

                return from _ in seq select new Money(_, money.Currency, MoneyRounding.None);
            }
            else
            {
                var q = DecimalRounding.Round(money.Amount / parts, decimalPlaces, rounding.ToRoundingMode());
                var seq = GetDistribution(money.Amount, parts, q);

                return from _ in seq select new Money(_, money.Currency, MoneyRounding.Unnecessary);
            }
        }

        private static IEnumerable<Money> DistributeImpl(
            Money money,
            int parts,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            var q = rounding.Round(money.Amount / parts, decimalPlaces);
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }

        private static IEnumerable<decimal> GetDistribution(decimal total, int count, decimal part)
        {
            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return total - (count - 1) * part;
        }

        #endregion

        public static IEnumerable<Money> Allocate(
            Money money,
            RatioArray ratios,
            int decimalPlaces,
            MoneyRounding rounding)
        {
            Require.True(rounding != MoneyRounding.Unnecessary, nameof(rounding));

            if (rounding == MoneyRounding.None)
            {
                return from _ in DecimalCalculator.Allocate(money.Amount, ratios)
                       select new Money(_, money.Currency, MoneyRounding.None);
            }
            else
            {
                var mode = rounding.ToRoundingMode();

                return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, mode)
                       select new Money(_, money.Currency, MoneyRounding.Unnecessary);
            }
        }

        public static IEnumerable<Money> Allocate(
            Money money,
            RatioArray ratios,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, rounding)
                   select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }
    }

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static void ThrowIfCurrencyMismatch(Money @this, Money that, string parameterName)
            => Enforce.True(@this.Currency != that.Currency, parameterName, Strings.Argument_CurrencyMismatch);
    }
}
