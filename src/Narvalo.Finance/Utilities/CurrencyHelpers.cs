// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Utilities
{
    // Used to share methods between Currency and CurrencyUnit<TCurrency>.
    internal static class CurrencyHelpers
    {
        private const char META_CURRENCY_MARK = 'X';

        public static bool IsMetaCurrency(string code)
        {
            Demand.NotNullOrEmpty(code);

            return code[0] == META_CURRENCY_MARK;
        }

        public static bool IsPseudoCurrency(string code, short? minorUnits)
        {
            Demand.NotNullOrEmpty(code);

            // Pseudo currencies do not have a minor currency unit.
            return IsMetaCurrency(code) && !minorUnits.HasValue;
        }
    }
}
