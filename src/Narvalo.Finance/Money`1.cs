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
        Justification = "[Intentionally] We do provide the alternate but we called it CompareTo.")]
    [SuppressMessage("Gendarme.Rules.Design.Generic", "DoNotDeclareStaticMembersOnGenericTypesRule",
        Justification = "[Intentionally] Operator's overloads must be static.")]
    public partial struct Money<TCurrency>
        : IEquatable<Money<TCurrency>>, IEquatable<Money>,
        IComparable, IComparable<Money<TCurrency>>, IComparable<Money>,
        IFormattable
        where TCurrency : Currency
    {
        // IMPORTANT: Keep this field 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly TCurrency s_Currency = CurrencyActivator<TCurrency>.CreateInstance();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Money<TCurrency> s_Zero = new Money<TCurrency>(0m);

        private readonly Money _inner;

        public Money(decimal amount)
        {
            _inner = new Money(amount, s_Currency);
        }

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUncalledPrivateCodeRule",
            Justification = "[Ignore] This constructor is called many times from Money.")]
        internal Money(Money inner)
        {
            _inner = inner;
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

        public decimal Amount { get { return _inner.Amount; } }

        internal Money Inner { get { return _inner; } }

        public static explicit operator Money<TCurrency>(Money value)
        {
            if (!(value.Currency == s_Currency))
            {
                throw new InvalidCastException();
            }

            return new Money<TCurrency>(value);
        }

        public static explicit operator Money(Money<TCurrency> value)
        {
            return value._inner;
        }
    }

    /// <content>
    /// Implements the <c>IEquatable</c> interfaces.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static bool operator ==(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.IsEqual(left, right);
        }

        //public static bool operator ==(Money<TCurrency> left, Money right)
        //{
        //    return Money.IsEqual(left, right);
        //}

        //public static bool operator ==(Money left, Money<TCurrency> right)
        //{
        //    return Money.IsEqual(right, left);
        //}

        public static bool operator !=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return !Money.IsEqual(left, right);
        }

        //public static bool operator !=(Money<TCurrency> left, Money right)
        //{
        //    return !Money.IsEqual(left, right);
        //}

        //public static bool operator !=(Money left, Money<TCurrency> right)
        //{
        //    return !Money.IsEqual(right, left);
        //}

        public bool Equals(Money<TCurrency> other)
        {
            return Money.IsEqual(this, other);
        }

        public bool Equals(Money other)
        {
            return Money.IsEqual(this, other);
        }

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnneededUnboxingRule",
            Justification = "[Ignore] Unboxing twice is necessary here.")]
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Money<TCurrency>)
            {
                return Money.IsEqual(this, (Money<TCurrency>)obj);
            }

            if (obj is Money)
            {
                return Money.IsEqual(this, (Money)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _inner.GetHashCode();
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
            return _inner.ToString();
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _inner.ToString(format, formatProvider);
        }
    }

    /// <content>
    /// Implements the <c>IComparable</c> interfaces.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static bool operator <(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) < 0;
        }

        public static bool operator <(Money<TCurrency> left, Money right)
        {
            return Money.Compare(left, right) < 0;
        }

        public static bool operator <(Money left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) < 0;
        }

        public static bool operator <=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) <= 0;
        }

        public static bool operator <=(Money<TCurrency> left, Money right)
        {
            return Money.Compare(left, right) <= 0;
        }

        public static bool operator <=(Money left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) <= 0;
        }

        public static bool operator >(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) > 0;
        }

        public static bool operator >(Money<TCurrency> left, Money right)
        {
            return Money.Compare(left, right) > 0;
        }

        public static bool operator >(Money left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) > 0;
        }

        public static bool operator >=(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) >= 0;
        }

        public static bool operator >=(Money<TCurrency> left, Money right)
        {
            return Money.Compare(left, right) >= 0;
        }

        public static bool operator >=(Money left, Money<TCurrency> right)
        {
            return Money.Compare(left, right) >= 0;
        }

        public int CompareTo(Money<TCurrency> other)
        {
            return Money.Compare(this, other);
        }

        public int CompareTo(Money other)
        {
            return Money.Compare(this, other);
        }

        [SuppressMessage("Gendarme.Rules.Performance", "AvoidUnneededUnboxingRule",
            Justification = "[Ignore] Unboxing twice is necessary here.")]
        int IComparable.CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj is Money<TCurrency>)
            {
                return Money.Compare(this, (Money<TCurrency>)obj);
            }

            if (obj is Money)
            {
                return Money.Compare(this, (Money)obj);
            }

            throw new ArgumentException(Strings.Money_ArgIsNotMoney);
        }
    }

    /// <content>
    /// Overrides the <c>op_Addition</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Add(left, right);
        }

        public static Money<TCurrency> operator +(Money<TCurrency> left, Money right)
        {
            return Money.Add(left, right);
        }

        public static Money<TCurrency> operator +(Money left, Money<TCurrency> right)
        {
            return Money.Add(right, left);
        }

        public Money<TCurrency> Add(Money<TCurrency> other)
        {
            return Money.Add(this, other);
        }

        public Money<TCurrency> Add(Money money)
        {
            return Money.Add(this, money);
        }

        public Money<TCurrency> Add(decimal amount)
        {
            return new Money<TCurrency>(Inner.Add(amount));
        }
    }

    /// <content>
    /// Overrides the <c>op_Subtraction</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> left, Money<TCurrency> right)
        {
            return Money.Subtract(left, right);
        }

        public static Money<TCurrency> operator -(Money<TCurrency> left, Money right)
        {
            return Money.Subtract(left, right);
        }

        public static Money<TCurrency> operator -(Money left, Money<TCurrency> right)
        {
            return Money.Subtract(left, right);
        }

        public Money<TCurrency> Subtract(Money<TCurrency> other)
        {
            return Money.Subtract(this, other);
        }

        public Money<TCurrency> Subtract(Money money)
        {
            return Money.Subtract(this, money);
        }

        public Money<TCurrency> Subtract(decimal amount)
        {
            return new Money<TCurrency>(Inner.Subtract(amount));
        }
    }

    /// <content>
    /// Overrides the <c>op_Multiply</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator *(decimal multiplier, Money<TCurrency> money)
        {
            return Money.Multiply(multiplier, money);
        }

        public static Money<TCurrency> operator *(Money<TCurrency> money, decimal multiplier)
        {
            return Money.Multiply(multiplier, money);
        }

        public Money<TCurrency> Multiply(decimal multiplier)
        {
            return Money.Multiply(multiplier, this);
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

            return Money.Divide(money, divisor);
        }

        public Money<TCurrency> Divide(decimal divisor)
        {
            Contract.Requires(divisor != 0m);

            return Money.Divide(this, divisor);
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

            return Money.Remainder(money, divisor);
        }

        public Money<TCurrency> Remainder(decimal divisor)
        {
            Contract.Requires(divisor != 0m);

            return Money.Remainder(this, divisor);
        }
    }

    /// <content>
    /// Overrides the <c>op_UnaryNegation</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator -(Money<TCurrency> money)
        {
            return Money.Negate(money);
        }

        public Money<TCurrency> Negate()
        {
            return Money.Negate(this);
        }
    }

    /// <content>
    /// Overrides the <c>op_UnaryPlus</c> operator.
    /// </content>
    public partial struct Money<TCurrency>
    {
        public static Money<TCurrency> operator +(Money<TCurrency> money)
        {
            return Money.Plus(money);
        }

        public Money<TCurrency> Plus()
        {
            return Money.Plus(this);
        }
    }
}
