// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    // Adapt the operators found in the class Math.
    public static partial class MoneyMath
    {
        public static Money Abs(Money money) => money.IsPositiveOrZero ? money : money.Negate();

        public static int Sign(Money money) => money < 0 ? -1 : (money > 0 ? 1 : 0);

        public static Money Max(Money money1, Money money2) => money1 >= money2 ? money1 : money2;

        public static Money Min(Money money1, Money money2) => money1 <= money2 ? money1 : money2;

        public static Money Clamp(Money money, Money min, Money max)
        {
            Require.True(min <= max, nameof(min));

            return money < min ? min : (money > max ? max : money);
        }

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Math.DivRem().")]
        public static Money DivRem(Money dividend, decimal divisor, out Money remainder)
        {
            // REVIEW: remainder = dividend % divisor is slower for integers. What about decimals?
            // > var q = dividend.Divide(divisor);
            // > remainder = dividend.Remainder(divisor);
            // For doubles, .NET uses:
            // Modulus = (Math.Abs(dividend) - (Math.Abs(divisor)
            //   * (Math.Floor(Math.Abs(dividend) / Math.Abs(divisor)))))
            //   * Math.Sign(dividend)
            decimal q = dividend.Amount / divisor;
            decimal rem = dividend.Amount - q * divisor;
            remainder = new Money(rem, dividend.Currency);
            return new Money(q, dividend.Currency);
        }
    }

    public static partial class MoneyMath
    {
        public static decimal Ceiling(Money money) => Round(money, Math.Ceiling);

        public static decimal Floor(Money money) => Round(money, Math.Floor);

        public static decimal Truncate(Money money) => Round(money, Math.Truncate);

        public static decimal Round(Money money) => Round(money, Math.Round);

        public static decimal Round(Money money, MidpointRounding mode)
            => Round(money, _ => Math.Round(_, mode));

        public static decimal Round(Money money, int decimalPlaces)
            => Round(money, decimalPlaces, MidpointRounding.ToEven);

        public static decimal Round(Money money, int decimalPlaces, MidpointRounding mode)
        {
            if (money.IsNormalized && money.Currency.DecimalPlaces == decimalPlaces) { return money.Amount; }
            return Math.Round(money.Amount, decimalPlaces, mode);
        }

        private static decimal Round(Money money, Func<decimal, decimal> round)
        {
            Demand.NotNull(round);
            if (money.IsNormalized && money.Currency.DecimalPlaces == 0) { return money.Amount; }
            return round.Invoke(money.Amount);
        }
    }
}
