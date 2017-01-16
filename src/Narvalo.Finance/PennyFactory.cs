// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Rounding;

    public static class PennyFactory
    {
        public static Moneypenny? TryCreateFromMinor(decimal minor, Currency currency, MidpointRounding mode)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));

            decimal amount = Math.Round(minor, mode);
            if (amount < Int64.MinValue || amount > Int64.MaxValue) { return null; }

            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny? TryCreateFromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
        {
            Require.True(currency.HasFixedDecimalPlaces, nameof(currency));
            Require.NotNull(adjuster, nameof(adjuster));

            decimal amount = adjuster.Round(minor);
            if (amount < Int64.MinValue || amount > Int64.MaxValue) { return null; }

            return new Moneypenny(Convert.ToInt64(amount), currency);
        }

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, MidpointRounding mode)
        {
            Expect.True(currency.HasFixedDecimalPlaces);

            return TryCreateFromMinor(currency.ConvertToMinor(major), currency, mode);
        }

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, IRoundingAdjuster adjuster)
        {
            Expect.True(currency.HasFixedDecimalPlaces);
            Expect.NotNull(adjuster);

            return TryCreateFromMinor(currency.ConvertToMinor(major), currency, adjuster);
        }

        public static Moneypenny? TryCreate(Money money)
        {
            Require.True(money.IsRounded, nameof(money));

            long? minor = money.ToLongMinor();
            if (!minor.HasValue) { return null; }

            return new Moneypenny(minor.Value, money.Currency);
        }
    }
}
