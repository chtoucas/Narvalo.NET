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
        public static Money Ceiling(Money money) => money.Normalize(Math.Ceiling);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Floor(Money money) => money.Normalize(Math.Floor);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Truncate(Money money) => money.Normalize(Math.Truncate);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money) => money.Normalize(Math.Round);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, MidpointRounding mode)
            => money.Normalize(_ => Math.Round(_, mode));

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, int decimalPlaces)
            => money.Round(decimalPlaces, MidpointRounding.ToEven);

        // DANGEROUS ZONE: You might lose or gain money.
        public static Money Round(Money money, int decimalPlaces, MidpointRounding mode)
            => money.Round(decimalPlaces, mode);
    }

    // LINQ extensions.
    public static partial class MoneyCalculator
    {
        public static Money Sum(this IEnumerable<Money> @this)
        {
            throw new NotImplementedException();
        }

        public static Money Sum(this IEnumerable<Money> @this, MidpointRounding rounding)
        {
            throw new NotImplementedException();
        }

        public static Money Average(this IEnumerable<Money> @this)
        {
            throw new NotImplementedException();
        }

        public static Money Average(this IEnumerable<Money> @this, MidpointRounding rounding)
        {
            throw new NotImplementedException();
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
