// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Numerics;
    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    // Per default, the CLR will use LayoutKind.Sequential for structs. Here, we do not care
    // about interop with unmanaged code, so why not let the CLR decide what's best for it?
    //[StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money : IEquatable<Money>, IComparable<Money>, IComparable, IFormattable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">An unsigned integer representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        [CLSCompliant(false)]
        public Money(uint amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">A signed integer representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        [CLSCompliant(false)]
        public Money(ulong amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">An unsigned long representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        public Money(int amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">An signed long representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        public Money(long amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount where there is no restriction on the scale.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        public Money(decimal amount, Currency currency) : this(amount, currency, false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        /// <param name="mode">The rounding mode.</param>
        public Money(decimal amount, Currency currency, MidpointRounding mode)
        {
            Amount = Math.Round(amount, currency.DecimalPlaces, mode);
            Currency = currency;
            IsNormalized = true;
        }

        internal Money(decimal amount, Currency currency, bool normalized)
        {
            Amount = amount;
            Currency = currency;
            IsNormalized = normalized;
        }

        /// <summary>
        /// Gets the amount of money.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// Gets a value indicating whether the amount is rounded to the number of decimal places
        /// specified by the currency.
        /// </summary>
        public bool IsNormalized { get; }

        /// <summary>
        /// Gets the amount expressed in minor units.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the instance is not
        /// normalized.</exception>
        /// <exception cref="OverflowException">Thrown if the amount is too large to fit into
        /// the Int64 range.</exception>
        /// <seealso cref="ToMinor(Money)"/>
        /// <seealso cref="ToMinor(Money, out long)"/>
        public long AmountInMinor
        {
            get
            {
                if (!IsNormalized)
                {
                    throw new InvalidOperationException("XXX");
                }

                return Convert.ToInt64(Currency.Factor * Amount);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the amount is zero.
        /// </summary>
        public bool IsZero => Amount == 0m;

        /// <summary>
        /// Gets a value indicating whether the amount is lower than zero.
        /// </summary>
        public bool IsNegative => Amount < 0m;

        /// <summary>
        /// Gets a value indicating whether the amount is lower than or equal to zero.
        /// </summary>
        public bool IsNegativeOrZero => Amount <= 0m;

        /// <summary>
        /// Gets a value indicating whether the amount is greater than zero.
        /// </summary>
        public bool IsPositive => Amount > 0m;

        /// <summary>
        /// Gets a value indicating whether the amount is greater than or equal to zero.
        /// </summary>
        public bool IsPositiveOrZero => Amount >= 0m;

        public static Money Zero(Currency currency) => new Money(0, currency, true);

        public static Money Epsilon(Currency currency) => new Money(currency.Epsilon, currency, true);

        public static Money One(Currency currency) => new Money(currency.One, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount already rounded to the number of decimal places specified by the currency.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">A currency.</param>
        public static Money OfMajor(decimal amount, Currency currency)
            => new Money(amount, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount expressed in minor units.
        /// </summary>
        /// <param name="minor">The amount of money in minor units.</param>
        /// <param name="currency">A currency.</param>
        public static Money OfMinor(long minor, Currency currency)
            => new Money(currency.Epsilon * minor, currency, true);

        public long? ToMinor()
        {
            if (!IsNormalized) { return null; }

            decimal minor = Currency.Factor * Amount;
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "[Intentionally] Standard Try... pattern.")]
        public bool ToMinor(out long result)
        {
            long? minor = ToMinor();
            result = minor ?? (Amount > 0 ? Int64.MaxValue : Int64.MinValue);
            return minor.HasValue;
        }

        public Money Normalize(MidpointRounding mode)
        {
            if (IsNormalized) { return this; }
            return new Money(Amount, Currency, mode);
        }

        internal void ThrowIfCurrencyMismatch(Money money, string parameterName)
            => Enforce.True(Currency == money.Currency, parameterName, Strings.Argument_CurrencyMismatch);

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => Format.Current("{0:F2} ({1})", Amount, Currency.Code);
    }

    // Implements the IFormattable interface.
    public partial struct Money
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.Format(this, null, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.Format(this, format, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(IFormatProvider formatProvider)
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.Format(this, null, NumberFormatInfo.GetInstance(formatProvider));
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

            return MoneyFormatter.Format(this, format, NumberFormatInfo.GetInstance(formatProvider));
        }
    }

    // Implements the IEquatable<Money> interface.
    public partial struct Money
    {
        public static bool operator ==(Money left, Money right) => left.Equals(right);

        public static bool operator !=(Money left, Money right) => !left.Equals(right);

        public bool Equals(Money other)
            => Amount == other.Amount
            && Currency == other.Currency
            && IsNormalized == other.IsNormalized;

        public override bool Equals(object obj)
        {
            if (!(obj is Money)) { return false; }

            return Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Amount.GetHashCode();
                hash = 31 * hash + Currency.GetHashCode();
                hash = 31 * hash + IsNormalized.GetHashCode();
                return hash;
            }
        }
    }

    // Implements the IComparable and IComparable<Money> interfaces.
    public partial struct Money
    {
        public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;
        public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;
        public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;
        public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

        public int CompareTo(Money other)
        {
            this.ThrowIfCurrencyMismatch(other, nameof(other));

            return Amount.CompareTo(other.Amount);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is Money))
            {
                throw new ArgumentException(Strings.Argument_InvalidMoneyType, nameof(obj));
            }

            return CompareTo((Money)obj);
        }
    }

    // Conversions.
    public partial struct Money
    {
        [CLSCompliant(false)]
        public static sbyte ToSByte(Money money) => Decimal.ToSByte(money.Amount);

        [CLSCompliant(false)]
        public static ushort ToUInt16(Money money) => Decimal.ToUInt16(money.Amount);

        [CLSCompliant(false)]
        public static uint ToUInt32(Money money) => Decimal.ToUInt32(money.Amount);

        [CLSCompliant(false)]
        public static ulong ToUInt64(Money money) => Decimal.ToUInt64(money.Amount);

        public static byte ToByte(Money money) => Decimal.ToByte(money.Amount);

        public static short ToInt16(Money money) => Decimal.ToInt16(money.Amount);

        public static int ToInt32(Money money) => Decimal.ToInt32(money.Amount);

        public static long ToInt64(Money money) => Decimal.ToInt64(money.Amount);

        public static decimal ToDecimal(Money money) => money.Amount;

        #region Integral type or decimal -> Money.

        [CLSCompliant(false)]
        public static implicit operator Money(sbyte value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static implicit operator Money(ushort value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static implicit operator Money(uint value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static implicit operator Money(ulong value) => new Money(value, Currency.None);

        public static implicit operator Money(byte value) => new Money(value, Currency.None);

        public static implicit operator Money(short value) => new Money(value, Currency.None);

        public static implicit operator Money(int value) => new Money(value, Currency.None);

        public static implicit operator Money(long value) => new Money(value, Currency.None);

        // NB: This one is explicit (the result is not normalized).
        public static explicit operator Money(decimal value) => new Money(value, Currency.None, false);

        #endregion

        #region Money -> integral type or decimal.

        [CLSCompliant(false)]
        public static explicit operator sbyte(Money value) => ToSByte(value);

        [CLSCompliant(false)]
        public static explicit operator ushort(Money value) => ToUInt16(value);

        [CLSCompliant(false)]
        public static explicit operator uint(Money value) => ToUInt32(value);

        [CLSCompliant(false)]
        public static explicit operator ulong(Money value) => ToUInt64(value);

        public static explicit operator byte(Money value) => ToByte(value);

        public static explicit operator short(Money value) => ToInt16(value);

        public static explicit operator int(Money value) => ToInt32(value);

        public static explicit operator long(Money value) => ToInt64(value);

        // NB: This one is implicit (no unexpected loss of precision).
        public static implicit operator decimal(Money value) => ToDecimal(value);

        #endregion
    }

    // Overrides the op_Addition operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        public static Money operator +(Money left, uint right) => left.Add(right);
        [CLSCompliant(false)]
        public static Money operator +(uint left, Money right) => right.Add(left);
        [CLSCompliant(false)]
        public static Money operator +(Money left, ulong right) => left.Add(right);
        [CLSCompliant(false)]
        public static Money operator +(ulong left, Money right) => right.Add(left);
        public static Money operator +(Money left, int right) => left.Add(right);
        public static Money operator +(int left, Money right) => right.Add(left);
        public static Money operator +(Money left, long right) => left.Add(right);
        public static Money operator +(long left, Money right) => right.Add(left);
        public static Money operator +(Money left, decimal right) => left.Add(right);
        public static Money operator +(decimal left, Money right) => right.Add(left);
        public static Money operator +(Money left, Money right) => left.Add(right);

        [CLSCompliant(false)]
        public Money Add(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Add(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Add(int amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Add(long amount)
        {
            if (amount == 0L) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Add(decimal amount)
        {
            if (amount == 0m) { return this; }
            return new Money(Amount + amount, Currency, false);
        }

        public Money Add(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0m) { return other; }
            if (other.Amount == 0m) { return this; }
            return new Money(Amount + other.Amount, Currency, IsNormalized && other.IsNormalized);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        public static Money operator -(Money left, uint right) => left.Subtract(right);
        [CLSCompliant(false)]
        public static Money operator -(uint left, Money right) => right.SubtractLeft(left);
        [CLSCompliant(false)]
        public static Money operator -(Money left, ulong right) => left.Subtract(right);
        [CLSCompliant(false)]
        public static Money operator -(ulong left, Money right) => right.SubtractLeft(left);
        public static Money operator -(Money left, long right) => left.Subtract(right);
        public static Money operator -(long left, Money right) => right.SubtractLeft(left);
        public static Money operator -(Money left, int right) => left.Subtract(right);
        public static Money operator -(int left, Money right) => right.SubtractLeft(left);
        public static Money operator -(Money left, decimal right) => left.Subtract(right);
        public static Money operator -(decimal left, Money right) => right.SubtractLeft(left);
        public static Money operator -(Money left, Money right) => left.Subtract(right);

        [CLSCompliant(false)]
        public Money Subtract(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount - amount, Currency, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Subtract(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money(Amount - amount, Currency, IsNormalized);
        }

        public Money Subtract(int amount) => Add(-amount);

        public Money Subtract(long amount) => Add(-amount);

        public Money Subtract(decimal amount) => Add(-amount);

        public Money Subtract(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0m) { return other.Negate(); }
            if (other.Amount == 0m) { return this; }
            return new Money(other.Amount - Amount, Currency, IsNormalized && other.IsNormalized);
        }

        private Money SubtractLeft(uint amount) => new Money(amount - Amount, Currency, IsNormalized);

        private Money SubtractLeft(ulong amount) => new Money(amount - Amount, Currency, IsNormalized);

        private Money SubtractLeft(int amount) => new Money(amount - Amount, Currency, IsNormalized);

        private Money SubtractLeft(long amount) => new Money(amount - Amount, Currency, IsNormalized);

        private Money SubtractLeft(decimal amount)
        {
            if (amount == 0m) { return Negate(); }
            return new Money(amount - Amount, Currency, false);
        }
    }

    // Overrides the op_Multiply operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        public static Money operator *(ulong multiplier, Money money) => money.Multiply(multiplier);
        [CLSCompliant(false)]
        public static Money operator *(Money money, ulong multiplier) => money.Multiply(multiplier);
        public static Money operator *(long multiplier, Money money) => money.Multiply(multiplier);
        public static Money operator *(Money money, long multiplier) => money.Multiply(multiplier);
        public static Money operator *(int multiplier, Money money) => money.Multiply(multiplier);
        public static Money operator *(Money money, int multiplier) => money.Multiply(multiplier);
        public static Money operator *(decimal multiplier, Money money) => money.Multiply(multiplier);
        public static Money operator *(Money money, decimal multiplier) => money.Multiply(multiplier);

        [CLSCompliant(false)]
        public Money Multiply(uint multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        [CLSCompliant(false)]
        public Money Multiply(ulong multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money Multiply(int multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money Multiply(long multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money Multiply(decimal multiplier) => new Money(multiplier * Amount, Currency, false);
    }

    // Overrides the op_Division operator.
    public partial struct Money
    {
        // WARNING: This method returns a decimal (a division implies that we lost the currency unit).
        public static decimal operator /(Money dividend, Money divisor) => dividend.Divide(divisor);
        public static Money operator /(Money dividend, decimal divisor) => dividend.Divide(divisor);

        // WARNING: This method returns a decimal (a division implies that we lost the currency unit).
        public decimal Divide(Money divisor) => Amount / divisor.Amount;

        public Money Divide(decimal divisor) => new Money(Amount / divisor, Currency, false);
    }

    // Overrides the op_Modulus operator.
    public partial struct Money
    {
        public static Money operator %(Money dividend, decimal divisor) => dividend.Remainder(divisor);

        public Money Remainder(decimal divisor) => new Money(Amount % divisor, Currency, false);
    }

    // Overrides the op_Increment operator.
    public partial struct Money
    {
        public static Money operator ++(Money money) => money.Increment();

        public Money Increment() => new Money(Amount + Currency.One, Currency, IsNormalized);

        public Money IncrementMinor() => new Money(Amount + Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_Decrement operator.
    public partial struct Money
    {
        public static Money operator --(Money money) => money.Decrement();

        public Money Decrement() => new Money(Amount - Currency.One, Currency, IsNormalized);

        public Money DecrementMinor() => new Money(Amount - Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Money
    {
        public static Money operator -(Money money) => money.Negate();

        public Money Negate() => IsZero ? this : new Money(-Amount, Currency, IsNormalized);
    }

    // Overrides the op_UnaryPlus operator.
    // This operator does nothing, only added for completeness.
    public partial struct Money
    {
        public static Money operator +(Money money) => money;

        public Money Plus() => this;
    }
}
