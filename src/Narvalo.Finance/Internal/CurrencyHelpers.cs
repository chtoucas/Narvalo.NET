// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Internal
{
    using System.Globalization;

    // Used to share methods/data between Currency and CurrencyUnit<TCurrency>.
    internal static partial class CurrencyHelpers
    {
        private const char META_CURRENCY_MARK = 'X';

        // The list is automatically generated using data obtained from the SNV website.
        public static decimal[] Epsilons => s_Epsilons;

        // The list is automatically generated using data obtained from the SNV website.
        public static uint[] PowersOfTen => s_PowersOfTen;

        public static bool IsMetaCurrency(string code)
        {
            Demand.NotNullOrEmpty(code);

            return code[0] == META_CURRENCY_MARK;
        }

        public static bool IsPseudoCurrency(string code, short? minorUnits)
        {
            Demand.NotNullOrEmpty(code);

            // A pseudo currency is a meta-currency which is not a regional currency.
            // Among meta-currencies, regional currencies are the only one that do have
            // a minor currency unit.
            return IsMetaCurrency(code) && !minorUnits.HasValue;
        }

        public static bool IsNativeTo(string code, CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, nameof(cultureInfo));

            if (cultureInfo.IsNeutralCulture) { return false; }

            var ri = new RegionInfo(cultureInfo.Name);

            return ri.ISOCurrencySymbol == code;
        }
    }
}
