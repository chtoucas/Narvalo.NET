// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Collections.Generic;

    public sealed partial class InMemoryCurrencyProvider : ICurrencyProvider
    {
        public HashSet<string> CurrencyCodes
        {
            get { return s_CurrencyCodeSet; }
        }

        public CurrencyInfoCollection GetCurrencies(CurrencyTypes types)
        {
            if (types.Contains(CurrencyTypes.AllCurrencies))
            {
                var current = s_CurrentCurrencies.Value;
                var legacy = s_LegacyCurrencies.Value;
                var capacity = current.Count + legacy.Count;

                var list = new List<CurrencyInfo>(capacity);
                list.AddRange(current);
                list.AddRange(legacy);

                return new CurrencyInfoCollection(list);
            }
            else if (types.Contains(CurrencyTypes.CurrentCurrencies))
            {
                return s_CurrentCurrencies.Value;
            }
            else
            {
                return s_LegacyCurrencies.Value;
            }
        }
    }
}
