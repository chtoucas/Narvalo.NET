// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Legacy
{
    using System.Collections.Generic;

    public sealed partial class InMemoryCurrencyInfoProvider : ICurrencyInfoProvider
    {
        public CurrencyInfoCollection GetCurrencies(CurrencyTypes types)
        {
            switch (types)
            {
                case CurrencyTypes.AllCurrencies:
                    var current = s_CurrentCurrencies.Value;
                    var legacy = s_LegacyCurrencies.Value;
                    var capacity = current.Count + legacy.Count;

                    var list = new List<CurrencyInfo>(capacity);
                    list.AddRange(current);
                    list.AddRange(legacy);

                    return new CurrencyInfoCollection(list);

                case CurrencyTypes.LegacyCurrencies:
                    return s_LegacyCurrencies.Value;

                case CurrencyTypes.CurrentCurrencies:
                default:
                    return s_CurrentCurrencies.Value;
            }
        }
    }
}
