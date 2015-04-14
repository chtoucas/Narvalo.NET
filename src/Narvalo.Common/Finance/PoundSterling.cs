// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the (British) "Pound Sterling" currency unit.
    /// </summary>
    public static class PoundSterling
    {
        internal const string CurrencyCode = "GBP";

        private static readonly Currency s_Currency = new Currency(CurrencyCode);

        /// <summary>
        /// Gets the (British) "Pound Sterling" currency.
        /// </summary>
        /// <value>The (British) "Pound Sterling" currency.</value>
        public static Currency Currency
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);

                return s_Currency;
            }
        }
    }
}
