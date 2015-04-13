// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    [Serializable]
    public sealed partial class Money : IComparable, IComparable<Money>, IFormattable
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

        private static void ThrowIfCurrencyMismatch_(Money left, Money right)
        {
            if (left._currency != right._currency)
            {
                throw new ArithmeticException("The currency of both arguments must match to perform this operation.");
            }
        }
    }

    // Implements IFormattable.
    public sealed partial class Money
    {
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Amount.ToString(format, formatProvider);
        }
    }

    // Implements IComparable and IComparable<Money>.
    public sealed partial class Money
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
            ThrowIfCurrencyMismatch_(this, other);

            return _amount.CompareTo(other._amount);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentException();
            }

            if (this.GetType() != obj.GetType())
            {
                throw new ArgumentException();
            }

            return CompareTo(obj as Money);
        }
    }

    // Overrides the + operator.
    public sealed partial class Money
    {
        public static Money Add(Money left, Money right)
        {
            ThrowIfCurrencyMismatch_(left, right);

            return new Money(left._amount + right._amount, left._currency);
        }

        public static Money Add(Money left, decimal right)
        {
            return new Money(left._amount + right, left._currency);
        }

        public static Money operator +(Money left, Money right)
        {
            return Add(left, right);
        }

        public static Money operator +(Money left, decimal right)
        {
            return Add(left, right);
        }

        public static Money operator +(decimal left, Money right)
        {
            return Add(right, left);
        }
    }

    // Overrides the - operator.
    public sealed partial class Money
    {
        public static Money Substract(Money left, Money right)
        {
            ThrowIfCurrencyMismatch_(left, right);

            return new Money(left._amount - right._amount, left._currency);
        }

        public static Money Substract(Money left, decimal right)
        {
            return new Money(left._amount - right, left._currency);
        }

        public static Money operator -(Money left, Money right)
        {
            return Substract(left, right);
        }

        public static Money operator -(Money left, decimal right)
        {
            return Substract(left, right);
        }

        public static Money operator -(decimal left, Money right)
        {
            return Substract(right, left);
        }
    }

    // Overrides the * operator.
    public sealed partial class Money
    {
        public static Money Multiply(decimal left, Money right)
        {
            return new Money(left * right._amount, right._currency);
        }

        public static Money operator *(decimal left, Money right)
        {
            return Multiply(left, right);
        }

        public static Money operator *(Money left, decimal right)
        {
            return Multiply(right, left);
        }
    }
}
