// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System;

    [Flags]
    public enum CurrencyTypes
    {
        CurrentCurrencies = 1 << 0,

        LegacyCurrencies = 1 << 1,

        AllCurrencies = CurrentCurrencies | LegacyCurrencies
    }

    public static class CurrencyTypesExtensions
    {
        public static bool Contains(this CurrencyTypes @this, CurrencyTypes value)
        {
            return (@this & value) != 0;
        }
    }
}
