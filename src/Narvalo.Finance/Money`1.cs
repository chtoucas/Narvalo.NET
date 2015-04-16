// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    // FIXME: Overflow operations.
    // FIXME: What if s_Currency throw an exception?
    // FIXME: Replace implicit by explicit
    // FIXME: Replace _amount by a private instance of Money.
    [DebuggerDisplay("{{ToString()}}")]
    [SuppressMessage("Gendarme.Rules.Design", "ProvideAlternativeNamesForOperatorOverloadsRule",
        Justification = "[Intentionally] We do provide the alternate but we called it CompareTo.")]
    public partial struct Money<TCurrency>
        : IEquatable<Money<TCurrency>>, IEquatable<Money>,
        IComparable, IComparable<Money<TCurrency>>, IComparable<Money>,
        IFormattable
        where TCurrency : Currency
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0m);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_Currency = CurrencyActivator<TCurrency>.CreateInstance();

        private readonly decimal _amount;
        //private readonly Money _value;

        public Money(decimal amount)
        {
            _amount = amount;
            //_value = new Money(amount, s_Currency);
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> Zero
        {
            get
            {
                Contract.Ensures(Contract.Result<TCurrency>() != null);

                return s_Zero;
            }
        }

        public decimal Amount { get { return _amount; } }

        public static explicit operator Money<TCurrency>(Money value)
        {
            if (!CheckCurrency_(value))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(value.Amount);
        }

        public static implicit operator Money(Money<TCurrency> value)
        {
            return new Money(value.Amount, s_Currency);
        }

        private static bool CheckCurrency_(Money value)
        {
            return value.Currency == s_Currency;
        }
    }

    /// <content>
    /// Implements the <c>IEquatable</c> interfaces.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static bool operator ==(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(Money<TCurrency> left, Money right)
        {
            return left.Equals(right);
        }

        public static bool operator ==(Money left, Money<TCurrency> right)
        {
            return right.Equals(left);
        }

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(Money<TCurrency> left, Money right)
        {
            return !left.Equals(right);
        }

        public static bool operator !=(Money left, Money<TCurrency> right)
        {
            return !right.Equals(left);
        }

        public bool Equals(Money<TCurrency> other)
        {
            return Amount == other.Amount;
        }

        public bool Equals(Money other)
        {
            return Amount == other.Amount && CheckCurrency_(other);
        }

        public override bool Equals(object obj)
        {
            // FIXME: Boxing.
            if (obj is Money<TCurrency>)
            {
                return Equals((Money<TCurrency>)obj);
            }

            if (obj is Money)
            {
                return Equals((Money)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (23 * hash) + Amount.GetHashCode();
                hash = (23 * hash) + s_Currency.GetHashCode();
                return hash;
            }
        }
    }

    /// <content>
    /// Implements the <see cref="IFormattable"/> interface.
    /// </content>
    public partial struct Money<TCurrency>
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return Format.CurrentCulture("{0} {1:F2}", s_Currency.Code, Amount);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // FIXME
            return Amount.ToString(format, formatProvider);
        }
    }

    /// <content>
    /// Implements the <c>IComparable</c> interfaces.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static bool operator <(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <(Money<TCurrency> left, Money right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <(Money left, Money<TCurrency> right)
        {
            return right.CompareTo(left) >= 0;
        }

        public static bool operator <=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator <=(Money<TCurrency> left, Money right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator <=(Money left, Money<TCurrency> right)
        {
            return right.CompareTo(left) > 0;
        }

        public static bool operator >(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >(Money<TCurrency> left, Money right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >(Money left, Money<TCurrency> right)
        {
            return right.CompareTo(left) <= 0;
        }

        public static bool operator >=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator >=(Money<TCurrency> left, Money right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator >=(Money left, Money<TCurrency> right)
        {
            return right.CompareTo(left) < 0;
        }

        public int CompareTo(Money<TCurrency> other)
        {
            return Amount.CompareTo(other.Amount);
        }

        public int CompareTo(Money other)
        {
            if (!CheckCurrency_(other))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney);
            }

            return Amount.CompareTo(other.Amount);
        }

        public int CompareTo(object obj)
        {
            // FIXME: Boxing.
            if (obj == null)
            {
                return 1;
            }

            if (obj is Money<TCurrency>)
            {
                return CompareTo((Money<TCurrency>)obj);
            }

            if (obj is Money)
            {
                return CompareTo((Money)obj);
            }

            throw new ArgumentException(Strings.Money_ArgIsNotMoney);
        }
    }

    /// <content>
    /// Implements the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.Add(right);
        }

        public static Money<TCurrency> operator +(Money<TCurrency> left, Money right)
        {
            return left.Add(right);
        }

        public static Money<TCurrency> operator +(Money left, Money<TCurrency> right)
        {
            return right.Add(left);
        }

        public static Money<TCurrency> operator +(Money<TCurrency> left, decimal right)
        {
            return left.Add(right);
        }

        public static Money<TCurrency> operator +(decimal left, Money<TCurrency> right)
        {
            return right.Add(left);
        }

        public Money<TCurrency> Add(Money<TCurrency> other)
        {
            return new Money<TCurrency>(Amount + other.Amount);
        }

        public Money<TCurrency> Add(Money other)
        {
            if (!CheckCurrency_(other))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney);
            }

            return new Money<TCurrency>(Amount + other.Amount);
        }

        public Money<TCurrency> Add(decimal other)
        {
            return new Money<TCurrency>(Amount + other);
        }
    }

    /// <content>
    /// Implements the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.Subtract(right);
        }

        public static Money<TCurrency> operator -(Money<TCurrency> left, Money right)
        {
            return left.Subtract(right);
        }

        public static Money<TCurrency> operator -(Money left, Money<TCurrency> right)
        {
            return right.Subtract(left);
        }

        public static Money<TCurrency> operator -(Money<TCurrency> left, decimal right)
        {
            return left.Subtract(right);
        }

        public static Money<TCurrency> operator -(decimal left, Money<TCurrency> right)
        {
            return right.Subtract(left);
        }

        public Money<TCurrency> Subtract(Money<TCurrency> other)
        {
            return new Money<TCurrency>(Amount - other.Amount);
        }

        public Money<TCurrency> Subtract(Money other)
        {
            if (!CheckCurrency_(other))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney);
            }

            return new Money<TCurrency>(Amount - other.Amount);
        }

        public Money<TCurrency> Subtract(decimal other)
        {
            return new Money<TCurrency>(Amount - other);
        }
    }

    /// <content>
    /// Implements the <c>op_Multiply</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator *(decimal multiplier, Money<TCurrency> right)
        {
            return right.Multiply(multiplier);
        }

        public static Money<TCurrency> operator *(Money<TCurrency> left, decimal multiplier)
        {
            return left.Multiply(multiplier);
        }

        public Money<TCurrency> Multiply(decimal multiplier)
        {
            return new Money<TCurrency>(multiplier * Amount);
        }
    }

    /// <content>
    /// Implements the <c>op_Division</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator /(decimal divisor, Money<TCurrency> right)
        {
            return right.Divide(divisor);
        }

        public static Money<TCurrency> operator /(Money<TCurrency> left, decimal divisor)
        {
            return left.Divide(divisor);
        }

        public Money<TCurrency> Divide(decimal divisor)
        {
            return new Money<TCurrency>(Amount / divisor);
        }
    }

    /// <content>
    /// Implements the <c>op_UnaryNegation</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> money)
        {
            return money.Negate();
        }

        public Money<TCurrency> Negate()
        {
            return new Money<TCurrency>(-Amount);
        }
    }
}
