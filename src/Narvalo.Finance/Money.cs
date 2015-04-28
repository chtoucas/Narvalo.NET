// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    [DebuggerDisplay("{{ToString()}}")]
    public partial struct Money
        : IEquatable<Money>, IComparable<Money>, IComparable, IFormattable
    {
        private readonly Currency _currency;
        private readonly decimal _amount;

        public Money(decimal amount, Currency currency)
        {
            Require.NotNull(currency, "currency");

            _amount = amount;
            _currency = currency;
        }

        public decimal Amount { get { return _amount; } }

        public Currency Currency
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);

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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(_currency != null);
        }

#endif
    }

    /// <content>
    /// Implements the <see cref="IEquatable{Money}"/> interface.
    /// </content>
    public partial struct Money
    {
        public static bool operator ==(Money left, Money right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Money other)
        {
            return Amount == other.Amount && Currency.Equals(other.Currency);
        }

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

    /// <content>
    /// Implements the <see cref="IFormattable"/> interface.
    /// </content>
    public partial struct Money
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return MoneyFormatter.Format(Amount, Currency);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return MoneyFormatter.Format(Amount, format, formatProvider);
        }
    }

    /// <content>
    /// Implements the <see cref="IComparable"/> and <see cref="IComparable{Money}"/> interfaces.
    /// </content>
    public partial struct Money
    {
        public static bool operator <(Money left, Money right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Money left, Money right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Money left, Money right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Money left, Money right)
        {
            return left.CompareTo(right) >= 0;
        }

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
                throw new ArgumentException(Strings.Money_ArgIsNotMoney);
            }

            return CompareTo((Money)obj);
        }
    }

    /// <content>
    /// Overrides the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money left, Money right)
        {
            return left.Add(right);
        }

        public Money Add(Money money)
        {
            ThrowIfCurrencyMismatch(money);

            var amount = checked(Amount + money.Amount);

            return new Money(amount, Currency);
        }

        public Money Add(decimal amount)
        {
            var value = checked(Amount + amount);

            return new Money(value, Currency);
        }
    }

    /// <content>
    /// Overrides the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money left, Money right)
        {
            return left.Subtract(right);
        }

        public Money Subtract(Money money)
        {
            ThrowIfCurrencyMismatch(money);

            return new Money(Amount - money.Amount, Currency);
        }

        public Money Subtract(decimal amount)
        {
            return new Money(Amount - amount, Currency);
        }
    }

    /// <content>
    /// Overrides the <c>op_Multiply</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator *(decimal multiplier, Money money)
        {
            return money.Multiply(multiplier);
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            return money.Multiply(multiplier);
        }

        public Money Multiply(decimal multiplier)
        {
            var amount = checked(multiplier * Amount);

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
            Contract.Requires(divisor != 0m);

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
            Contract.Requires(divisor != 0m);

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
        public static Money operator -(Money money)
        {
            return money.Negate();
        }

        public Money Negate()
        {
            return new Money(-Amount, Currency);
        }
    }

    /// <content>
    /// Overrides the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money money)
        {
            return money;
        }

        public Money Plus()
        {
            return this;
        }
    }
}
