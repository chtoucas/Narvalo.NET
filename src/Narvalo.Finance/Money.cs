// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;

    using Narvalo.Finance.Properties;

    // FIXME: Overflow operations.
    [DebuggerDisplay("{{ToString()}}")]
    public partial struct Money
        : IEquatable<Money>, IComparable, IComparable<Money>, IFormattable
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

        public Currency Currency { get { return _currency; } }

        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return Format.CurrentCulture("{0} {1:F2}", Currency.Code, Amount);
        }

        private static void ThrowIfCurrencyMismatch_(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                // FIXME: Throw the correct exception.
                // The currency of both arguments must match to perform this operation
                throw new ArithmeticException();
            }
        }
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
                hash = (23 * hash) + Amount.GetHashCode();
                hash = (23 * hash) + Currency.GetHashCode();
                return hash;
            }
        }
    }

    /// <content>
    /// Implements the <see cref="IFormattable"/> interface.
    /// </content>
    public partial struct Money
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Amount.ToString(format, formatProvider);
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

        public static int Compare(Money left, Money right)
        {
            return left.CompareTo(right);
        }

        public int CompareTo(Money other)
        {
            ThrowIfCurrencyMismatch_(this, other);

            return Amount.CompareTo(other.Amount);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Money))
            {
                throw new ArgumentException(Strings_Finance.Money_ArgIsNotMoney);
            }

            return CompareTo((Money)obj);
        }
    }

    /// <content>
    /// Implements the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money left, Money right)
        {
            return left.Add(right);
        }

        public static Money operator +(Money left, decimal right)
        {
            return left.Add(right);
        }

        public static Money operator +(decimal left, Money right)
        {
            return right.Add(left);
        }

        public static Money Add(Money left, Money right)
        {
            return left.Add(right);
        }

        public static Money Add(Money left, decimal right)
        {
            return left.Add(right);
        }

        public Money Add(Money other)
        {
            ThrowIfCurrencyMismatch_(this, other);

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Add(decimal other)
        {
            return new Money(Amount + other, Currency);
        }
    }

    /// <content>
    /// Implements the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money left, Money right)
        {
            return left.Subtract(right);
        }

        public static Money operator -(Money left, decimal right)
        {
            return left.Subtract(right);
        }

        public static Money operator -(decimal left, Money right)
        {
            return right.Subtract(left);
        }

        public static Money Subtract(Money left, Money right)
        {
            return left.Subtract(right);
        }

        public static Money Subtract(Money left, decimal right)
        {
            return left.Subtract(right);
        }

        public Money Subtract(Money other)
        {
            ThrowIfCurrencyMismatch_(this, other);

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Subtract(decimal other)
        {
            return new Money(Amount - other, Currency);
        }
    }

    /// <content>
    /// Implements the <c>op_Multiply</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator *(decimal scaleFactor, Money right)
        {
            return Multiply(scaleFactor, right);
        }

        public static Money operator *(Money left, decimal scaleFactor)
        {
            return Multiply(scaleFactor, left);
        }

        public static Money Multiply(decimal scaleFactor, Money right)
        {
            return new Money(scaleFactor * right.Amount, right.Currency);
        }
    }

    /// <content>
    /// Implements the <c>op_UnaryNegation</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money money)
        {
            return Negate(money);
        }

        public static Money Negate(Money money)
        {
            return new Money(-money.Amount, money.Currency);
        }
    }

    /// <content>
    /// Implements the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money money)
        {
            return money;
        }

        public static Money Plus(Money money)
        {
            return money;
        }
    }
}
