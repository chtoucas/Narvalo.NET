// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    public partial struct Money<TCurrency>
        : IEquatable<Money<TCurrency>>, IComparable<Money<TCurrency>>, IComparable, IFormattable
        where TCurrency : Currency
    {
        // IMPORTANT: This static field MUST remain first to be initialized before the others.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_Currency = CurrencyActivator<TCurrency>.CreateInstance();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0m);

        public Money(decimal amount)
        {
            Amount = amount;
        }

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes",
            Justification = "[Ignore] There is no such thing as a generic static property on a non-generic type.")]
        public static Money<TCurrency> Zero { get { return s_Zero; } }

        public decimal Amount { get; }

        public static explicit operator Money<TCurrency>(Money value)
        {
            if (!(value.Currency == s_Currency))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(value.Amount);
        }

        public static explicit operator Money(Money<TCurrency> value)
            => new Money(value.Amount, s_Currency);
    }

    // Implements the IEquatable interfaces.
    public partial struct Money<TCurrency>
    {
        public static bool operator ==(Money<TCurrency> left, Money<TCurrency> right) => left.Equals(right);

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right) => !left.Equals(right);

        public bool Equals(Money<TCurrency> other) => Amount == other.Amount;

        public override bool Equals(object obj)
        {
            if (!(obj is Money<TCurrency>))
            {
                return false;
            }

            return Equals((Money<TCurrency>)obj);
        }

        public override int GetHashCode()
        {
            // TODO: Cache the hash code for s_Currency.
            unchecked
            {
                int hash = 17;
                hash = 31 * hash + ((long)Amount).GetHashCode();
                hash = 31 * hash + s_Currency.GetHashCode();
                return hash;
            }
        }
    }

    // Implements the IFormattable interface.
    public partial struct Money<TCurrency>
    {
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString() => MoneyFormatter.Format(Amount, s_Currency);

        public string ToString(string format, IFormatProvider formatProvider)
            => MoneyFormatter.Format(Amount, format, formatProvider);
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
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Money<TCurrency>))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney, nameof(obj));
            }

            return CompareTo((Money<TCurrency>)obj);
        }
    }

    // Overrides the op_Addition operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right)
            => left.Add(right);

        public Money<TCurrency> Add(Money<TCurrency> money)
        {
            decimal amount = checked(Amount + money.Amount);

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

        public Money<TCurrency> Subtract(Money<TCurrency> money) => new Money<TCurrency>(Amount - money.Amount);

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
        public static Money<TCurrency> operator -(Money<TCurrency> money)
        {
            return money.Negate();
        }

        public Money<TCurrency> Negate()
        {
            return new Money<TCurrency>(-Amount);
        }
    }

    // Overrides the op_UnaryPlus operator.
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> money) => money;

        public Money<TCurrency> Plus() => this;
    }
}
