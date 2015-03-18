// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    // FIXME: Very bad design.
    [ContractClass(typeof(ICurrencyProviderContract))]
    public interface ICurrencyProvider
    {
        /// <summary>
        /// Gets the set of available currency codes.
        /// </summary>
        /// <value>The set of available currency codes.</value>
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
    }

    [ContractClassFor(typeof(ICurrencyProvider))]
    internal abstract class ICurrencyProviderContract : ICurrencyProvider
    {
        HashSet<string> ICurrencyProvider.CurrencyCodes
        {
            get
            {
                Contract.Ensures(Contract.Result<HashSet<string>>() != null);

                return default(HashSet<string>);
            }
        }

        IEnumerable<CurrencyInfo> ICurrencyProvider.GetCurrencies(CurrencyTypes types)
        {
            return default(IEnumerable<CurrencyInfo>);
        }
    }
}
