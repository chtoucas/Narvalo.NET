// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Collections.Generic;

    public interface ICurrencyProvider
    {
        /// <summary>
        /// Gets the list of currency codes.
        /// </summary>
        /// <value>The list of currency codes.</value>
        HashSet<string> CurrencyCodes { get; }

        /// <summary>
        /// Obtains the list of supported currencies filtered by the specified
        /// <see cref="CurrencyTypes"/> parameter.
        /// </summary>
        /// <param name="types">A bitwise combination of the enumeration values that filter the 
        /// currencies to retrieve.</param>
        /// <returns>An enumeration that contains the currencies filtered by the <paramref name="types"/> 
        /// parameter.</returns>
        IEnumerable<CurrencyInfo> GetCurrencies(CurrencyTypes types);

        /// <summary>
        /// Obtains the fallback currency symbol for a given currency code.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Since the currency symbol actually depends on the culture and local habits, this method 
        /// can only return a closest match. The result is then non-authoritative.
        /// </para>
        /// <para>
        /// If none is found, returns the Unicode currency sign.
        /// </para>
        /// </remarks>
        /// <param name="code">The alphabetic code of the currency.</param>
        /// <returns>The fallback currency symbol for a given currency code.</returns>
        string GetFallbackSymbol(string code);
    }
}
