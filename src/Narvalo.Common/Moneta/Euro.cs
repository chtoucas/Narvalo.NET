// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the "Euro" currency unit.
    /// </summary>
    public static class Euro
    {
        internal const string Code = "EUR";

        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of(Code);

        /// <summary>
        /// Gets the "Euro" currency.
        /// </summary>
        /// <value>The "Euro" currency.</value>
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
