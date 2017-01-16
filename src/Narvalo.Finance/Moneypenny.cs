// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.PennyCalculator;

    // A lightweight money type.
    //
    // Despite its name, this type is not restricted to currencies with minor units of size 2
    // but does not handle arbitrary subunits.
    // Precisely, we suppose that the underlying currency has a fixed number of decimal places.
    // For currencies admitting a minor currency unit, it ensures that we can convert an amount
    // from the subunit to the main unit, and vice versa.
    // For currencies without a minor currency unit, somehow, the minor currency unit is the main
    // unit itself, and no conversion is ever needed.
    //
    // Advantages:
    // - Using an Int64 as the backing type for the amount allows for fast arithmetic operations.
    // - Rounding is no longer needed; the amount is always normalized. See below for caveats
    //   when performing a division.
    // Disadvantages:
    // - Only available for currencies specifying a fixed number of decimal places (see above),
    //   ie it rules out all withdrawn ISO currencies.
    // - The Int64 range is smaller. This has two consequences:
    //   * more opportunities to throw an overflow exception.
    //   * some operations might be lossful.
    //     Divide(long) is actually an integer division, it then rounds toward zero if needed;
    //     this is necessary to keep the operation closed. If you do not want this, you should
    //     use Divide(decimal) or DivRem() instead.
    // - Multiply(long), Divide(long) and Remainder(long) are closed but obviously not very useful.
    //   We provide decimal overloads but, for that, we no longer return a Moneypenny object.
    //
    // Remark: This class and FastMoney from JavaMoney are similar in purpose (fast operations)
    // but different in the way they deal with amounts. Here, we only consider strict amounts
    // (no rounding is ever needed, at the expense of what you can do with it), a restriction
    // that does not exist with FastMoney.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Moneypenny
        : IEquatable<Moneypenny>, IComparable<Moneypenny>, IComparable, IFormattable
    {
        private const string DEFAULT_FORMAT = "G";

        public Moneypenny(long amount, Currency currency)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        public long Amount { get; }

        public Currency Currency { get; }

        public string PennyOrCurrencyCode
            => Currency.DecimalPlaces == 0 ? Currency.Code : Currency.MinorCurrencyCode;

        public bool IsZero => Amount == 0L;
        public bool IsNegative => Amount < 0L;
        public bool IsNegativeOrZero => Amount <= 0L;
        public bool IsPositive => Amount > 0L;
        public bool IsPositiveOrZero => Amount >= 0L;

        public decimal ToMajor() => Currency.ConvertToMajor(Amount);

        public static Moneypenny Zero(Currency currency) => new Moneypenny(0L, currency);

        public static Moneypenny One(Currency currency) => new Moneypenny(1L, currency);

        internal void ThrowIfCurrencyMismatch(Moneypenny penny, string parameterName)
            => Enforce.True(Currency == penny.Currency, parameterName, Strings.Argument_CurrencyMismatch);

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => Format.Current("{0} {1:N0}", Currency.Code, Amount);
    }

    // Implements the IFormattable interface.
    public partial struct Moneypenny
    {
        public override string ToString()
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.FormatPenny(this, null, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.FormatPenny(this, format, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return ToString(null, formatProvider);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();

            if (formatProvider != null)
            {
                var fmt = formatProvider.GetFormat(GetType()) as ICustomFormatter;
                if (fmt != null)
                {
                    return fmt.Format(format, this, formatProvider);
                }
            }

            return MoneyFormatter.FormatPenny(this, format, NumberFormatInfo.GetInstance(formatProvider));
        }
    }

    // Implements the IEquatable<Cent> interface.
    public partial struct Moneypenny
    {
        public static bool operator ==(Moneypenny left, Moneypenny right) => left.Equals(right);

        public static bool operator !=(Moneypenny left, Moneypenny right) => !left.Equals(right);

        public bool Equals(Moneypenny other) => Amount == other.Amount && Currency == other.Currency;

        public override bool Equals(object obj)
        {
            if (!(obj is Moneypenny)) { return false; }

            return Equals((Moneypenny)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Amount.GetHashCode();
                hash = 31 * hash + Currency.GetHashCode();
                return hash;
            }
        }
    }

    // Implements the IComparable and IComparable<Cent> interfaces.
    public partial struct Moneypenny
    {
        public static bool operator <(Moneypenny left, Moneypenny right) => left.CompareTo(right) < 0;
        public static bool operator <=(Moneypenny left, Moneypenny right) => left.CompareTo(right) <= 0;
        public static bool operator >(Moneypenny left, Moneypenny right) => left.CompareTo(right) > 0;
        public static bool operator >=(Moneypenny left, Moneypenny right) => left.CompareTo(right) >= 0;

        public int CompareTo(Moneypenny other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            return Amount.CompareTo(other.Amount);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is Moneypenny))
            {
                throw new ArgumentException(Strings.Argument_InvalidMoneyType, nameof(obj));
            }

            return CompareTo((Moneypenny)obj);
        }
    }

    // Conversions.
    public partial struct Moneypenny
    {
        public Money ToMoney() => Money.OfMinor(Amount, Currency);

        public static explicit operator Moneypenny(long value) => new Moneypenny(value, Currency.None);

        public static implicit operator long(Moneypenny value) => value.Amount;

        public static explicit operator Money(Moneypenny value) => value.ToMoney();
    }

    // Math operators.
    public partial struct Moneypenny
    {
        public int Sign => Sign(this);

        public Moneypenny Abs() => PennyCalculator.Abs(this);

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "[Intentionally] Math.DivRem().")]
        public Moneypenny DivRem(long divisor, out Moneypenny remainder)
            => PennyCalculator.DivRem(this, divisor, out remainder);
    }

    // Overrides the op_Addition operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator +(Moneypenny left, long right) => Add(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator +(long left, Moneypenny right) => Add(right, left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator +(Moneypenny left, Moneypenny right) => Add(left, right);

        public Moneypenny Plus(long amount) => Add(this, amount);

        public Moneypenny Plus(Moneypenny other) => Add(this, other);
    }

    // Overrides the op_Subtraction operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator -(Moneypenny left, long right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator -(long left, Moneypenny right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator -(Moneypenny left, Moneypenny right) => Subtract(left, right);

        public Moneypenny Minus(Moneypenny other) => Subtract(this, other);

        public Moneypenny Minus(long amount) => Subtract(this, amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator *(long multiplier, Moneypenny penny) => Multiply(penny, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator *(Moneypenny penny, long multiplier) => Multiply(penny, multiplier);

        public Moneypenny MultiplyBy(long multiplier) => Multiply(this, multiplier);
    }

    // Overrides the op_Division operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator /(Moneypenny dividend, long divisor) => Divide(dividend, divisor);

        public Moneypenny DivideBy(long divisor) => Divide(this, divisor);
    }

    // Overrides the op_Modulus operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator %(Moneypenny dividend, long divisor) => Modulus(dividend, divisor);

        public Moneypenny Remainder(long divisor) => Modulus(this, divisor);
    }

    // Overrides the op_Increment operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator ++(Moneypenny penny) => PennyCalculator.Increment(penny);

        public Moneypenny Increment() => PennyCalculator.Increment(this);
    }

    // Overrides the op_Decrement operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator --(Moneypenny penny) => PennyCalculator.Decrement(penny);

        public Moneypenny Decrement() => PennyCalculator.Decrement(this);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See PennyCalculator.")]
        public static Moneypenny operator -(Moneypenny penny) => PennyCalculator.Negate(penny);

        public Moneypenny Negate() => PennyCalculator.Negate(this);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Moneypenny
    {
        // This operator does nothing, only added for completeness.
        public static Moneypenny operator +(Moneypenny penny) => penny;

        public Moneypenny Plus() => this;
    }
}
