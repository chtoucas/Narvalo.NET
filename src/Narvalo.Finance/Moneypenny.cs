﻿// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    // A lightweight money type where the amount is stored in minor units.
    //
    // Despite the name, this type is not restricted to currencies with minor units of size 2.
    // Nevertheless, we assume that a penny represents one minor unit exactly (as found in
    // ISO 4217); we do not handle arbitrary subunits.
    //
    // Advantages:
    // - Using an Int64 as the backing type for the amount allows for fast arithmetic operations.
    // - Rounding is no longer needed; the amount is always normalized.
    //   See below for caveats when performing a division.
    // Disadvantages:
    // - Only available for currencies specifying a fixed number of decimal places, ie it does not
    //   support legacy ISO currencies.
    // - The Int64 range is smaller. This has two consequences:
    //   * more opportunities to throw an overflow exception.
    //   * some operations might be lossful:
    //     - OfMajor() and OfMinor() cast a decimal to a long which is a lossful operation.
    //     - Divide(long) is actually an integer division, it then rounds toward zero if needed;
    //       this is necessary to keep the operation closed. If you do not want this, you should
    //       use Divide(decimal) or DivRem() instead.
    // - Multiply(long), Divide(long) and Remainder(long) are closed but obviously not very useful.
    //   We provide decimal overloads but, for that, we no longer return a Moneypenny object.
    // - We do not provide any support for anything besides the basic arithmetic operations,
    //   but you can still convert a Moneypenny object to a Money object.
    // Remark: This class and FastMoney from JavaMoney are similar in purpose (fast operations)
    // but different in the way they deal with amounts. Here, we only consider strict amounts
    // (no rounding is ever needed, at the expense of what you can do with it), a restriction
    // that does not exist with FastMoney.
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

        public bool IsZero => Amount == 0L;
        public bool IsNegative => Amount < 0L;
        public bool IsNegativeOrZero => Amount <= 0L;
        public bool IsPositive => Amount > 0L;
        public bool IsPositiveOrZero => Amount >= 0L;

        public decimal ToMajor() => Currency.ConvertToMajor(Amount);

        private void ThrowIfCurrencyMismatch(Moneypenny penny, string parameterName)
            => Enforce.True(Currency == penny.Currency, parameterName, Strings.Argument_CurrencyMismatch);
    }

    // Static factory methods.
    public partial struct Moneypenny
    {
        public static Moneypenny Zero(Currency currency) => new Moneypenny(0L, currency);

        public static Moneypenny One(Currency currency) => new Moneypenny(1L, currency);

        // DANGEROUS ZONE when major is not normalized.
        public static Moneypenny? OfMajor(decimal major, Currency currency)
        {
            decimal minor = currency.ConvertToMinor(major);
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return new Moneypenny(Convert.ToInt64(minor), currency);
        }

        // DANGEROUS ZONE when minor is not normalized.
        public static Moneypenny? OfMinor(decimal minor, Currency currency)
        {
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return new Moneypenny(Convert.ToInt64(minor), currency);
        }
    }

    // Implements the IFormattable interface.
    public partial struct Moneypenny
    {
        public override string ToString()
        {
            Warrant.NotNull<string>();
            return ToString(DEFAULT_FORMAT, null);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return ToString(format, null);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return ToString(DEFAULT_FORMAT, formatProvider);
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

            throw new NotImplementedException();
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
        public static Moneypenny FromMoney(Money money)
        {
            long? amount = money.ToLongMinor();
            if (!amount.HasValue) { throw new InvalidOperationException("XXX"); }

            return new Moneypenny(amount.Value, money.Currency);
        }

        public Money ToMoney() => Money.OfMinor(Amount, Currency);

        public static explicit operator Moneypenny(long value) => new Moneypenny(value, Currency.None);

        public static explicit operator Moneypenny(Money value) => FromMoney(value);

        public static implicit operator long(Moneypenny value) => value.Amount;

        public static explicit operator Money(Moneypenny value) => value.ToMoney();
    }

    // Math operators.
    public partial struct Moneypenny
    {
        public Moneypenny Abs() => IsPositiveOrZero ? this : Negate();

        public int Sign() => Amount < 0L ? -1 : (Amount > 0L ? 1 : 0);

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "[Intentionally] Math.DivRem().")]
        public Moneypenny DivRem(long divisor, out Moneypenny remainder)
        {
            long rem;
            long q = Integer.DivRem(Amount, divisor, out rem);
            remainder = new Moneypenny(rem, Currency);
            return new Moneypenny(q, Currency);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator +(Moneypenny left, long right) => left.Add(right);
        public static Moneypenny operator +(long left, Moneypenny right) => right.Add(left);
        public static Moneypenny operator +(Moneypenny left, Moneypenny right) => left.Add(right);

        public Moneypenny Add(long amount)
        {
            if (amount == 0L) { return this; }
            return new Moneypenny(checked(Amount + amount), Currency);
        }

        public Moneypenny Add(Moneypenny other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0L) { return other; }
            if (other.Amount == 0L) { return this; }
            return new Moneypenny(checked(Amount + other.Amount), Currency);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator -(Moneypenny left, long right) => left.Subtract(right);
        public static Moneypenny operator -(long left, Moneypenny right) => right.SubtractLeft(left);
        public static Moneypenny operator -(Moneypenny left, Moneypenny right) => left.Subtract(right);

        public Moneypenny Subtract(long amount) => Add(-amount);

        public Moneypenny Subtract(Moneypenny other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0L) { return other.Negate(); }
            if (other.Amount == 0L) { return this; }
            return new Moneypenny(checked(other.Amount - Amount), Currency);
        }

        private Moneypenny SubtractLeft(long amount) => new Moneypenny(checked(amount - Amount), Currency);
    }

    // Overrides the op_Multiply operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator *(long multiplier, Moneypenny money) => money.Multiply(multiplier);
        public static Moneypenny operator *(Moneypenny money, long multiplier) => money.Multiply(multiplier);
        //public static Money operator *(decimal multiplier, Moneypenny money) => money.Multiply(multiplier);
        //public static Money operator *(Moneypenny money, decimal multiplier) => money.Multiply(multiplier);

        public Moneypenny Multiply(long multiplier) => new Moneypenny(checked(multiplier * Amount), Currency);

        // NB: This operation is not closed: Moneypenny -> Money.
        // NB: Decimal multiplication is always checked.
        public Money Multiply(decimal multiplier) => new Money(multiplier * Amount, Currency);
    }

    // Overrides the op_Division operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator /(Moneypenny dividend, long divisor) => dividend.Divide(divisor);
        //public static decimal operator /(Moneypenny dividend, Moneypenny divisor) => dividend.Divide(divisor);
        //public static Money operator /(Moneypenny dividend, decimal divisor) => dividend.Divide(divisor);

        public Moneypenny Divide(long divisor) => new Moneypenny(checked(Amount / divisor), Currency);

        // NB: This method returns a decimal (a division implies that we lost the currency unit).
        // NB: Decimal division is always checked.
        public decimal Divide(Moneypenny divisor) => Amount / (decimal)divisor.Amount;

        // NB: This operation is not closed: Moneypenny -> Money.
        // NB: Decimal division is always checked.
        public Money Divide(decimal divisor) => new Money(Amount / divisor, Currency);
    }

    // Overrides the op_Modulus operator.
    public partial struct Moneypenny
    {
        public static Moneypenny operator %(Moneypenny dividend, long divisor) => dividend.Remainder(divisor);
        //public static Money operator %(Moneypenny dividend, decimal divisor) => dividend.Remainder(divisor);

        public Moneypenny Remainder(long divisor) => new Moneypenny(checked(Amount % divisor), Currency);

        // NB: This operation is not closed: Moneypenny -> Money.
        // NB: Decimal remainder is always checked.
        public Money Remainder(decimal divisor) => new Money(Amount % divisor, Currency);
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
    // This operator does nothing, only added for completeness.
    public partial struct Moneypenny
    {
        public static Moneypenny operator +(Moneypenny penny) => penny;

        public Moneypenny Plus() => this;
    }
}