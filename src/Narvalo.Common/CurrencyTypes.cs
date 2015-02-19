// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    [Flags]
    [Serializable]
    public enum CurrencyTypes
    {
        CurrentCurrencies = 1 << 0,

        LegacyCurrencies = 1 << 1,

        AllCurrencies = CurrentCurrencies | LegacyCurrencies
    }
}
