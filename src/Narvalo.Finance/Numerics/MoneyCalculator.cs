// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            Money.ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount + other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfCurrency(amount, @this.Currency)
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
            Money.ThrowIfCurrencyMismatch(@this, other, nameof(other));

            var amount = @this.Amount - other.Amount;

            return @this.IsNormalized && other.IsNormalized
                ? Money.OfCurrency(amount, @this.Currency)
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

    // LINQ Sum() extension.
    public static partial class MoneyCalculator
    {
        public static Money Sum(this IEnumerable<Money> @this, IDecimalRounding mode)
        {
            Require.NotNull(@this, nameof(@this));
            throw new NotImplementedException();
        }
    }

    // LINQ Average() extension.
    public static partial class MoneyCalculator
    {
        public static Money Average(this IEnumerable<Money> @this, IDecimalRounding mode)
        {
            Require.NotNull(@this, nameof(@this));
            throw new NotImplementedException();
        }
    }

    // Distribute.
    public static partial class MoneyCalculator
    {
        public static IEnumerable<Money> Distribute(
            Money money,
            int parts,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));
            Require.Range(parts > 0, nameof(parts));
            Warrant.NotNull<IEnumerable<Money>>();

            var q = rounding.Round(money.Amount / parts, decimalPlaces);
            var seq = Calculator.GetDistribution(money.Amount, parts, q);

            return from _ in seq select Money.OfCurrency(_, money.Currency);
        }

        public static IEnumerable<Money> Allocate(
            Money money,
            RatioArray ratios,
            int decimalPlaces,
            IDecimalRounding rounding)
        {
            Require.NotNull(rounding, nameof(rounding));

            return from _ in DecimalCalculator.Allocate(money.Amount, ratios, decimalPlaces, rounding)
                   select Money.OfCurrency(_, money.Currency);
        }
    }
}
