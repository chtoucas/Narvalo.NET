// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    using Narvalo.Finance.Properties;

    // FIXME: Overflow operations.
    // REVIEW: IConvertible?
    [DebuggerDisplay("{{ToString()}}")]
    [SuppressMessage("Gendarme.Rules.Design", "ProvideAlternativeNamesForOperatorOverloadsRule",
        Justification = "[Intentionally] We do provide the alternate but we called it CompareTo.")]
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
            return IsEqual(left, right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !IsEqual(left, right);
        }

        public static bool IsEqual<TCurrency>(Money<TCurrency> money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            return money1.Amount == money2.Amount;
        }

        public static bool IsEqual<TCurrency>(Money<TCurrency> money1, Money money2)
            where TCurrency : Currency
        {
            return IsEqual(money1.Inner, money2);
        }

        public static bool IsEqual(Money money1, Money money2)
        {
            return money1.Amount == money2.Amount && money1.Currency.Equals(money2.Currency);
        }

        public bool Equals(Money other)
        {
            return IsEqual(this, other);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Money))
            {
                return false;
            }

            return IsEqual(this, (Money)obj);
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
        /// <inheritdoc cref="Object.ToString" />
        public override string ToString()
        {
            return Format.CurrentCulture("{0} {1:F2}", Currency.Code, Amount);
        }

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
            return Compare(left, right) < 0;
        }

        public static bool operator <=(Money left, Money right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator >(Money left, Money right)
        {
            return Compare(left, right) > 0;
        }

        public static bool operator >=(Money left, Money right)
        {
            return Compare(left, right) >= 0;
        }

        public static int Compare<TCurrency>(Money<TCurrency> money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            return money1.Amount.CompareTo(money2.Amount);
        }

        public static int Compare<TCurrency>(Money<TCurrency> money1, Money money2)
            where TCurrency : Currency
        {
            return Compare(money1.Inner, money2);
        }

        public static int Compare<TCurrency>(Money money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            return Compare(money1, money2.Inner);
        }

        public static int Compare(Money money1, Money money2)
        {
            ThrowIfCurrencyMismatch_(money1, money2);

            return money1.Amount.CompareTo(money2.Amount);
        }

        public int CompareTo(Money other)
        {
            return Compare(this, other);
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

            return Compare(this, (Money)obj);
        }
    }

    /// <content>
    /// Overrides the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money left, Money right)
        {
            return Add(left, right);
        }

        public static Money<TCurrency> Add<TCurrency>(Money<TCurrency> money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            var amount = checked(money1.Amount + money2.Amount);

            return new Money<TCurrency>(amount);
        }

        public static Money<TCurrency> Add<TCurrency>(Money<TCurrency> money1, Money money2)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Add(money1.Inner, money2));
        }

        public static Money Add(Money money1, Money money2)
        {
            ThrowIfCurrencyMismatch_(money1, money2);

            var amount = checked(money1.Amount + money2.Amount);

            return new Money(amount, money1.Currency);
        }

        public Money Add(Money money)
        {
            return Add(this, money);
        }

        public Money Add(decimal amount)
        {
            var result = checked(Amount + amount);

            return new Money(result, Currency);
        }
    }

    /// <content>
    /// Overrides the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money left, Money right)
        {
            return Subtract(left, right);
        }

        public static Money<TCurrency> Subtract<TCurrency>(Money<TCurrency> money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(money1.Amount - money2.Amount);
        }

        public static Money<TCurrency> Subtract<TCurrency>(Money<TCurrency> money1, Money money2)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Subtract(money1.Inner, money2));
        }

        public static Money<TCurrency> Subtract<TCurrency>(Money money1, Money<TCurrency> money2)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Subtract(money1, money2.Inner));
        }

        public static Money Subtract(Money money1, Money money2)
        {
            ThrowIfCurrencyMismatch_(money1, money2);

            return new Money(money1.Amount - money2.Amount, money1.Currency);
        }

        public Money Subtract(Money money)
        {
            return Subtract(this, money);
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
            return Multiply(multiplier, money);
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            return Multiply(multiplier, money);
        }

        public static Money<TCurrency> Multiply<TCurrency>(decimal multiplier, Money<TCurrency> money)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Multiply(multiplier, money.Inner));
        }

        public static Money Multiply(decimal multiplier, Money money)
        {
            var amount = checked(multiplier * money.Amount);

            return new Money(amount, money.Currency);
        }

        public Money Multiply(decimal multiplier)
        {
            return Multiply(multiplier, this);
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

            return Divide(money, divisor);
        }

        public static Money<TCurrency> Divide<TCurrency>(Money<TCurrency> money, decimal divisor)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Divide(money.Inner, divisor));
        }

        public static Money Divide(Money money, decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money(money.Amount / divisor, money.Currency);
        }

        public Money Divide(decimal divisor)
        {
            Contract.Requires(divisor != 0m);

            return Divide(this, divisor);
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

            return Remainder(money, divisor);
        }

        public static Money<TCurrency> Remainder<TCurrency>(Money<TCurrency> money, decimal divisor)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(Remainder(money.Inner, divisor));
        }

        public static Money Remainder(Money money, decimal divisor)
        {
            if (divisor == 0m)
            {
                throw new DivideByZeroException();
            }

            return new Money(money.Amount % divisor, money.Currency);
        }

        public Money Remainder(decimal divisor)
        {
            Contract.Requires(divisor != 0m);

            return Remainder(this, divisor);
        }
    }

    /// <content>
    /// Overrides the <c>op_UnaryNegation</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator -(Money money)
        {
            return Negate(money);
        }

        public static Money<TCurrency> Negate<TCurrency>(Money<TCurrency> money)
            where TCurrency : Currency
        {
            return new Money<TCurrency>(-money.Amount);
        }

        public static Money Negate(Money money)
        {
            return new Money(-money.Amount, money.Currency);
        }

        public Money Negate()
        {
            return Negate(this);
        }
    }

    /// <content>
    /// Overrides the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money
    {
        public static Money operator +(Money money)
        {
            return Plus(money);
        }

        public static Money<TCurrency> Plus<TCurrency>(Money<TCurrency> money)
            where TCurrency : Currency
        {
            return money;
        }

        public static Money Plus(Money money)
        {
            return money;
        }

        public Money Plus()
        {
            return Plus(this);
        }
    }
}
