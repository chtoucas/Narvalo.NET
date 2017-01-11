// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    using Narvalo.Finance.Utilities;

    public static partial class PennyCalculator
    {
        public static Moneypenny Max(Moneypenny penny1, Moneypenny penny2) => penny1 >= penny2 ? penny1 : penny2;

        public static Moneypenny Min(Moneypenny penny1, Moneypenny penny2) => penny1 <= penny2 ? penny1 : penny2;

        public static Moneypenny Clamp(Moneypenny penny, Moneypenny min, Moneypenny max)
        {
            Require.True(min <= max, nameof(min));

            return penny < min ? min : (penny > max ? max : penny);
        }
    }

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
