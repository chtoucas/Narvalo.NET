// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;

    public partial interface IPresenterBindingAttributesResolver
    {
        IEnumerable<PresenterBindingAttribute> Resolve(Type viewType);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterBindingAttributesResolverContract))]
    public partial interface IPresenterBindingAttributesResolver { }

    [ContractClassFor(typeof(IPresenterBindingAttributesResolver))]
    internal abstract class IPresenterBindingAttributesResolverContract : IPresenterBindingAttributesResolver
    {
        IEnumerable<PresenterBindingAttribute> IPresenterBindingAttributesResolver.Resolve(Type viewType)
        {
            Requires(viewType != null);
            Ensures(Result<IEnumerable<PresenterBindingAttribute>>() != null);

            return default(IEnumerable<PresenterBindingAttribute>);
        }
    }
}

#endif
