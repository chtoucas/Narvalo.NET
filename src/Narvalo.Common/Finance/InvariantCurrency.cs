// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the "Special Drawing Right" currency unit.
    /// </summary>
    public static class InvariantCurrency
    {
        public const string Code = "XDR";

        private static readonly Currency s_Currency = new Currency(Code);

        /// <summary>
        /// Gets the "Special Drawing Right" currency.
        /// </summary>
        /// <remarks>This is the currency used by the invariant culture.</remarks>
        /// <value>The "Special Drawing Right" currency.</value>
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
