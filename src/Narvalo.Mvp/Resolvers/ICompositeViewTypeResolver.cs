// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public partial interface ICompositeViewTypeResolver
    {
        Type Resolve(Type viewType);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(ICompositeViewTypeResolverContract))]
    public partial interface ICompositeViewTypeResolver { }

    [ContractClassFor(typeof(ICompositeViewTypeResolver))]
    internal abstract class ICompositeViewTypeResolverContract : ICompositeViewTypeResolver
    {
        Type ICompositeViewTypeResolver.Resolve(Type viewType)
        {
            Requires(viewType != null);
            Ensures(Result<Type>() != null);

            return default(Type);
        }
    }
}

#endif
