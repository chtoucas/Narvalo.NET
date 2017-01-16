// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Properties;

    using static Narvalo.Finance.MoneyCalculator;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount for which the number of decimal places is determined by the currency.
        /// <para>If the currency has no fixed decimal places, <paramref name="mode"/> is ignored
        /// and the amount is stored as it.</para>
        /// </summary>
        /// <param name="amount">The decimal representing the amount of money.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="mode">The rounding mode.</param>
        public Money(decimal amount, Currency currency, MidpointRounding mode)
        {
            Amount = currency.HasFixedDecimalPlaces
                ? Math.Round(amount, currency.DecimalPlaces, mode)
                : amount;
            Currency = currency;
            IsNormalized = true;
        }

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
        /// of the rounding methods found in <see cref="MoneyCalculator"/>.</para>
        /// </summary>
        /// <remarks>It is highly unlikely that you will ever need to call this property,
        /// the library is supposed to do the right thing.</remarks>
        public bool IsRoundable => Currency.HasFixedDecimalPlaces;

        /// <summary>
        /// Gets a value indicating whether the amount is rounded.
        /// Always returns false if the instance is not roundable.
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

        /// <summary>
        /// Gets the amount given in minor units.
        /// </summary>
        /// <exception cref="OverflowException">Thrown if the result is too large to fit into
        /// the Decimal range.</exception>
        /// <seealso cref="ToLongMinor()"/>
        /// <seealso cref="ToLongMinor(out long)"/>
        /// <returns>The amount in minor units. If the instance is not normalizable, it returns
        /// the amount untouched as if the currency had no minor currency unit.</returns>
        public decimal ToMinor() => Currency.ConvertToMinor(Amount);

        /// <summary>
        /// Gets the amount given in minor units and converted to a 64-bit signed integer.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the instance is not normalized.</exception>
        /// <returns>A 64-bit signed integer representing the amount in minor units;
        /// <see langword="null"/> if the result is too large to fit into the Int64 range.</returns>
        public long? ToLongMinor()
        {
            if (!IsNormalized) { throw new InvalidOperationException("XXX"); }

            decimal minor = ToMinor();
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        /// <summary>
        /// Gets the amount given in minor units and converted to a 64-bit signed integer.
        /// </summary>
        /// <param name="result">If the conversion succeeded, contains a 64-bit signed integer
        /// representing the amount in minor units, or Int64.MaxValue when the conversion failed
        /// and <see cref="Amount"/> is positive, otherwise Int64.MinValue.</param>
        /// <exception cref="InvalidOperationException">Thrown if the instance is not normalized.</exception>
        /// <returns><see langword="true"/> if the amount was converted successfully; otherwise,
        /// <see langword="false"/>.</returns>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "[Intentionally] Standard Try... pattern.")]
        public bool ToLongMinor(out long result)
        {
            long? minor = ToLongMinor();
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
        private string DebuggerDisplay
            => Format.Current("{0} {1:F}; IsNormalized={2})", Currency.Code, Amount, IsRoundable ? "true" : "false");
    }

    // Static factory methods.
    public partial struct Money
    {
        public static Money Zero(Currency currency) => new Money(0, currency, true);

        public static Money Epsilon(Currency currency) => new Money(currency.Epsilon, currency, true);

        public static Money One(Currency currency) => new Money(currency.One, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount <c>already</c> rounded to the number of decimal places specified by the currency.
        /// </summary>
        /// <param name="major">The decimal representing the amount of money in major units.</param>
        /// <param name="currency">The currency.</param>
        public static Money OfMajor(decimal major, Currency currency)
            => new Money(major, currency, true);

        /// <summary>
        /// Creates a new instance of the <see cref="Money"/> class for a specific currency
        /// and an amount given in minor units.
        /// </summary>
        /// <param name="minor">The signed long representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        public static Money OfMinor(long minor, Currency currency)
            => new Money(currency.ConvertToMajor(minor), currency, true);

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
        /// and an amount given in minor units for which the number of decimal places is
        /// determined by the currency.
        /// </summary>
        /// <param name="minor">The decimal representing the amount of money in minor units.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="mode">The rounding mode.</param>
        public static Money OfMinor(decimal minor, Currency currency, MidpointRounding mode)
            => new Money(currency.ConvertToMajor(minor), currency, mode);
    }

    // Implements the IFormattable interface.
    public partial struct Money
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.FormatMoney(this, null, NumberFormatInfo.CurrentInfo);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();
            return MoneyFormatter.FormatMoney(this, format, NumberFormatInfo.CurrentInfo);
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

            return MoneyFormatter.FormatMoney(this, format, NumberFormatInfo.GetInstance(formatProvider));
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

        // NB: If we make the other ops implicit, we MUST keep this one explicit:
        // the cast is lossless but the result is not normalized.
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

        // NB: This one is implicit (no unexpected loss of precision, fast & completely harmless).
        public static implicit operator decimal(Money value) => ToDecimal(value);

        #endregion
    }

    // Math operators.
    public partial struct Money
    {
        public int Sign => Sign(this);

        public Money Abs() => MoneyCalculator.Abs(this);

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#", Justification = "[Intentionally] Math.DivRem().")]
        public Money DivRem(long divisor, out Money remainder)
            => MoneyCalculator.DivRem(this, divisor, out remainder);
    }

    // Overrides the op_Addition operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, uint right) => Add(left, right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(uint left, Money right) => Add(right, left);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, ulong right) => Add(left, right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(ulong left, Money right) => Add(right, left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, int right) => Add(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(int left, Money right) => Add(right, left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, long right) => Add(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(long left, Money right) => Add(right, left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, decimal right) => Add(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(decimal left, Money right) => Add(right, left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator +(Money left, Money right) => Add(left, right);

        public Money Plus(Money other) => Add(this, other);

        [CLSCompliant(false)]
        public Money Plus(uint amount) => Add(this, amount);

        [CLSCompliant(false)]
        public Money Plus(ulong amount) => Add(this, amount);

        public Money Plus(int amount) => Add(this, amount);

        public Money Plus(long amount) => Add(this, amount);

        public Money Plus(decimal amount) => Add(this, amount);
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, uint right) => Subtract(left, right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(uint left, Money right) => Subtract(left, right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, ulong right) => Subtract(left, right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(ulong left, Money right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, long right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(long left, Money right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, int right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(int left, Money right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, decimal right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(decimal left, Money right) => Subtract(left, right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money left, Money right) => Subtract(left, right);

        public Money Minus(Money other) => Subtract(this, other);

        [CLSCompliant(false)]
        public Money Minus(uint amount) => Subtract(this, amount);

        [CLSCompliant(false)]
        public Money Minus(ulong amount) => Subtract(this, amount);

        public Money Minus(int amount) => Subtract(this, amount);

        public Money Minus(long amount) => Subtract(this, amount);

        public Money Minus(decimal amount) => Subtract(this, amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Money
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(ulong multiplier, Money money) => Multiply(money, multiplier);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(Money money, ulong multiplier) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(long multiplier, Money money) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(Money money, long multiplier) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(int multiplier, Money money) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(Money money, int multiplier) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(decimal multiplier, Money money) => Multiply(money, multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator *(Money money, decimal multiplier) => Multiply(money, multiplier);

        [CLSCompliant(false)]
        public Money MultiplyBy(uint multiplier) => Multiply(this, multiplier);

        [CLSCompliant(false)]
        public Money MultiplyBy(ulong multiplier) => Multiply(this, multiplier);

        public Money MultiplyBy(int multiplier) => Multiply(this, multiplier);

        public Money MultiplyBy(long multiplier) => Multiply(this, multiplier);

        public Money MultiplyBy(decimal multiplier) => Multiply(this, multiplier);
    }

    // Overrides the op_Division operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator /(Money dividend, decimal divisor) => Divide(dividend, divisor);

        public Money DivideBy(decimal divisor) => Divide(this, divisor);
    }

    // Overrides the op_Modulus operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator %(Money dividend, decimal divisor) => Modulus(dividend, divisor);

        public Money Remainder(decimal divisor) => Modulus(this, divisor);
    }

    // Overrides the op_Increment operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator ++(Money money) => Increment(money);

        public Money IncrementMajor() => Increment(this);

        // For currencies without minor units, this is equivalent to Increment().
        public Money IncrementMinor() => new Money(Amount + Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_Decrement operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator --(Money money) => Decrement(money);

        public Money DecrementMajor() => Decrement(this);

        // For currencies without minor units, this is equivalent to Decrement().
        public Money DecrementMinor() => new Money(Amount - Currency.Epsilon, Currency, IsNormalized);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Money
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.")]
        public static Money operator -(Money money) => MoneyCalculator.Negate(money);

        public Money Negate() => MoneyCalculator.Negate(this);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Money
    {
        // This operator does nothing, only added for completeness.
        public static Money operator +(Money money) => money;

        public Money Plus() => this;
    }
}
