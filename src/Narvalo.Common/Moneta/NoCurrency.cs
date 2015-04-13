// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the pseudo-currency for transactions where no currency is involved.
    /// </summary>
    public static class NoCurrency
    {
        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of("XXX");

        /// <summary>
        /// Gets the pseudo-currency for transactions where no currency is involved.
        /// </summary>
        /// <value>The pseudo-currency for transactions where no currency is involved.</value>
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
