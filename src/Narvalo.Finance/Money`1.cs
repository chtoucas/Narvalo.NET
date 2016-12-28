// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Narvalo.Finance.Globalization;
    using Narvalo.Finance.Properties;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money<TCurrency>
        : IEquatable<Money<TCurrency>>, IComparable<Money<TCurrency>>, IComparable, IFormattable
        where TCurrency : CurrencyUnit<TCurrency>
    {
        // IMPORTANT: This static field MUST remain first in order to be initialized before the other(s).
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_UnderlyingUnit = CurrencyUnit.OfType<TCurrency>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0M);

        public Money(decimal amount)
        {
            Amount = amount;
        }

        internal static TCurrency UnderlyingUnit
        {
            get { Warrant.NotNull<TCurrency>(); return s_UnderlyingUnit; }
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> Zero => s_Zero;

        public decimal Amount { get; }

        public TCurrency Unit
        {
            get { Warrant.NotNull<TCurrency>(); return UnderlyingUnit; }
        }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        private string DebuggerDisplay => Format.Invariant("{0:F2} ({1})", Amount, Unit.Code);

        public static explicit operator Money<TCurrency>(Money value)
        {
            if (!(value.Currency.Code == UnderlyingUnit.Code))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(value.Amount);
        }

        public static explicit operator Money(Money<TCurrency> value)
            => new Money(value.Amount, value.Unit.ToCurrency());
    }

    // Implements the IFormattable interface.
    public partial struct Money<TCurrency>
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

            return MoneyFormatter.Format(this, null, NumberFormatInfo.GetInstance(formatProvider));
        }
    }

    // Implements the IEquatable interfaces.
    public partial struct Money<TCurrency>
    {
        public static bool operator ==(Money<TCurrency> left, Money<TCurrency> right) => left.Equals(right);

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right) => !left.Equals(right);

        public bool Equals(Money<TCurrency> other) => Amount == other.Amount;

        public override bool Equals(object obj)
        {
            if (!(obj is Money<TCurrency>)) { return false; }

            return Equals((Money<TCurrency>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + Amount.GetHashCode();
                hash = 31 * hash + Unit.GetHashCode();
                return hash;
            }
        }
    }

    // Implements the IComparable interfaces.
    public partial struct Money<TCurrency>
    {
        public static bool operator <(Money<TCurrency> left, Money<TCurrency> right)
            => left.CompareTo(right) < 0;

        public static bool operator <=(Money<TCurrency> left, Money<TCurrency> right)
            => left.CompareTo(right) <= 0;

        public static bool operator >(Money<TCurrency> left, Money<TCurrency> right)
            => left.CompareTo(right) > 0;

        public static bool operator >=(Money<TCurrency> left, Money<TCurrency> right)
            => left.CompareTo(right) >= 0;

        public int CompareTo(Money<TCurrency> other) => Amount.CompareTo(other.Amount);

        int IComparable.CompareTo(object obj)
        {
            if (obj == null) { return 1; }

            if (!(obj is Money<TCurrency>))
            {
                throw new ArgumentException(Strings.Argument_InvalidMoneyType, nameof(obj));
            }

            return CompareTo((Money<TCurrency>)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right)
            => left.Add(right);

        public Money<TCurrency> Add(Money<TCurrency> other)
        {
            decimal amount = checked(Amount + other.Amount);

            return new Money<TCurrency>(amount);
        }

        public Money<TCurrency> Add(decimal amount)
        {
            decimal value = checked(Amount + amount);

            return new Money<TCurrency>(value);
        }
    }

    // Overrides the op_Subtraction operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> left, Money<TCurrency> right)
            => left.Subtract(right);

        public Money<TCurrency> Subtract(Money<TCurrency> other) => new Money<TCurrency>(Amount - other.Amount);

        public Money<TCurrency> Subtract(decimal amount) => new Money<TCurrency>(Amount - amount);
    }

    // Overrides the op_Multiply operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator *(decimal multiplier, Money<TCurrency> money)
            => money.Multiply(multiplier);

        public static Money<TCurrency> operator *(Money<TCurrency> money, decimal multiplier)
            => money.Multiply(multiplier);

        public Money<TCurrency> Multiply(decimal multiplier)
        {
            var amount = checked(multiplier * Amount);

            return new Money<TCurrency>(amount);
        }
    }

    // Overrides the op_Division operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator /(Money<TCurrency> money, decimal divisor)
        {
            Expect.True(divisor != 0m);

            return money.Divide(divisor);
        }

        public Money<TCurrency> Divide(decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money<TCurrency>(Amount / divisor);
        }
    }

    // Overrides the op_Modulus operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator %(Money<TCurrency> money, decimal divisor)
        {
            Expect.True(divisor != 0m);

            return money.Remainder(divisor);
        }

        public Money<TCurrency> Remainder(decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money<TCurrency>(Amount % divisor);
        }
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
        public static Money<TCurrency> operator +(Money<TCurrency> money) => money;

        public Money<TCurrency> Plus() => this;
    }
}
