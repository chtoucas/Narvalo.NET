// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the pseudo-currency for platinum.
    /// </summary>
    public static class Platinum
    {
        internal const string Code = "XPT";

        private static readonly Currency s_Currency = new Currency(Code);

        /// <summary>
        /// Gets the pseudo-currency for platinum.
        /// </summary>
        /// <remarks>The code for a precious metal is formed after its chemical symbol: PT.</remarks>
        /// <value>The pseudo-currency for platinum.</value>
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
