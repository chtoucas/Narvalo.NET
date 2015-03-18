// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Collections.Generic;
    using System.Linq;

    // FIXME: Very bad design.
    public sealed partial class DefaultCurrencyProvider : ICurrencyProvider
    {
        public HashSet<string> CurrencyCodes
        {
            get { return s_CurrencyCodeSet; }
        }

        public IEnumerable<CurrencyInfo> GetCurrencies(CurrencyTypes types)
        {
            if (types.HasFlag(CurrencyTypes.AllCurrencies)) {
                return CurrentCurrencies.Concat(LegacyCurrencies);
            }
            else if (types.HasFlag(CurrencyTypes.CurrentCurrencies)) {
                return CurrentCurrencies;
            }
            else {
                return LegacyCurrencies;
            }
        }
    }
}
