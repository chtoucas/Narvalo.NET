// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Globalization;

    public static class RegionInfoExtensions
    {
        public static bool IsUsing(this RegionInfo @this, Currency currency)
        {
            Require.NotNull(@this, nameof(@this));

            return @this.ISOCurrencySymbol == currency.Code;
        }

        public static bool IsUsing<TCurrency>(this RegionInfo @this, TCurrency currency)
            where TCurrency : CurrencyUnit<TCurrency>
        {
            Require.NotNull(@this, nameof(@this));

            return @this.ISOCurrencySymbol == currency.Code;
        }
    }
}
