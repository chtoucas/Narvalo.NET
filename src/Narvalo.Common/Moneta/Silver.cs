// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the pseudo-currency for silver.
    /// </summary>
    public static class Silver
    {
        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of("XAG");

        /// <summary>
        /// Gets the pseudo-currency for silver.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: AG.</remarks>
        /// <value>The pseudo-currency for silver.</value>
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
