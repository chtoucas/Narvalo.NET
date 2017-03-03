// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance
{
    public partial interface ICurrencyInfoProvider
    {
        /// <summary>
        /// Obtains the list of supported currencies filtered by the specified
        /// <see cref="CurrencyTypes"/> parameter.
        /// </summary>
        /// <param name="types">A bitwise combination of the enumeration values that filter the
        /// currencies to retrieve.</param>
        /// <returns>An enumeration that contains the currencies filtered by the <paramref name="types"/>
        /// parameter.</returns>
        CurrencyInfoCollection GetCurrencies(CurrencyTypes types);
    }
}
