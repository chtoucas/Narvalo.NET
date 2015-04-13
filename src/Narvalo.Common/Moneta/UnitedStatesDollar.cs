// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the "United States Dollar" currency unit.
    /// </summary>
    public static class UnitedStatesDollar
    {
        internal const string Code = "USD";

        private static readonly Currency s_Currency = Narvalo.Moneta.Currency.Of(Code);

        /// <summary>
        /// Gets the "United States Dollar" currency.
        /// </summary>
        /// <value>The "United States Dollar" currency.</value>
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
