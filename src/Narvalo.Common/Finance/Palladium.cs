// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the pseudo-currency for palladium.
    /// </summary>
    public static class Palladium
    {
        internal const string Code = "XPD";

        private static readonly Currency s_Currency = new Currency(Code);

        /// <summary>
        /// Gets the pseudo-currency for palladium.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PD.</remarks>
        /// <value>The pseudo-currency for palladium.</value>
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
