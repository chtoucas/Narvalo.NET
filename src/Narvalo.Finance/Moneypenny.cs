// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Properties;
    using Narvalo.Finance.Utilities;

    // A lightweight money type.
    // Moneypenny is mostly self-contained (see also PennyCalculator && PennyFactory).
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

        private void ThrowIfCurrencyMismatch(Moneypenny penny, string parameterName)
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

    // Math methods.
    public partial struct Moneypenny
    {
        public int Sign => Amount < 0L ? -1 : (Amount > 0L ? 1 : 0);

        public Moneypenny Abs() => IsPositiveOrZero ? this : Negate();

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

    // Allocations.
    public partial struct Moneypenny
    {
        public IEnumerable<Moneypenny> Allocate(int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Moneypenny>>();

            long q = Amount / count;
            var part = new Moneypenny(q, Currency);

            for (var i = 0; i < count - 1; i++)
            {
                yield return part;
            }

            yield return new Moneypenny(Amount - (count - 1) * q, Currency);
        }

        public IEnumerable<Moneypenny> AllocateEvenly(int count)
        {
            Require.Range(count > 1, nameof(count));
            Warrant.NotNull<IEnumerable<Moneypenny>>();

            var cy = Currency;

            return from _ in Integer.DivideEvenly(Amount, count) select new Moneypenny(_, cy);
        }

        //public IEnumerable<Moneypenny> Allocate(Moneypenny money, RatioArray ratios)
        //{
        //    Currency currency = money.Currency;
        //    long total = money.Amount;

        //    int len = ratios.Length;
        //    var dist = new decimal[len];
        //    long last = total;

        //    for (var i = 0; i < len - 1; i++)
        //    {
        //        long amount = ratios[i] * total;
        //        last -= amount;
        //        yield return new Moneypenny(amount, currency);
        //    }

        //    yield return new Moneypenny(last, currency);
        //}
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
