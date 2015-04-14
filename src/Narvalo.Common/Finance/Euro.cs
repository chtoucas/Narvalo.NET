// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the "Euro" currency unit.
    /// </summary>
    public sealed class Euro : Currency
    {
        internal const string CurrencyCode = "EUR";

        private static readonly Currency s_Currency = new Currency(CurrencyCode);

        private static readonly CurrencyInfo s_CurrencyInfo = new CurrencyInfo(CurrencyCode, 978) {
            EnglishName = @"Euro",
            MinorUnits = 2
        };

        private Euro() : base(CurrencyCode) { }

        /// <summary>
        /// Gets the unique instance of the <see cref="Currency" /> class for the "Euro".
        /// </summary>
        /// <value>The unique instance of the <see cref="Currency" /> class for the "Euro".</value>
        public static Currency Currency
        {
            get
            {
                Contract.Ensures(Contract.Result<Currency>() != null);

                return s_Currency;
            }
        }

        /// <summary>
        /// Gets the unique instance of the <see cref="CurrencyInfo" /> class for the "Euro".
        /// </summary>
        /// <value>The unique instance of the <see cref="CurrencyInfo" /> class for the "Euro".</value>
        public static CurrencyInfo CurrencyInfo
        {
            get
            {
                Contract.Ensures(Contract.Result<CurrencyInfo>() != null);

                return s_CurrencyInfo;
            }
        }
    }
}
