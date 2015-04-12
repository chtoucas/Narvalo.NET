// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Globalization
{
    using System.Collections.Generic;

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

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Globalization
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Collections.ObjectModel;

    [ContractClass(typeof(ICurrencyInfoProviderContract))]
    public partial interface ICurrencyInfoProvider { }

    [ContractClassFor(typeof(ICurrencyInfoProvider))]
    internal abstract class ICurrencyInfoProviderContract : ICurrencyInfoProvider
    {
        public override CurrencyInfoCollection GetCurrencies(CurrencyTypes types)
        {
                Contract.Ensures(Contract.Result<CurrencyInfoCollection>() != null);

                return default(CurrencyInfoCollection);
        }
    }
}

#endif
