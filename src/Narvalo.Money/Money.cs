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

    // Per default, the CLR will use LayoutKind.Sequential for structs. Here, we do not care
    // about interop with unmanaged code, so why not let the CLR decide what's best for it?
    //[StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money : Internal.IMoney<Money, Currency>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">The unsigned integer representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        [CLSCompliant(false)]
        public Money(uint amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">The unsigned long representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        [CLSCompliant(false)]
        public Money(ulong amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">The signed integer representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        public Money(int amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount without minor units.
        /// </summary>
        /// <param name="amount">The signed long representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        public Money(long amount, Currency currency) : this(amount, currency, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount where there is no restriction on the scale.
        /// </summary>
        /// <param name="amount">The decimal representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        public Money(decimal amount, Currency currency) : this(amount, currency, false) { }

        internal Money(decimal amount, Currency currency, bool normalized)
        {
            Amount = amount;
            Currency = currency;
            IsNormalized = currency.HasFixedDecimalPlaces ? normalized : true;
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
        /// specified by the currency. Always returns true if <see cref="IsRoundable"/> is false.
        /// <para>In plain english, returns true if we already store the best representation of
        /// the amount that we can get.</para>
        /// </summary>
        public bool IsNormalized { get; }

        /// <summary>
        /// Gets a value indicating whether the instance is "roundable".
        /// <para>An instance is said to be roundable if the currency specifies a standard
        /// representation for the number of decimal places allowed in an amount. If it does not,
        /// we never round the amount; customers of this class can still do this manually using one
        /// of the rounding methods found in <see cref="RoundingMachine"/>.</para>
        /// </summary>
        /// <remarks>It is highly unlikely that you will ever need to call this property,
        /// the library is supposed to do the right thing.</remarks>
        public bool IsRoundable => Currency.HasFixedDecimalPlaces;

        /// <summary>
        /// Gets a value indicating whether the amount is rounded to the number of decimal places
        /// specified by the currency. Always returns false if the instance is not roundable.
        /// </summary>
        public bool IsRounded => IsRoundable && IsNormalized;

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

        public int Sign => Amount < 0m ? -1 : (Amount > 0m ? 1 : 0);

        public Money Normalize(MidpointRounding mode)
        {
            if (IsNormalized) { return this; }
            return FromMajor(Amount, Currency, mode);
        }

        public Money Normalize(IRoundingAdjuster adjuster)
        {
            if (IsNormalized) { return this; }
            return FromMajor(Amount, Currency, adjuster);
        }

        internal void ThrowIfCurrencyMismatch(Money money, string parameterName)
            => Require.True(Currency == money.Currency, parameterName, Strings_Money.Argument_CurrencyMismatch);

        [ExcludeFromCodeCoverage]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "[Intentionally] Debugger-only code.")]
        private string DebuggerDisplay
            => "(" + Currency.Code + ") " + Amount.ToString("F2", CultureInfo.CurrentCulture)
            + "; IsNormalized=" + (IsNormalized ? "true" : "false");
    }

    // Factory methods: FromXXX() methods produce normalized instances, OfXXX() do not.
    public partial struct Money
    {
        public static Money Zero(Currency currency) => new Money(0, currency, true);

        public static Money Epsilon(Currency currency) => new Money(currency.Epsilon, currency, true);

        public static Money One(Currency currency) => new Money(currency.One, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount where there is no restriction on the scale.
        /// <para>Strictly equivalent to <see cref="Money(Decimal, Currency)"/>; only added
        /// for completeness.</para>
        /// </summary>
        /// <param name="major">The decimal representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        public static Money OfMajor(decimal major, Currency currency)
            => new Money(major, currency, false);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units where there is no restriction on the scale.
        /// </summary>
        /// <param name="minor">The decimal representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        public static Money OfMinor(decimal minor, Currency currency)
            => new Money(currency.ConvertToMajor(minor), currency, false);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount <c>ALREADY</c> rounded to the number of decimal places specified by the
        /// currency.
        /// </summary>
        /// <param name="major">The decimal representing the amount of money in major units.</param>
        /// <param name="currency">The currency.</param>
        public static Money FromMajor(decimal major, Currency currency)
            => new Money(major, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units.
        /// </summary>
        /// <param name="minor">The signed long representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        public static Money FromMinor(decimal minor, Currency currency)
            => new Money(currency.ConvertToMajor(minor), currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// <para>If the currency has no fixed decimal places, <paramref name="mode"/> is ignored
        /// and the amount is stored as it.</para>
        /// </summary>
        /// <param name="amount">The decimal representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="mode">The rounding mode.</param>
        public static Money FromMajor(decimal amount, Currency currency, MidpointRounding mode)
        {
            var major = currency.HasFixedDecimalPlaces
                 ? Math.Round(amount, currency.DecimalPlaces, mode)
                 : amount;
            return new Money(major, currency, true);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// <para>If the currency has no fixed decimal places, <paramref name="adjuster"/> is
        /// ignored and the amount is stored as it.</para>
        /// </summary>
        /// <param name="amount">A decimal representing the amount of money.</param>
        /// <param name="currency">The specific currency.</param>
        /// <param name="adjuster">The rounding adjuster.</param>
        public static Money FromMajor(decimal amount, Currency currency, IRoundingAdjuster adjuster)
        {
            Require.NotNull(adjuster, nameof(adjuster));
            decimal major = currency.HasFixedDecimalPlaces
                ? adjuster.Round(amount, currency.DecimalPlaces)
                : amount;
            return new Money(major, currency, true);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units for which the number of decimal places is
        /// determined by the currency.
        /// </summary>
        /// <param name="minor">The decimal representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="mode">The rounding mode.</param>
        public static Money FromMinor(decimal minor, Currency currency, MidpointRounding mode)
            => FromMajor(currency.ConvertToMajor(minor), currency, mode);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units for which the number of decimal places is
        /// determined by the currency.
        /// </summary>
        /// <param name="minor">The decimal representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="adjuster">The rounding adjuster.</param>
        public static Money FromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
            => FromMajor(currency.ConvertToMajor(minor), currency, adjuster);
    }

    // Domain-specific conversions.
    public partial struct Money
    {
        /// <summary>
        /// Gets the amount given in minor units.
        /// </summary>
        /// <exception cref="OverflowException">Thrown if the result is too large to fit into
        /// the Decimal range.</exception>
        /// <seealso cref="ToLongMinor()"/>
        /// <returns>The amount in minor units. If the instance is not normalizable, it returns
        /// the amount untouched as if the currency had no minor currency unit.</returns>
        public decimal ToMinor() => Currency.ConvertToMinor(Amount);

        /// <summary>
        /// Gets the amount given in minor units and converted to a 64-bit signed integer.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the instance is not normalized.</exception>
        /// <returns>A 64-bit signed integer representing the amount in minor units;
        /// null if the result is too large to fit into the Int64 range.</returns>
        public long? ToLongMinor()
        {
            if (!IsNormalized) { throw new InvalidOperationException("XXX"); }

            decimal minor = ToMinor();
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        public Money<TCurrency> ToMoney<TCurrency>() where TCurrency : Currency<TCurrency>
        {
            if (Currency.Code != Money<TCurrency>.UnderlyingUnit.Code)
            {
                throw new InvalidOperationException("XXX");
            }

            return new Money<TCurrency>(Amount, IsNormalized);
        }

        public Moneypenny? ToPenny()
        {
            if (!IsRounded) { throw new InvalidOperationException("XXX"); }

            long? amount = ToLongMinor();
            return amount.HasValue ? new Moneypenny(amount.Value, Currency) : (Moneypenny?)null;
        }
    }

    // Numeric conversions.
    public partial struct Money
    {
        [CLSCompliant(false)]
        public sbyte ToSByte() => Decimal.ToSByte(Amount);

        [CLSCompliant(false)]
        public ushort ToUInt16() => Decimal.ToUInt16(Amount);

        [CLSCompliant(false)]
        public uint ToUInt32() => Decimal.ToUInt32(Amount);

        [CLSCompliant(false)]
        public ulong ToUInt64() => Decimal.ToUInt64(Amount);

        public byte ToByte() => Decimal.ToByte(Amount);

        public short ToInt16() => Decimal.ToInt16(Amount);

        public int ToInt32() => Decimal.ToInt32(Amount);

        public long ToInt64() => Decimal.ToInt64(Amount);

        // Lossless conversion.
        public decimal ToDecimal() => Amount;

        // NB: Implicit conversion (no unexpected loss of precision, fast & completely harmless).
        public static implicit operator decimal(Money value) => value.Amount;

        #region Harmless explicit conversions from a numeric type to Money w/o Currency.

        [CLSCompliant(false)]
        public static explicit operator Money(sbyte value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static explicit operator Money(ushort value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static explicit operator Money(uint value) => new Money(value, Currency.None);

        [CLSCompliant(false)]
        public static explicit operator Money(ulong value) => new Money(value, Currency.None);

        public static explicit operator Money(byte value) => new Money(value, Currency.None);

        public static explicit operator Money(short value) => new Money(value, Currency.None);

        public static explicit operator Money(int value) => new Money(value, Currency.None);

        public static explicit operator Money(long value) => new Money(value, Currency.None);

        // WARNING: Even if we made the other ops implicit, we should keep this one explicit:
        // the cast is lossless but the result is not normalized.
        public static explicit operator Money(decimal value) => new Money(value, Currency.None, false);

        #endregion
    }

    // Implements the IFormattable interface.
    public partial struct Money
    {
        public override string ToString() => FormatImpl(null, NumberFormatInfo.CurrentInfo);

        public string ToString(string format) => FormatImpl(format, NumberFormatInfo.CurrentInfo);

        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                object fmt = formatProvider.GetFormat(typeof(Money));
                if (fmt is ICustomFormatter fmtr) { return fmtr.Format(format, this, formatProvider); }
            }

            return FormatImpl(format, NumberFormatInfo.GetInstance(formatProvider));
        }

        private string FormatImpl(string format, NumberFormatInfo info)
        {
            Debug.Assert(info != null);

            var spec = MoneyFormat.Parse(format, Currency.FixedDecimalPlaces);

            return MoneyFormatter.FormatMoney(spec, Amount, Currency.Code, info);
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

        public override bool Equals(object obj) => (obj is Money) && Equals((Money)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(
                Amount.GetHashCode(),
                Currency.GetHashCode(),
                IsNormalized.GetHashCode());
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
                throw new ArgumentException(Strings_Money.Argument_InvalidMoneyType, nameof(obj));
            }

            return CompareTo((Money)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, uint right) => left.Plus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(uint left, Money right) => right.Plus(left);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, ulong right) => left.Plus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(ulong left, Money right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, int right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(int left, Money right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, long right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(long left, Money right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, decimal right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(decimal left, Money right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money operator +(Money left, Money right) => left.Plus(right);

        public Money Plus(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0m) { return other; }
            if (other.Amount == 0m) { return this; }
            return new Money(Amount + other.Amount, Currency, IsNormalized && other.IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Plus(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Plus(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Plus(int amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Plus(long amount)
        {
            if (amount == 0L) { return this; }
            return new Money(Amount + amount, Currency, IsNormalized);
        }

        public Money Plus(decimal amount)
        {
            if (amount == 0m) { return this; }
            return new Money(Amount + amount, Currency, false);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, uint right) => left.Minus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money operator -(uint left, Money right)
            => new Money(left - right.Amount, right.Currency, right.IsNormalized);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, ulong right) => left.Minus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money operator -(ulong left, Money right)
            => new Money(left - right.Amount, right.Currency, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, long right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money operator -(long left, Money right)
            => new Money(left - right.Amount, right.Currency, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, int right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money operator -(int left, Money right)
            => new Money(left - right.Amount, right.Currency, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, decimal right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money operator -(decimal left, Money right)
        {
            if (left == 0m) { return right.Negate(); }
            return new Money(left - right.Amount, right.Currency, false);
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money operator -(Money left, Money right) => left.Minus(right);

        public Money Minus(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            if (Amount == 0m) { return other.Negate(); }
            if (other.Amount == 0m) { return this; }
            return new Money(Amount - other.Amount, Currency, IsNormalized && other.IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Minus(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money(Amount - amount, Currency, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money Minus(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money(Amount - amount, Currency, IsNormalized);
        }

        public Money Minus(int amount) => Plus(-amount);

        public Money Minus(long amount) => Plus(-amount);

        public Money Minus(decimal amount) => Plus(-amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(ulong multiplier, Money money) => money.MultiplyBy(multiplier);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(Money money, ulong multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(long multiplier, Money money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(Money money, long multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(int multiplier, Money money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(Money money, int multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(decimal multiplier, Money money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money operator *(Money money, decimal multiplier) => money.MultiplyBy(multiplier);

        [CLSCompliant(false)]
        public Money MultiplyBy(uint multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        [CLSCompliant(false)]
        public Money MultiplyBy(ulong multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money MultiplyBy(int multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money MultiplyBy(long multiplier) => new Money(multiplier * Amount, Currency, IsNormalized);

        public Money MultiplyBy(decimal multiplier) => new Money(multiplier * Amount, Currency, false);
    }

    // Overrides the op_Division operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named DivideBy().")]
        public static Money operator /(Money dividend, decimal divisor) => dividend.DivideBy(divisor);

        public Money DivideBy(decimal divisor) => new Money(Amount / divisor, Currency, false);
    }

    // Overrides the op_Modulus operator.
    public partial struct Money
    {
        public static Money operator %(Money dividend, decimal divisor) => dividend.Mod(divisor);

        public Money Mod(decimal divisor) => new Money(Amount % divisor, Currency, false);
    }

    // Overrides the op_Increment operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named IncrementMajor().")]
        public static Money operator ++(Money money) => money.IncrementMajor();

        public Money IncrementMajor() => new Money(Amount + Currency.One, Currency, IsNormalized);

        // For currencies without minor units, this is equivalent to Increment().
        public Money IncrementMinor() => new Money(Amount + Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_Decrement operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named DecrementMajor().")]
        public static Money operator --(Money money) => money.DecrementMajor();

        public Money DecrementMajor() => new Money(Amount - Currency.One, Currency, IsNormalized);

        // For currencies without minor units, this is equivalent to Decrement().
        public Money DecrementMinor() => new Money(Amount - Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Money
    {
        public static Money operator -(Money money) => money.Negate();

        public Money Negate() => IsZero ? this : new Money(-Amount, Currency, IsNormalized);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Money
    {
        public static Money operator +(Money money) => money.Plus();

        // This operator does nothing, only added for completeness.
        public Money Plus() => this;
    }
}
