// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using Narvalo.Finance.Utilities;

    // Only used to make the methods look (to us) like ordinary static methods of the Currency class.
    using static Narvalo.Finance.Currency;

    public static class CurrencyFactory
    {
        public static Currency? TryCreate(string code)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            short? minorUnits;
            if (!Codes.TryGetValue(code, out minorUnits)) { return null; }

            return new Currency(code, minorUnits);
        }

        public static Currency? TryCreate(string code, CurrencyTypes types)
        {
            Require.NotNull(code, nameof(code));
            Sentinel.Expect.CurrencyCode(code);

            if (types.Contains(CurrencyTypes.Active))
            {
                short? minorUnits;
                if (Codes.TryGetValue(code, out minorUnits))
                {
                    return new Currency(code, minorUnits);
                }
            }

            if (types.Contains(CurrencyTypes.Custom))
            {
                short? minorUnits;
                if (UserCodes.TryGetValue(code, out minorUnits))
                {
                    return new Currency(code, minorUnits);
                }
            }

            // At last, we look for a withdrawn currency.
            if (types.Contains(CurrencyTypes.Withdrawn) && WithdrawnCodes.Contains(code))
            {
                // For withdrawn currencies, ISO 4217 does not provide any information
                // concerning the minor units. See the property HasMinorCurrency for more info.
                return new Currency(code, UnknownMinorUnits);
            }

            return null;
        }
    }
}
