// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    internal static class CurrencyHelpers
    {
        private const char META_CURRENCY_MARK = 'X';

        // TODO: What about EUR, CFP... for this, is it enough to check the country code too
        // (the first two letters)?
        public static bool IsMetaCurrency(string currencyCode)
        {
            Demand.NotNullOrEmpty(currencyCode);

            return currencyCode[0] == META_CURRENCY_MARK;
        }
    }
}
