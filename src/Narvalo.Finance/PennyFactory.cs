// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;

    using Narvalo.Finance.Rounding;

    public static class PennyFactory
    {
        #region From minor.

        public static Moneypenny CreateFromMinor(decimal minor, Currency currency, MidpointRounding mode)
        {
            Expect.True(currency.HasFixedDecimalPlaces);

            var penny = TryCreateFromMinor(minor, currency, mode);
            if (!penny.HasValue) { throw new NotSupportedException("XXX"); }

            return penny.Value;
        }

        public static Moneypenny CreateFromMinor(decimal minor, Currency currency, IRoundingAdjuster adjuster)
        {
            Expect.True(currency.HasFixedDecimalPlaces);
            Expect.NotNull(adjuster);

            var penny = TryCreateFromMinor(minor, currency, adjuster);
            if (!penny.HasValue) { throw new NotSupportedException("XXX"); }

            return penny.Value;
        }

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

        #endregion

        #region From major.

        public static Moneypenny CreateFromMajor(decimal major, Currency currency, MidpointRounding mode)
            => CreateFromMinor(currency.ConvertToMinor(major), currency, mode);

        public static Moneypenny CreateFromMajor(decimal major, Currency currency, IRoundingAdjuster adjuster)
            => CreateFromMinor(currency.ConvertToMinor(major), currency, adjuster);

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, MidpointRounding mode)
            => TryCreateFromMinor(currency.ConvertToMinor(major), currency, mode);

        public static Moneypenny? TryCreateFromMajor(decimal major, Currency currency, IRoundingAdjuster adjuster)
            => TryCreateFromMinor(currency.ConvertToMinor(major), currency, adjuster);

        #endregion

        #region From Money.

        public static Moneypenny Create(Money money, MidpointRounding mode)
        {
            Expect.True(money.IsRoundable);

            var penny = TryCreate(money, mode);
            if (!penny.HasValue) { throw new NotSupportedException("XXX"); }

            return penny.Value;
        }

        public static Moneypenny Create(Money money, IRoundingAdjuster adjuster)
        {
            Expect.True(money.IsRoundable);
            Expect.NotNull(adjuster);

            var penny = TryCreate(money, adjuster);
            if (!penny.HasValue) { throw new NotSupportedException("XXX"); }

            return penny.Value;
        }

        public static Moneypenny? TryCreate(Money money, MidpointRounding mode)
        {
            Require.True(money.IsRoundable, nameof(money));

            long? minor = money.Normalize(mode).ToLongMinor();
            if (!minor.HasValue) { return null; }

            return new Moneypenny(minor.Value, money.Currency);
        }

        public static Moneypenny? TryCreate(Money money, IRoundingAdjuster adjuster)
        {
            Require.True(money.IsRoundable, nameof(money));
            Expect.NotNull(adjuster);

            long? minor = money.Normalize(adjuster).ToLongMinor();
            if (!minor.HasValue) { return null; }

            return new Moneypenny(minor.Value, money.Currency);
        }

        #endregion
    }
}
