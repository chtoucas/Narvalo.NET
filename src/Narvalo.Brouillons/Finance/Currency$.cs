// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    public static class CurrencyHelper
    {
        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class associated 
        /// with the specified culture.
        /// </summary>
        /// <param name="cultureInfo">A culture info.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="cultureInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the specified culture.</exception>
        /// <returns>The currency for the specified culture info.</returns>
        public static Currency OfCulture(CultureInfo cultureInfo)
        {
            Require.NotNull(cultureInfo, "cultureInfo");
            Contract.Ensures(Contract.Result<Currency>() != null);

            return Currency.OfRegion(new RegionInfo(cultureInfo.LCID));
        }

        /// <summary>
        /// Obtains an instance of the <see cref="Currency" /> class for the culture
        /// used by the current thread.
        /// </summary>
        /// <exception cref="CurrencyNotFoundException">Thrown if no currency exists for the current culture.</exception>
        /// <returns>The currency for the culture used by the current thread.</returns>
        public static Currency OfCurrentCulture()
        {
            Contract.Ensures(Contract.Result<Currency>() != null);

            return Currency.OfRegion(new RegionInfo(CultureInfo.CurrentCulture.LCID));
        }
    }
}
