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
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount / divisor, @this.Currency, mode);
        }
    }

    // Remainder/Modulo with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Remainder(this Money @this, decimal divisor, MidpointRounding mode)
        {
            Expect.True(divisor != 0m);
            return new Money(@this.Amount % divisor, @this.Currency, mode);
        }
    }

    // Other calculations.
    public static partial class MoneyCalculator
    {
        #region Operators normally found in the class Math.

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
            Expect.True(divisor != 0m);

            var q = dividend.Divide(divisor);
            // REVIEW: remainder = dividend % divisor is slower for integers. What about decimals?
            // > remainder = new Money(dividend.Amount - q.Amount * divisor, dividend.Currency);
            remainder = dividend.Remainder(divisor);
            return q;
        }

        public static Money Ceiling(Money money) => Money.OfCurrency(Math.Ceiling(money.Amount), money.Currency);

        public static Money Floor(Money money) => Money.OfCurrency(Math.Floor(money.Amount), money.Currency);

        public static Money Truncate(Money money) => Money.OfCurrency(Math.Truncate(money.Amount), money.Currency);

        public static Money Round(Money money) => Money.OfCurrency(Math.Round(money.Amount), money.Currency);

        public static Money Round(Money money, int decimalPlaces)
            => Money.OfCurrency(Math.Round(money.Amount, decimalPlaces), money.Currency);

        public static Money Round(Money money, int decimalPlaces, MidpointRounding mode)
            => Money.OfCurrency(Math.Round(money.Amount, decimalPlaces, mode), money.Currency);

        public static Money Round(Money money, MidpointRounding mode)
            => Money.OfCurrency(Math.Round(money.Amount, mode), money.Currency);

        #endregion

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

            return DistributeImpl(money, parts, decimalPlaces, mode);
        }

        private static IEnumerable<Money> DistributeImpl(
            Money money,
            int parts,
            int decimalPlaces,
            MidpointRounding mode)
        {
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
