// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the pseudo-currency for gold.
    /// </summary>
    public static class Gold
    {
        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of("XAU");

        /// <summary>
        /// Gets the pseudo-currency for gold.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AU.</remarks>
        /// <value>The pseudo-currency for gold.</value>
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
