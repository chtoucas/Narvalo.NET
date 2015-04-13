// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the currency specifically reserved for testing purposes.
    /// </summary>
    public static class TestCurrency
    {
        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of("XTS");

        /// <summary>
        /// Gets the currency specifically reserved for testing purposes.
        /// </summary>
        /// <value>The currency specifically reserved for testing purposes.</value>
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
