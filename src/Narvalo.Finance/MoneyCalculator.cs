// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Finance.Numerics;
    using Narvalo.Finance.Properties;

    // Addition with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Plus(this Money @this, decimal amount)
            => Plus(@this, amount, RoundingMode.Default);

        public static Money Plus(this Money @this, decimal amount, RoundingMode mode)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, mode);
        }

        public static Money Plus(this Money @this, decimal amount, IMoneyRounding rounding)
        {
            if (amount == 0m) { return @this; }
            return new Money(@this.Amount + amount, @this.Currency, rounding);
        }

        public static Money Plus(this Money @this, Money other)
            => Plus(@this, other, RoundingMode.Default);

        public static Money Plus(this Money @this, Money other, RoundingMode mode)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { mode = RoundingMode.Unnecessary; }

            return new Money(@this.Amount + other.Amount, @this.Currency, mode);
        }

        public static Money Plus(this Money @this, Money other, IMoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount + other.Amount, @this.Currency, RoundingMode.Unnecessary)
                : new Money(@this.Amount + other.Amount, @this.Currency, rounding);
        }
    }

    // Subtraction with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Minus(this Money @this, decimal amount)
            => Plus(@this, -amount, RoundingMode.Default);

        public static Money Minus(this Money @this, decimal amount, RoundingMode mode)
            => Plus(@this, -amount, mode);

        public static Money Minus(this Money @this, decimal amount, IMoneyRounding rounding)
            => Plus(@this, -amount, rounding);

        public static Money Minus(this Money @this, Money other)
            => Minus(@this, other, RoundingMode.Default);

        public static Money Minus(this Money @this, Money other, RoundingMode mode)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            if (@this.IsNormalized && other.IsNormalized) { mode = RoundingMode.Unnecessary; }

            return new Money(@this.Amount - other.Amount, @this.Currency, mode);
        }

        public static Money Minus(this Money @this, Money other, IMoneyRounding rounding)
        {
            ThrowIfCurrencyMismatch(@this, other, nameof(other));

            return @this.IsNormalized && other.IsNormalized
                ? new Money(@this.Amount - other.Amount, @this.Currency, RoundingMode.Unnecessary)
                : new Money(@this.Amount - other.Amount, @this.Currency, rounding);
        }
    }

    // Multiplication with rounding.
    public static partial class MoneyCalculator
    {
        public static Money MultiplyBy(this Money @this, decimal multiplier)
            => MultiplyBy(@this, multiplier, RoundingMode.Default);

        public static Money MultiplyBy(this Money @this, decimal multiplier, RoundingMode mode)
            => new Money(multiplier * @this.Amount, @this.Currency, mode);

        public static Money MultiplyBy(this Money @this, decimal multiplier, IMoneyRounding rounding)
            => new Money(multiplier * @this.Amount, @this.Currency, rounding);
    }

    // Division with rounding.
    public static partial class MoneyCalculator
    {
        public static Money DivideBy(this Money @this, decimal divisor)
            => DivideBy(@this, divisor, RoundingMode.Default);

        public static Money DivideBy(this Money @this, decimal divisor, RoundingMode mode)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount / divisor, @this.Currency, mode);
        }

        public static Money DivideBy(this Money @this, decimal divisor, IMoneyRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount / divisor, @this.Currency, rounding);
        }
    }

    // Remainder with rounding.
    public static partial class MoneyCalculator
    {
        public static Money Modulo(this Money @this, decimal divisor)
            => Modulo(@this, divisor, RoundingMode.Default);

        public static Money Modulo(this Money @this, decimal divisor, RoundingMode mode)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount % divisor, @this.Currency, mode);
        }

        public static Money Modulo(this Money @this, decimal divisor, IMoneyRounding rounding)
        {
            if (divisor == 0m) { throw new DivideByZeroException(); }
            return new Money(@this.Amount % divisor, @this.Currency, rounding);
        }
    }

    // Allocation / Distribution.
    public static partial class MoneyCalculator
    {
        public static IEnumerable<Money> Distribute(this Money @this, int decimalPlaces, int parts)
            => Distribute(@this, decimalPlaces, parts, RoundingMode.Default);

        public static IEnumerable<Money> Distribute(
            this Money @this,
            int decimalPlaces,
            int parts,
            RoundingMode mode)
        {
            //return DecimalCalculator.Distribute(@this.Amount, decimalPlaces, parts, rounding);
            throw new NotImplementedException();
        }

        public static IEnumerable<Money> Allocate(this Money @this, int decimalPlaces, int percentage)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentage(percentage), RoundingMode.Default);

        public static IEnumerable<Money> Allocate(this Money @this, int decimalPlaces, int[] percentages)
            => Allocate(@this, decimalPlaces, RatioArray.FromPercentages(percentages), RoundingMode.Default);

        public static IEnumerable<Money> Allocate(this Money @this, int decimalPlaces, decimal ratio)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratio), RoundingMode.Default);

        public static IEnumerable<Money> Allocate(this Money @this, int decimalPlaces, decimal[] ratios)
            => Allocate(@this, decimalPlaces, RatioArray.Of(ratios), RoundingMode.Default);

        public static IEnumerable<Money> Allocate(
            this Money @this,
            int decimalPlaces,
            RatioArray ratios,
            RoundingMode mode)
        {
            //return DecimalCalculator.Allocate(@this.Amount, decimalPlaces, ratios, rounding);
            throw new NotImplementedException();
        }
    }

    // Helpers.
    public static partial class MoneyCalculator
    {
        private static void ThrowIfCurrencyMismatch(Money @this, Money that, string parameterName)
            => Enforce.True(@this.Currency != that.Currency, parameterName, Strings.Argument_CurrencyMismatch);
    }
}
