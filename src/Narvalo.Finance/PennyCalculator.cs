// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Narvalo.Finance.Utilities;

    // Standard binary math operators.
    public static partial class PennyCalculator
    {
        public static Moneypenny Add(Moneypenny left, Moneypenny right)
        {
            left.ThrowIfCurrencyMismatch(right, nameof(right));

            if (left.Amount == 0L) { return right; }
            if (right.Amount == 0L) { return left; }
            return new Moneypenny(checked(left.Amount + right.Amount), left.Currency);
        }

        public static Moneypenny Add(Moneypenny penny, long amount)
        {
            if (amount == 0L) { return penny; }
            return new Moneypenny(checked(penny.Amount + amount), penny.Currency);
        }

        public static Moneypenny Subtract(Moneypenny left, Moneypenny right)
        {
            left.ThrowIfCurrencyMismatch(right, nameof(right));

            if (left.Amount == 0L) { return right.Negate(); }
            if (right.Amount == 0L) { return left; }
            return new Moneypenny(checked(right.Amount - left.Amount), left.Currency);
        }

        public static Moneypenny Subtract(Moneypenny penny, long amount) => Add(penny, -amount);

        public static Moneypenny Subtract(long amount, Moneypenny penny)
            => new Moneypenny(checked(amount - penny.Amount), penny.Currency);

        public static Moneypenny Multiply(Moneypenny penny, long multiplier)
            => new Moneypenny(checked(multiplier * penny.Amount), penny.Currency);

        public static Moneypenny Divide(Moneypenny dividend, long divisor)
            => new Moneypenny(checked(dividend.Amount / divisor), dividend.Currency);

        public static Moneypenny Remainder(Moneypenny dividend, long divisor)
            => new Moneypenny(checked(dividend.Amount % divisor), dividend.Currency);
    }

    // Standard binary math operators under which the Moneypenny type is not closed.
    // NB: Decimal multiplication is always checked.
    public static partial class PennyCalculator
    {
        public static Money Multiply(Moneypenny penny, decimal multiplier)
            => new Money(multiplier * penny.Amount, penny.Currency);

        // This division returns a decimal (we lost the currency unit).
        // It is a lot like computing a percentage (if multiplied by 100, of course).
        public static decimal Divide(Moneypenny dividend, Moneypenny divisor)
            => dividend.Amount / (decimal)divisor.Amount;

        public static Money Divide(Moneypenny dividend, decimal divisor)
            => new Money(dividend.Amount / divisor, dividend.Currency);

        public static Money Modulus(Moneypenny dividend, decimal divisor)
            => new Money(dividend.Amount % divisor, dividend.Currency);
    }

    // Other math operators.
    public static partial class PennyCalculator
    {
        public static Moneypenny Abs(Moneypenny penny) => penny.IsPositiveOrZero ? penny : penny.Negate();

        public static Moneypenny Max(Moneypenny penny1, Moneypenny penny2) => penny1 >= penny2 ? penny1 : penny2;

        public static Moneypenny Min(Moneypenny penny1, Moneypenny penny2) => penny1 <= penny2 ? penny1 : penny2;

        public static Moneypenny Clamp(Moneypenny penny, Moneypenny min, Moneypenny max)
        {
            Require.True(min <= max, nameof(min));

            return penny < min ? min : (penny > max ? max : penny);
        }

        // Divide+Remainder aka DivRem.
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Div", Justification = "[Intentionally] Math.DivRem().")]
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#", Justification = "[Intentionally] Math.DivRem().")]
        public static Moneypenny DivRem(Moneypenny dividend, long divisor, out Moneypenny remainder)
        {
            long rem;
            long q = Integer.DivRem(dividend.Amount, divisor, out rem);
            remainder = new Moneypenny(rem, dividend.Currency);
            return new Moneypenny(q, dividend.Currency);
        }
    }

    // LINQ-like Sum().
    public static partial class PennyCalculator
    {
        public static Moneypenny Sum(this IEnumerable<Moneypenny> pennies)
        {
            Require.NotNull(pennies, nameof(pennies));

            using (IEnumerator<Moneypenny> it = pennies.GetEnumerator())
            {
                if (!it.MoveNext()) { goto EMPTY_COLLECTION; }

                var pny = it.Current;
                var currency = pny.Currency;
                long sum = pny.Amount;

                while (it.MoveNext())
                {
                    pny = it.Current;

                    MoneyChecker.ThrowIfCurrencyMismatch(pny, currency);

                    checked { sum += pny.Amount; }
                }

                return new Moneypenny(sum, currency);
            }

            EMPTY_COLLECTION:
            return Moneypenny.Zero(Currency.None);
        }

        public static Moneypenny Sum(this IEnumerable<Moneypenny?> pennies)
        {
            Require.NotNull(pennies, nameof(pennies));

            using (IEnumerator<Moneypenny?> it = pennies.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    Moneypenny? item = it.Current;
                    if (!item.HasValue) { continue; }

                    var pny = item.Value;
                    var currency = pny.Currency;
                    long sum = pny.Amount;

                    while (it.MoveNext())
                    {
                        item = it.Current;

                        if (item.HasValue)
                        {
                            pny = item.Value;

                            MoneyChecker.ThrowIfCurrencyMismatch(pny, currency);

                            checked { sum += pny.Amount; }
                        }
                    }

                    return new Moneypenny(sum, currency);
                }
            }

            return Moneypenny.Zero(Currency.None);
        }
    }
}
