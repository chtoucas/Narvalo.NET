// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Runtime.InteropServices;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    // FIXME: Use int's to represent the amount and, later on, create a BigMoney struct based
    // on BigRational (BigDecimal?) for arbitrary-precision calculations.
    [StructLayout(LayoutKind.Auto)]
    public partial struct Money
        : IEquatable<Money>, IComparable<Money>, IComparable, IFormattable
    {
        private readonly Currency _currency;
        private readonly decimal _amount;

        public Money(decimal amount, Currency currency)
        {
            Require.NotNull(currency, nameof(currency));

            _amount = amount;
            _currency = currency;
        }

        public decimal Amount { get { return _amount; } }

        public Currency Currency
        {
            get
            {
                Warrant.NotNull<Currency>();

                return _currency;
            }
        }

        // Check that currencies match.
        private void ThrowIfCurrencyMismatch(Money other)
        {
            if (Currency != other.Currency)
            {
                // FIXME: Throw the correct exception.
                throw new ArithmeticException();
            }
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

    // Implements the IFormattable interface.
    public partial struct Money
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() => MoneyFormatter.Format(Amount, Currency);

        public string ToString(string format, IFormatProvider formatProvider)
            => MoneyFormatter.Format(Amount, format, formatProvider);
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
            ThrowIfCurrencyMismatch(other);

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

    /// <content>
    /// Overrides the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money left, Money right) => left.Add(right);

        public Money Add(Money money)
        {
            ThrowIfCurrencyMismatch(money);

            decimal amount = checked(Amount + money.Amount);

            return new Money(amount, Currency);
        }

        public Money Add(decimal amount)
        {
            decimal value = checked(Amount + amount);

            return new Money(value, Currency);
        }
    }

    /// <content>
    /// Overrides the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money left, Money right) => left.Subtract(right);

        public Money Subtract(Money money)
        {
            ThrowIfCurrencyMismatch(money);

            return new Money(Amount - money.Amount, Currency);
        }

        public Money Subtract(decimal amount) => new Money(Amount - amount, Currency);
    }

    /// <content>
    /// Overrides the <c>op_Multiply</c> operator.
    /// </content>
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

    /// <content>
    /// Overrides the <c>op_Division</c> operator.
    /// </content>
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

    /// <content>
    /// Overrides the <c>op_Modulus</c> operator.
    /// </content>
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

    /// <content>
    /// Overrides the <c>op_UnaryNegation</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money money) => money.Negate();

        public Money Negate() => new Money(-Amount, Currency);
    }

    /// <content>
    /// Overrides the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money money) => money;

        public Money Plus() => this;
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

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
