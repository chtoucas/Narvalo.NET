// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Internal;
    using Narvalo.Finance.Properties;

    [DebuggerDisplay("{{ToString()}}")]
    [SuppressMessage("Gendarme.Rules.Design", "ProvideAlternativeNamesForOperatorOverloadsRule",
        Justification = "[Intentionally] We do provide an alternate method but we call it CompareTo.")]
    [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
        Justification = "[Intentionally] Operator's overloads must be static.")]
    public partial struct Money<TCurrency>
        : IEquatable<Money<TCurrency>>, IComparable, IComparable<Money<TCurrency>>, IFormattable
        where TCurrency : Currency, new()
    {
        // IMPORTANT: This static field MUST remain the first to be initialized before the others.
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_Currency = CurrencyActivator<TCurrency>.CreateInstance();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0m);

        private readonly decimal _amount;

        public Money(decimal amount)
        {
            _amount = amount;
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
            if (!(value.Currency == s_Currency))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(value.Amount);
        }

        public static implicit operator Money(Money<TCurrency> value)
        {
            return new Money(value.Amount, s_Currency);
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

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Money<TCurrency> other)
        {
            return Amount == other.Amount;
        }

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
                hash = (31 * hash) + ((long)Amount).GetHashCode();
                hash = (31 * hash) + s_Currency.GetHashCode();
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
            return MoneyFormatter.Format(Amount, s_Currency);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return MoneyFormatter.Format(Amount, format, formatProvider);
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

        public static bool operator <=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.CompareTo(right) >= 0;
        }

        public int CompareTo(Money<TCurrency> other)
        {
            return Amount.CompareTo(other.Amount);
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Money<TCurrency>))
            {
                throw new ArgumentException(Strings.Money_ArgIsNotMoney);
            }

            return CompareTo((Money<TCurrency>)obj);
        }
    }

    /// <content>
    /// Overrides the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.Add(right);
        }

        public Money<TCurrency> Add(Money<TCurrency> money)
        {
            var amount = checked(Amount + money.Amount);

            return new Money<TCurrency>(amount);
        }
    }

    /// <content>
    /// Overrides the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> left, Money<TCurrency> right)
        {
            return left.Subtract(right);
        }

        public Money<TCurrency> Subtract(Money<TCurrency> money)
        {
            return new Money<TCurrency>(Amount - money.Amount);
        }
    }

    /// <content>
    /// Overrides the <c>op_Multiply</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator *(decimal multiplier, Money<TCurrency> money)
        {
            return money.Multiply(multiplier);
        }

        public static Money<TCurrency> operator *(Money<TCurrency> money, decimal multiplier)
        {
            return money.Multiply(multiplier);
        }

        public Money<TCurrency> Multiply(decimal multiplier)
        {
            var amount = checked(multiplier * Amount);

            return new Money<TCurrency>(amount);
        }
    }

    /// <content>
    /// Overrides the <c>op_Division</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator /(Money<TCurrency> money, decimal divisor)
        {
            Contract.Requires(divisor != 0m);

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

    /// <content>
    /// Overrides the <c>op_Modulus</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator %(Money<TCurrency> money, decimal divisor)
        {
            Contract.Requires(divisor != 0m);

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

    /// <content>
    /// Overrides the <c>op_UnaryNegation</c> operator.
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

    /// <content>
    /// Overrides the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> money)
        {
            return money;
        }

        public Money<TCurrency> Plus()
        {
            return this;
        }
    }
}
