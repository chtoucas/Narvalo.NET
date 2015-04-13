// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the Japanese Yen currency unit.
    /// </summary>
    public static class Yen
    {
        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of("JPY");

        /// <summary>
        /// Gets the Japanese Yen currency.
        /// </summary>
        /// <value>The Japanese Yen currency.</value>
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
