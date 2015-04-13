// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Moneta
{
    using System.Collections.Generic;

    public partial interface ICurrencyProvider
    {
        /// <summary>
        /// Gets the set of available currency codes.
        /// </summary>
        /// <value>The set of available currency codes.</value>
        HashSet<string> CurrencyCodes { get; }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Collections.ObjectModel;

    [ContractClass(typeof(ICurrencyProviderContract))]
    public partial interface ICurrencyProvider { }

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
    }
}

#endif
