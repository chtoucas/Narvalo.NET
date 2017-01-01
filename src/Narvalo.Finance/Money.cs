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

    // Per default, the CLR will use LayoutKind.Sequential for structs. Here, we do not care
    // about interop with unmanaged code, so why not let the CLR decide what's best for it?
    // Disabled: http://stackoverflow.com/questions/21881554/why-does-the-system-datetime-struct-have-layout-kind-auto?rq=1
    //[StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money : IEquatable<Money>, IComparable<Money>, IComparable, IFormattable
    {
        private readonly _Decimal _decimal;

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class without any currency attached.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        public Money(decimal amount) : this(amount, Currency.None) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which no rounding is done.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">The specific currency.</param>
        public Money(decimal amount, Currency currency)
        {
            _decimal = new _Decimal(amount);
            Currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">The specific currency.</param>
        /// <param name="rounding">The rounding mode.</param>
        public Money(decimal amount, Currency currency, MidpointRounding rounding)
        {
            _decimal = new _Decimal(amount, currency.DecimalPlaces, rounding);
            Currency = currency;
        }

        public decimal Amount => _decimal.Value;

        public Currency Currency { get; }

        public bool Normalized => _decimal.HasFixedScale;

        public Money Normalize() => Normalize(MidpointRounding.ToEven);

        public Money Normalize(MidpointRounding rounding)
        {
            if (Normalized) { return this; }

            return new Money(Amount, Currency, rounding);
        }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay => Format.Invariant("{0:F2} ({1})", Amount, Currency.Code);

        // Check that the currencies match.
        private void ThrowIfCurrencyMismatch(Money other, string parameterName)
            => Enforce.True(Currency != other.Currency, parameterName, Strings.Argument_CurrencyMismatch);
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

        public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;

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
            ThrowIfCurrencyMismatch(other, nameof(other));

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
        public static byte ToByte(Money value) => Decimal.ToByte(value.Amount);

        [CLSCompliant(false)]
        public static sbyte ToSByte(Money value) => Decimal.ToSByte(value.Amount);

        [CLSCompliant(false)]
        public static ushort ToUInt16(Money value) => Decimal.ToUInt16(value.Amount);

        public static short ToInt16(Money value) => Decimal.ToInt16(value.Amount);

        [CLSCompliant(false)]
        public static uint ToUInt32(Money value) => Decimal.ToUInt32(value.Amount);

        public static int ToInt32(Money value) => Decimal.ToInt32(value.Amount);

        [CLSCompliant(false)]
        public static ulong ToUInt64(Money value) => Decimal.ToUInt64(value.Amount);

        public static long ToInt64(Money value) => Decimal.ToInt64(value.Amount);

        public static decimal ToDecimal(Money value) => value.Amount;

        #region Integral type or decimal -> Money.

        public static implicit operator Money(byte value) => new Money(value);

        [CLSCompliant(false)]
        public static implicit operator Money(sbyte value) => new Money(value);

        [CLSCompliant(false)]
        public static implicit operator Money(ushort value) => new Money(value);

        public static implicit operator Money(short value) => new Money(value);

        [CLSCompliant(false)]
        public static implicit operator Money(uint value) => new Money(value);

        public static implicit operator Money(int value) => new Money(value);

        [CLSCompliant(false)]
        public static implicit operator Money(ulong value) => new Money(value);

        public static implicit operator Money(long value) => new Money(value);

        public static implicit operator Money(decimal value) => new Money(value);

        #endregion

        #region Money -> integral type or decimal.

        public static explicit operator byte(Money value) => ToByte(value);

        [CLSCompliant(false)]
        public static explicit operator sbyte(Money value) => ToSByte(value);

        [CLSCompliant(false)]
        public static explicit operator ushort(Money value) => ToUInt16(value);

        public static explicit operator short(Money value) => ToInt16(value);

        [CLSCompliant(false)]
        public static explicit operator uint(Money value) => ToUInt32(value);

        public static explicit operator int(Money value) => ToInt32(value);

        [CLSCompliant(false)]
        public static explicit operator ulong(Money value) => ToUInt64(value);

        public static explicit operator long(Money value) => ToInt64(value);

        // NB: This one is implicit (no loss of precision).
        public static implicit operator decimal(Money value) => ToDecimal(value);

        #endregion
    }

    // Overrides the op_Addition operator.
    public partial struct Money
    {
        public static Money operator +(Money left, Money right) => left.Add(right);

        public Money Add(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Add(decimal amount) => new Money(Amount + amount, Currency);
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money
    {
        public static Money operator -(Money left, Money right) => left.Subtract(right);

        public Money Subtract(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Subtract(decimal amount) => new Money(Amount - amount, Currency);
    }

    // Overrides the op_Multiply operator.
    public partial struct Money
    {
        public static Money operator *(decimal multiplier, Money money) => money.Multiply(multiplier);

        public static Money operator *(Money money, decimal multiplier) => money.Multiply(multiplier);

        public Money Multiply(decimal multiplier) => new Money(multiplier * Amount, Currency);
    }

    // Overrides the op_Division operator.
    public partial struct Money
    {
        public static Money operator /(Money money, decimal divisor)
        {
            Expect.True(divisor != 0m);

            return money.Divide(divisor);
        }

        public Money Divide(decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money(Amount / divisor, Currency);
        }
    }

    // Overrides the op_Modulus operator.
    public partial struct Money
    {
        public static Money operator %(Money money, decimal divisor)
        {
            Expect.True(divisor != 0m);

            return money.Remainder(divisor);
        }

        public Money Remainder(decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money(Amount % divisor, Currency);
        }
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Money
    {
        public static Money operator -(Money money) => money.Negate();

        public Money Negate() => new Money(-Amount, Currency);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Money
    {
        public static Money operator +(Money money) => money;

        public Money Plus() => this;
    }
}
