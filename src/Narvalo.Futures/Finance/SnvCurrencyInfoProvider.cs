// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Collections.Generic;

    public sealed partial class SnvCurrencyInfoProvider : ICurrencyInfoProvider
    {
        public CurrencyInfoCollection GetCurrencies(CurrencyTypes types)
        {
            switch (types)
            {
                case CurrencyTypes.ISO:
                    var current = s_CurrentCurrencies.Value;
                    var legacy = s_LegacyCurrencies.Value;
                    var capacity = current.Count + legacy.Count;

                    var list = new List<CurrencyInfo>(capacity);
                    list.AddRange(current);
                    list.AddRange(legacy);

                    return new CurrencyInfoCollection(list);

                case CurrencyTypes.Withdrawn:
                    return s_LegacyCurrencies.Value;

                case CurrencyTypes.Active:
                default:
                    return s_CurrentCurrencies.Value;
            }
        }
    }
}
