// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    using Narvalo.Finance.Properties;

    // FIXME: Use int's to represent the amount and, later on, create a BigMoney struct based
    // on BigRational (BigDecimal?) for arbitrary-precision calculations.
    [StructLayout(LayoutKind.Auto)]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial struct Money
        : IEquatable<Money>, IComparable<Money>, IComparable, IFormattable
    {
        private readonly Currency _currency;

        public Money(decimal amount, Currency currency)
        {
            Require.NotNull(currency, nameof(currency));

            Amount = amount;
            _currency = currency;
        }

        public decimal Amount { get; }

        public Currency Currency
        {
            get { Warrant.NotNull<Currency>(); return _currency; }
        }

        [ExcludeFromCodeCoverage(Justification = "Debugger-only code.")]
        private string DebuggerDisplay => Format.Invariant("{0:F2} ({1})", Amount, Currency.Code);

        // Check that the currencies match.
        private void ThrowIfCurrencyMismatch(Money other, string parameterName)
        {
            if (Currency != other.Currency)
            {
                throw new ArgumentException("XXX", parameterName);
            }
        }
    }

    // Implements the IFormattable interface.
    public partial struct Money
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            Warrant.NotNull<string>();

            return Format.Current("{0:F2} ({1})", Amount, Currency.Code);
        }

        public string ToString(string format)
        {
            Warrant.NotNull<string>();

            return ToString(format, null);
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

            // FIXME: wrong.
            return Amount.ToString(format, formatProvider);
        }
    }

    // Implements the IEquatable<Money> interface.
    public partial struct Money
    {
        public static bool operator ==(Money left, Money right) => left.Equals(right);

        public static bool operator !=(Money left, Money right) => !left.Equals(right);

        public bool Equals(Money other) => Amount == other.Amount && Currency.Equals(other.Currency);

        public override bool Equals(object obj)
        {
            if (!(obj is Money))
            {
                return false;
            }

            return Equals((Money)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + ((long)Amount).GetHashCode();
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
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Money))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney, nameof(obj));
            }

            return CompareTo((Money)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Money
    {
        public static Money operator +(Money left, Money right) => left.Add(right);

        public Money Add(Money other)
        {
            ThrowIfCurrencyMismatch(other, nameof(other));

            decimal amount = checked(Amount + other.Amount);

            return new Money(amount, Currency);
        }

        public Money Add(decimal amount)
        {
            decimal value = checked(Amount + amount);

            return new Money(value, Currency);
        }
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

        public Money Multiply(decimal multiplier)
        {
            decimal amount = checked(multiplier * Amount);

            return new Money(amount, Currency);
        }
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

#if CONTRACTS_FULL

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    public partial struct Money
    {
        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_currency != null);
        }
    }
}

#endif
