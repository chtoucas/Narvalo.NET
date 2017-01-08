// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Finance.Numerics;
    using Narvalo.Finance.Properties;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money @this, decimal amount, MoneyRounding rounding)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, Money other, MoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { rounding = MoneyRounding.Unnecessary; }

            return new Money(@this.Amount + other.Amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount + other.Amount, @this.Currency, MoneyRounding.Unnecessary)
                : new Money(@this.Amount + other.Amount, @this.Currency, rounding);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money @this, decimal amount, MoneyRounding rounding)
            => Add(@this, -amount, rounding);

        public static Money Subtract(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return Add(@this, -amount, rounding);
        }

        public static Money Subtract(this Money @this, Money other, MoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { rounding = MoneyRounding.Unnecessary; }

            return new Money(@this.Amount - other.Amount, @this.Currency, rounding);
        }

        public static Money Subtract(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount - other.Amount, @this.Currency, MoneyRounding.Unnecessary)
                : new Money(@this.Amount - other.Amount, @this.Currency, rounding);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money @this, decimal multiplier, MoneyRounding rounding)
            => new Money(multiplier * @this.Amount, @this.Currency, rounding);

        public static Money Multiply(this Money @this, decimal multiplier, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return new Money(multiplier * @this.Amount, @this.Currency, rounding);
        }
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money @this, decimal divisor, MoneyRounding rounding)
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }

        public static Money Divide(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.True(divisor != 0m);
            Expect.NotNull(rounding);
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, MoneyRounding rounding)
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }

        public static Money Remainder(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.True(divisor != 0m);
            Expect.NotNull(rounding);
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }
    }

    // Divide+Remainder aka DivRem.
    public static partial class MoneyCalculator
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Mimic the behaviour of Math.DivRem().")]
        public static Money DivRem(this Money @this, decimal divisor, out decimal remainder)
        {
            Expect.True(divisor != 0m);

            var q = @this.Divide(divisor);
            // NB: remainder = dividend % divisor is slower.
            remainder = @this.Amount - q.Amount * divisor;
            return q;
        }
    }

    // Other calculations.
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
            MoneyRounding rounding)
        {
            Require.True(rounding != MoneyRounding.Unnecessary, nameof(rounding));
            Require.Range(parts > 0, nameof(parts));
            Warrant.NotNull<IEnumerable<Money>>();

            return DistributeImpl(money, parts, decimalPlaces, rounding);
        }

        public static IEnumerable<Money> Distribute(
            Money money,
            int parts,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            Require.Range(parts > 0, nameof(parts));
            Warrant.NotNull<IEnumerable<Money>>();

            return DistributeImpl(money, parts, decimalPlaces, rounding);
        }

        private static IEnumerable<Money> DistributeImpl(
            Money money,
            int parts,
            int decimalPlaces,
            MoneyRounding rounding)
        {
            Warrant.NotNull<IEnumerable<Money>>();

            var q = DecimalRounding.Round(money.Amount / parts, decimalPlaces, rounding.ToRoundingMode());
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }

        private static IEnumerable<Money> DistributeImpl(
            Money money,
            int parts,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Demand.NotNull(rounding);
            Warrant.NotNull<IEnumerable<Money>>();

            var q = rounding.Round(money.Amount / parts, decimalPlaces);
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }

        private static IEnumerable<decimal> GetDistribution(decimal total, int count, decimal part)
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
            MoneyRounding rounding)
        {
            Require.True(rounding != MoneyRounding.Unnecessary, nameof(rounding));

            var mode = rounding.ToRoundingMode();

            return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, mode)
                   select new Money(_, money.Currency, MoneyRounding.Unnecessary);
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
