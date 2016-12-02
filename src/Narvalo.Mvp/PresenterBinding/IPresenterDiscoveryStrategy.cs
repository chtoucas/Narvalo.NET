// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;

    public partial interface IPresenterDiscoveryStrategy
    {
        PresenterDiscoveryResult FindBindings(IEnumerable<object> hosts, IEnumerable<IView> views);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterDiscoveryStrategyContract))]
    public partial interface IPresenterDiscoveryStrategy { }

    [ContractClassFor(typeof(IPresenterDiscoveryStrategy))]
    internal abstract class IPresenterDiscoveryStrategyContract : IPresenterDiscoveryStrategy
    {
        PresenterDiscoveryResult IPresenterDiscoveryStrategy.FindBindings(
            IEnumerable<object> hosts,
            IEnumerable<IView> views)
        {
            Ensures(Result<PresenterDiscoveryResult>() != null);

            return default(PresenterDiscoveryResult);
        }
    }
}

#endif
