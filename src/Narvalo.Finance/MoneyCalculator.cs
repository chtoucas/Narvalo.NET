// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Narvalo.Finance.Numerics;
    using Narvalo.Finance.Properties;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Add(this Money @this, decimal amount, MidpointRounding rounding)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            if (amount == 0m) { return @this; }
            return Money.Create(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, Money other, MidpointRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.Of(amount, @this.Currency)
                : new Money(amount, @this.Currency, rounding);
        }

        public static Money Add(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.Of(amount, @this.Currency)
                : Money.Create(amount, @this.Currency, rounding);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Subtract(this Money @this, decimal amount, MidpointRounding rounding)
            => Add(@this, -amount, rounding);

        public static Money Subtract(this Money @this, decimal amount, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return Add(@this, -amount, rounding);
        }

        public static Money Subtract(this Money @this, Money other, MidpointRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.Of(amount, @this.Currency)
                : new Money(amount, @this.Currency, rounding);
        }

        public static Money Subtract(this Money @this, Money other, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.Of(amount, @this.Currency)
                : Money.Create(amount, @this.Currency, rounding);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Multiply(this Money @this, decimal multiplier, MidpointRounding rounding)
            => new Money(multiplier * @this.Amount, @this.Currency, rounding);

        public static Money Multiply(this Money @this, decimal multiplier, IDecimalRounding rounding)
        {
            Expect.NotNull(rounding);
            return Money.Create(multiplier * @this.Amount, @this.Currency, rounding);
        }
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Divide(this Money @this, decimal divisor, MidpointRounding rounding)
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }

        public static Money Divide(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.True(divisor != 0m);
            Expect.NotNull(rounding);
            return Money.Create(@this.Amount / divisor, @this.Currency, rounding);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, MidpointRounding rounding)
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }

        public static Money Remainder(this Money @this, decimal divisor, IDecimalRounding rounding)
        {
            Expect.True(divisor != 0m);
            Expect.NotNull(rounding);
            return Money.Create(@this.Amount % divisor, @this.Currency, rounding);
        }
    }

    // Other calculations.
    public static partial class MoneyCalculator
    {
        public static Money Abs(Money money)
            => money.IsPositiveOrZero ? money : money.Negate();

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Math.DivRem().")]
        public static Money DivRem(Money money, decimal divisor, out decimal remainder)
        {
            Expect.True(divisor != 0m);

            var q = money.Divide(divisor);
            // NB: remainder = dividend % divisor is slower.
            remainder = money.Amount - q.Amount * divisor;
            return q;
        }

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
            MidpointRounding rounding)
        {
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
            MidpointRounding rounding)
        {
            Warrant.NotNull<IEnumerable<Money>>();

            var q = rounding.Round(money.Amount / parts, decimalPlaces);
            var seq = GetDistribution(money.Amount, parts, q);

            return from _ in seq select Money.Of(_, money.Currency);
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

            return from _ in seq select Money.Of(_, money.Currency);
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
            MidpointRounding rounding)
        {
            throw new NotImplementedException();
            //return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, mode)
            //       select new Money(_, money.Currency, MoneyRounding.Unnecessary);
        }

        public static IEnumerable<Money> Allocate(
            Money money,
            RatioArray ratios,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, rounding)
                   select Money.Of(_, money.Currency);
        }
    }

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static void ThrowIfCurrencyMismatch(Money @this, Money that, string parameterName)
            => Enforce.True(@this.Currency != that.Currency, parameterName, Strings.Argument_CurrencyMismatch);
    }
}
