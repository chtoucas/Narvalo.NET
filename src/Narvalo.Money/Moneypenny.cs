// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Generic;
    using Narvalo.Globalization;
    using Narvalo.Properties;
    using Narvalo.Finance.Rounding;

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
    //     DivideBy(long) is actually an integer division, it then rounds toward zero if needed;
    //     this is necessary to keep the operation closed. If you do not want this, you should
    //     use Divide(decimal) or DivRem() instead.
    // - MultiplyBy(long), DivideBy(long) and Mod(long) are closed but obviously not very useful.
    //   We provide decimal overloads but, for that, we no longer return a Moneypenny object.
    //
    // Remark: This class and FastMoney from JavaMoney are similar in purpose (fast operations)
    // but different in the way they deal with amounts. Here, we only consider strict amounts
    // (no rounding is ever needed, at the expense of what you can do with it), a restriction
    // that does not exist with FastMoney.
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Moneypenny : Internal.IMoneypenny<Moneypenny, Currency>
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
        public int Sign => Amount < 0L ? -1 : (Amount > 0L ? 1 : 0);

        public static Moneypenny Zero(Currency currency) => new Moneypenny(0L, currency);

        public static Moneypenny One(Currency currency) => new Moneypenny(1L, currency);

        internal void ThrowIfCurrencyMismatch(Moneypenny penny, string parameterName)
            => Enforce.True(Currency == penny.Currency, parameterName, Strings.Argument_CurrencyMismatch);

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => Format.Current("{0} {1:N0}", Currency.Code, Amount);
    }

    // Factory methods.
    public partial struct Moneypenny
    {
        public static Moneypenny FromMinor(decimal minor, Currency currency, MidpointRounding mode)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));

            decimal amount = Math.Round(minor, mode);
            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny FromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));
            Require.NotNull(adjuster, nameof(adjuster));

            decimal amount = adjuster.Round(minor);
            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny FromMajor(decimal major, Currency currency, MidpointRounding mode)
            => FromMinor(currency.ConvertToMinor(major), currency, mode);

        public static Moneypenny FromMajor(decimal major, Currency currency, IRoundingAdjuster adjuster)
            => FromMinor(currency.ConvertToMinor(major), currency, adjuster);

        public static Moneypenny? TryCreateFromMinor(decimal minor, Currency currency, MidpointRounding mode)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));

            decimal amount = Math.Round(minor, mode);
            if (amount < Int64.MinValue || amount > Int64.MaxValue) { return null; }

            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny? TryCreateFromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));
            Require.NotNull(adjuster, nameof(adjuster));

            decimal amount = adjuster.Round(minor);
            if (amount < Int64.MinValue || amount > Int64.MaxValue) { return null; }

            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, MidpointRounding mode)
            => TryCreateFromMinor(currency.ConvertToMinor(major), currency, mode);

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, IRoundingAdjuster adjuster)
            => TryCreateFromMinor(currency.ConvertToMinor(major), currency, adjuster);
    }

    // Domain-specific conversions.
    public partial struct Moneypenny
    {
        public decimal ToMajor() => Currency.ConvertToMajor(Amount);

        public Money ToMoney() => new Money(ToMajor(), Currency, true);

        public Money<TCurrency> ToMoney<TCurrency>() where TCurrency : Currency<TCurrency>
        {
            if (Currency.Code != Money<TCurrency>.UnderlyingUnit.Code)
            {
                throw new InvalidOperationException("XXX");
            }

            return new Money<TCurrency>(ToMajor(), true);
        }

        // Explicit conversion that should not fail.
        public static explicit operator Money(Moneypenny value) => value.ToMoney();
    }

    // Numeric conversions.
    public partial struct Moneypenny
    {
        [CLSCompliant(false)]
        public sbyte ToSByte() => Convert.ToSByte(Amount);

        [CLSCompliant(false)]
        public ushort ToUInt16() => Convert.ToUInt16(Amount);

        [CLSCompliant(false)]
        public uint ToUInt32() => Convert.ToUInt32(Amount);

        [CLSCompliant(false)]
        public ulong ToUInt64() => Convert.ToUInt64(Amount);

        public byte ToByte() => Convert.ToByte(Amount);

        public short ToInt16() => Convert.ToInt16(Amount);

        public int ToInt32() => Convert.ToInt32(Amount);

        // Lossless conversion.
        public long ToInt64() => Amount;

        // Lossless conversion.
        public decimal ToDecimal() => Amount;

        // NB: Implicit conversion (no unexpected loss of precision, fast & completely harmless).
        public static implicit operator long(Moneypenny value) => value.Amount;

        #region Harmless explicit conversions from a numeric type to Moneypenny w/o Currency.

        // No conversion from ulong since there is a risk of overflow.

        [CLSCompliant(false)]
        public static explicit operator Moneypenny(sbyte value) => new Moneypenny(value, Currency.None);

        [CLSCompliant(false)]
        public static explicit operator Moneypenny(ushort value) => new Moneypenny(value, Currency.None);

        [CLSCompliant(false)]
        public static explicit operator Moneypenny(uint value) => new Moneypenny(value, Currency.None);

        public static explicit operator Moneypenny(byte value) => new Moneypenny(value, Currency.None);

        public static explicit operator Moneypenny(short value) => new Moneypenny(value, Currency.None);

        public static explicit operator Moneypenny(int value) => new Moneypenny(value, Currency.None);

        public static explicit operator Moneypenny(long value) => new Moneypenny(value, Currency.None);

        #endregion
    }

    // Implements the IFormattable interface.
    public partial struct Moneypenny
    {
        public override string ToString() => FormatImpl(null, NumberFormatInfo.CurrentInfo);

        public string ToString(string format) => FormatImpl(format, NumberFormatInfo.CurrentInfo);

        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                var fmtr = formatProvider.GetFormat(typeof(Moneypenny)) as ICustomFormatter;
                if (fmtr != null) { return fmtr.Format(format, this, formatProvider); }
            }

            return FormatImpl(format, NumberFormatInfo.GetInstance(formatProvider));
        }

        private string FormatImpl(string format, NumberFormatInfo info)
        {
            Demand.NotNull(info);

            MoneyFormat spec;

            if (format == null || format.Length == 0)
            {
                spec = new MoneyFormat('G', 0);
            }
            else if (format.Length == 1)
            {
                spec = new MoneyFormat(format[0], 0);
            }
            else
            {
                throw new FormatException("XXX");
            }

            return MoneyFormatter.FormatMoney(spec, Amount, PennyOrCurrencyCode, info);
        }
    }

    // Implements the IEquatable<Cent> interface.
    public partial struct Moneypenny
    {
        public static bool operator ==(Moneypenny left, Moneypenny right) => left.Equals(right);

        public static bool operator !=(Moneypenny left, Moneypenny right) => !left.Equals(right);

        public bool Equals(Moneypenny other) => Amount == other.Amount && Currency == other.Currency;

        public override bool Equals(object obj) => (obj is Moneypenny) && Equals((Moneypenny)obj);

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

    // Overrides the op_Addition operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Moneypenny operator +(Moneypenny left, long right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Moneypenny operator +(long left, Moneypenny right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Moneypenny operator +(Moneypenny left, Moneypenny right) => left.Plus(right);

        public Moneypenny Plus(Moneypenny other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0L) { return other; }
            if (other.Amount == 0L) { return this; }
            return new Moneypenny(checked(Amount + other.Amount), Currency);
        }

        public Moneypenny Plus(long amount)
        {
            if (amount == 0L) { return this; }
            return new Moneypenny(checked(Amount + amount), Currency);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Moneypenny operator -(Moneypenny left, long right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Moneypenny operator -(long left, Moneypenny right)
            => new Moneypenny(checked(left - right.Amount), right.Currency);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Moneypenny operator -(Moneypenny left, Moneypenny right) => left.Minus(right);

        public Moneypenny Minus(Moneypenny other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0L) { return other.Negate(); }
            if (other.Amount == 0L) { return this; }
            return new Moneypenny(checked(other.Amount - Amount), Currency);
        }

        public Moneypenny Minus(long amount) => Plus(-amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Moneypenny operator *(long multiplier, Moneypenny penny) => penny.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Moneypenny operator *(Moneypenny penny, long multiplier) => penny.MultiplyBy(multiplier);

        public Moneypenny MultiplyBy(long multiplier) => new Moneypenny(checked(multiplier * Amount), Currency);
    }

    // Overrides the op_Division operator.
    public partial struct Moneypenny
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named DivideBy().")]
        public static Moneypenny operator /(Moneypenny dividend, long divisor) => dividend.DivideBy(divisor);

        public Moneypenny DivideBy(long divisor) => new Moneypenny(checked(Amount / divisor), Currency);
    }

    // Overrides the op_Modulus operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator %(Moneypenny dividend, long divisor) => dividend.Mod(divisor);

        public Moneypenny Mod(long divisor) => new Moneypenny(checked(Amount % divisor), Currency);
    }

    // Overrides the op_Increment operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator ++(Moneypenny penny) => penny.Increment();

        public Moneypenny Increment() => new Moneypenny(checked(Amount + 1L), Currency);
    }

    // Overrides the op_Decrement operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator --(Moneypenny penny) => penny.Decrement();

        public Moneypenny Decrement() => new Moneypenny(checked(Amount - 1L), Currency);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator -(Moneypenny penny) => penny.Negate();

        public Moneypenny Negate() => IsZero ? this : new Moneypenny(-Amount, Currency);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator +(Moneypenny penny) => penny.Plus();

        // This operator does nothing, only added for completeness.
        public Moneypenny Plus() => this;
    }
}
