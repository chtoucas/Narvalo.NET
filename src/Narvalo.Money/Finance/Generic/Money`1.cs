// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Generic
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Globalization;
    using Narvalo.Properties;
    using Narvalo.Finance.Rounding;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money<TCurrency> : Internal.IMoney<Money<TCurrency>, TCurrency>
        where TCurrency : Currency<TCurrency>
    {
        // IMPORTANT: This static field MUST remain first in order to be initialized before the other(s).
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_UnderlyingUnit = CurrencyUnit.OfType<TCurrency>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Epsilon = new Money<TCurrency>(UnderlyingUnit.Epsilon);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_One = new Money<TCurrency>(UnderlyingUnit.One);

        [CLSCompliant(false)]
        public Money(uint amount) : this(amount, true) { }

        [CLSCompliant(false)]
        public Money(ulong amount) : this(amount, true) { }

        public Money(int amount) : this(amount, true) { }

        public Money(long amount) : this(amount, true) { }

        public Money(decimal amount) : this(amount, false) { }

        internal Money(decimal amount, bool normalized)
        {
            Amount = amount;
            IsNormalized = UnderlyingUnit.HasFixedDecimalPlaces ? normalized : true;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> Zero => s_Zero;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> Epsilon => s_Epsilon;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes", Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> One => s_One;

        internal static TCurrency UnderlyingUnit => s_UnderlyingUnit;

        public decimal Amount { get; }

        public TCurrency Currency => UnderlyingUnit;

        public bool IsNormalized { get; }

        public bool IsRoundable => UnderlyingUnit.HasFixedDecimalPlaces;

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

        public Money<TCurrency> Normalize(MidpointRounding mode)
        {
            if (IsNormalized) { return this; }
            return MoneyFactory.FromMajor<TCurrency>(Amount, mode);
        }

        public Money<TCurrency> Normalize(IRoundingAdjuster adjuster)
        {
            if (IsNormalized) { return this; }
            return MoneyFactory.FromMajor<TCurrency>(Amount, adjuster);
        }

        [ExcludeFromCodeCoverage]
        private string DebuggerDisplay
            => "(" + Currency.Code + ") " + Amount.ToString("F2", CultureInfo.CurrentCulture)
            + "; IsNormalized=" + (IsNormalized ? "true" : "false");
    }

    // Domain-specific conversions.
    public partial struct Money<TCurrency>
    {
        public decimal ToMinor() => UnderlyingUnit.ConvertToMinor(Amount);

        public long? ToLongMinor()
        {
            if (!IsNormalized)
            {
                throw new InvalidCastException(Strings.InvalidConversion_MoneyIsNotNormalized);
            }

            decimal minor = ToMinor();
            if (minor < Int64.MinValue || minor > Int64.MaxValue) { return null; }
            return Convert.ToInt64(minor);
        }

        public Money ToMoney() => new Money(Amount, Currency.ToCurrency(), IsNormalized);

        public Moneypenny? ToPenny()
        {
            if (!IsRounded)
            {
                throw new InvalidCastException(Strings.InvalidConversion_MoneyIsNotRounded);
            }

            long? amount = ToLongMinor();
            return amount.HasValue
                ? new Moneypenny(amount.Value, Currency.ToCurrency())
                : (Moneypenny?)null;
        }

        // Explicit conversion that should not fail.
        public static explicit operator Money(Money<TCurrency> value) => value.ToMoney();
    }

    // Numeric conversions.
    public partial struct Money<TCurrency>
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
        public static implicit operator decimal(Money<TCurrency> value) => value.Amount;

        #region Harmless explicit conversions from a numeric type to Money<T> w/o Currency.

        [CLSCompliant(false)]
        public static explicit operator Money<TCurrency>(sbyte value) => new Money<TCurrency>(value);

        [CLSCompliant(false)]
        public static explicit operator Money<TCurrency>(ushort value) => new Money<TCurrency>(value);

        [CLSCompliant(false)]
        public static explicit operator Money<TCurrency>(uint value) => new Money<TCurrency>(value);

        [CLSCompliant(false)]
        public static explicit operator Money<TCurrency>(ulong value) => new Money<TCurrency>(value);

        public static explicit operator Money<TCurrency>(byte value) => new Money<TCurrency>(value);

        public static explicit operator Money<TCurrency>(short value) => new Money<TCurrency>(value);

        public static explicit operator Money<TCurrency>(int value) => new Money<TCurrency>(value);

        public static explicit operator Money<TCurrency>(long value) => new Money<TCurrency>(value);

        // WARNING: Even if we made the other ops implicit, we should keep this one explicit:
        // the cast is lossless but the result is not normalized.
        public static explicit operator Money<TCurrency>(decimal value) => new Money<TCurrency>(value, false);

        #endregion
    }

    // Implements the IFormattable interface.
    public partial struct Money<TCurrency>
    {
        public override string ToString() => FormatImpl(null, NumberFormatInfo.CurrentInfo);

        public string ToString(string format) => FormatImpl(format, NumberFormatInfo.CurrentInfo);

        public string ToString(IFormatProvider formatProvider) => ToString(null, formatProvider);

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (formatProvider != null)
            {
                object fmt = formatProvider.GetFormat(typeof(Money<TCurrency>));
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

    // Implements the IEquatable interfaces.
    public partial struct Money<TCurrency>
    {
        public static bool operator ==(Money<TCurrency> left, Money<TCurrency> right) => left.Equals(right);

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right) => !left.Equals(right);

        public bool Equals(Money<TCurrency> other) => Amount == other.Amount;

        public override bool Equals(object obj)
            => (obj is Money<TCurrency>) && Equals((Money<TCurrency>)obj);

        public override int GetHashCode()
            => HashCodeHelpers.Combine(Amount.GetHashCode(), Currency.GetHashCode(), IsNormalized.GetHashCode());
    }

    // Implements the IComparable and IComparable<Money<TCurrency>> interfaces.
    public partial struct Money<TCurrency>
    {
        public static bool operator <(Money<TCurrency> left, Money<TCurrency> right) => left.CompareTo(right) < 0;

        public static bool operator <=(Money<TCurrency> left, Money<TCurrency> right) => left.CompareTo(right) <= 0;

        public static bool operator >(Money<TCurrency> left, Money<TCurrency> right) => left.CompareTo(right) > 0;

        public static bool operator >=(Money<TCurrency> left, Money<TCurrency> right) => left.CompareTo(right) >= 0;

        public int CompareTo(Money<TCurrency> other) => Amount.CompareTo(other.Amount);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is Money<TCurrency>))
            {
                throw new ArgumentException(Strings.Argument_CannotCompare, nameof(obj));
            }

            return CompareTo((Money<TCurrency>)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Money<TCurrency>
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, uint right) => left.Plus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(uint left, Money<TCurrency> right) => right.Plus(left);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, ulong right) => left.Plus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(ulong left, Money<TCurrency> right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, int right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(int left, Money<TCurrency> right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, long right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(long left, Money<TCurrency> right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, decimal right) => left.Plus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Plus().")]
        public static Money<TCurrency> operator +(decimal left, Money<TCurrency> right) => right.Plus(left);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] See MoneyCalculator.Add().")]
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right) => left.Plus(right);

        public Money<TCurrency> Plus(Money<TCurrency> other)
        {
            if (Amount == 0m) { return other; }
            if (other.Amount == 0m) { return this; }
            return new Money<TCurrency>(Amount + other.Amount);
        }

        [CLSCompliant(false)]
        public Money<TCurrency> Plus(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money<TCurrency>(Amount + amount, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money<TCurrency> Plus(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money<TCurrency>(Amount + amount, IsNormalized);
        }

        public Money<TCurrency> Plus(int amount)
        {
            if (amount == 0) { return this; }
            return new Money<TCurrency>(Amount + amount, IsNormalized);
        }

        public Money<TCurrency> Plus(long amount)
        {
            if (amount == 0L) { return this; }
            return new Money<TCurrency>(Amount + amount, IsNormalized);
        }

        public Money<TCurrency> Plus(decimal amount)
        {
            if (amount == 0m) { return this; }
            return new Money<TCurrency>(Amount + amount, false);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money<TCurrency>
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, uint right) => left.Minus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money<TCurrency> operator -(uint left, Money<TCurrency> right)
            => new Money<TCurrency>(left - right.Amount, right.IsNormalized);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, ulong right) => left.Minus(right);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money<TCurrency> operator -(ulong left, Money<TCurrency> right)
            => new Money<TCurrency>(left - right.Amount, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, long right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money<TCurrency> operator -(long left, Money<TCurrency> right)
            => new Money<TCurrency>(left - right.Amount, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, int right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money<TCurrency> operator -(int left, Money<TCurrency> right)
            => new Money<TCurrency>(left - right.Amount, right.IsNormalized);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, decimal right) => left.Minus(right);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Not provided.")]
        public static Money<TCurrency> operator -(decimal left, Money<TCurrency> right)
        {
            if (left == 0m) { return right.Negate(); }
            return new Money<TCurrency>(left - right.Amount, false);
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named Minus().")]
        public static Money<TCurrency> operator -(Money<TCurrency> left, Money<TCurrency> right) => left.Minus(right);

        public Money<TCurrency> Minus(Money<TCurrency> other)
        {
            if (Amount == 0m) { return other.Negate(); }
            if (other.Amount == 0m) { return this; }
            return new Money<TCurrency>(Amount - other.Amount);
        }

        [CLSCompliant(false)]
        public Money<TCurrency> Minus(uint amount)
        {
            if (amount == 0) { return this; }
            return new Money<TCurrency>(Amount - amount, IsNormalized);
        }

        [CLSCompliant(false)]
        public Money<TCurrency> Minus(ulong amount)
        {
            if (amount == 0UL) { return this; }
            return new Money<TCurrency>(Amount - amount, IsNormalized);
        }

        public Money<TCurrency> Minus(int amount) => Plus(-amount);

        public Money<TCurrency> Minus(long amount) => Plus(-amount);

        public Money<TCurrency> Minus(decimal amount) => Plus(-amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Money<TCurrency>
    {
        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(ulong multiplier, Money<TCurrency> money) => money.MultiplyBy(multiplier);

        [CLSCompliant(false)]
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(Money<TCurrency> money, ulong multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(long multiplier, Money<TCurrency> money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(Money<TCurrency> money, long multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(int multiplier, Money<TCurrency> money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(Money<TCurrency> money, int multiplier) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(decimal multiplier, Money<TCurrency> money) => money.MultiplyBy(multiplier);

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named MultiplyBy().")]
        public static Money<TCurrency> operator *(Money<TCurrency> money, decimal multiplier) => money.MultiplyBy(multiplier);

        [CLSCompliant(false)]
        public Money<TCurrency> MultiplyBy(uint multiplier) => new Money<TCurrency>(multiplier * Amount, IsNormalized);

        [CLSCompliant(false)]
        public Money<TCurrency> MultiplyBy(ulong multiplier) => new Money<TCurrency>(multiplier * Amount, IsNormalized);

        public Money<TCurrency> MultiplyBy(int multiplier) => new Money<TCurrency>(multiplier * Amount, IsNormalized);

        public Money<TCurrency> MultiplyBy(long multiplier) => new Money<TCurrency>(multiplier * Amount, IsNormalized);

        public Money<TCurrency> MultiplyBy(decimal multiplier) => new Money<TCurrency>(multiplier * Amount);
    }

    // Overrides the op_Division operator.
    public partial struct Money<TCurrency>
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named DivideBy().")]
        public static Money<TCurrency> operator /(Money<TCurrency> money, decimal divisor) => money.DivideBy(divisor);

        public Money<TCurrency> DivideBy(decimal divisor) => new Money<TCurrency>(Amount / divisor);
    }

    // Overrides the op_Modulus operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator %(Money<TCurrency> money, decimal divisor) => money.Mod(divisor);

        public Money<TCurrency> Mod(decimal divisor) => new Money<TCurrency>(Amount % divisor);
    }

    // Overrides the op_Increment operator.
    public partial struct Money<TCurrency>
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named IncrementMajor().")]
        public static Money<TCurrency> operator ++(Money<TCurrency> money) => money.IncrementMajor();

        public Money<TCurrency> IncrementMajor() => new Money<TCurrency>(Amount + UnderlyingUnit.One);

        // For currencies without minor units, this is equivalent to Increment().
        public Money<TCurrency> IncrementMinor() => new Money<TCurrency>(Amount + UnderlyingUnit.Epsilon);
    }

    // Overrides the op_Decrement operator.
    public partial struct Money<TCurrency>
    {
        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "[Intentionally] Named DecrementMajor().")]
        public static Money<TCurrency> operator --(Money<TCurrency> money) => money.DecrementMajor();

        public Money<TCurrency> DecrementMajor() => new Money<TCurrency>(Amount - UnderlyingUnit.One);

        // For currencies without minor units, this is equivalent to Decrement().
        public Money<TCurrency> DecrementMinor() => new Money<TCurrency>(Amount - UnderlyingUnit.Epsilon);
    }

    // Overrides the op_UnaryNegation operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> money) => money.Negate();

        public Money<TCurrency> Negate() => new Money<TCurrency>(-Amount);
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> money) => money.Plus();

        public Money<TCurrency> Plus() => this;
    }
}
