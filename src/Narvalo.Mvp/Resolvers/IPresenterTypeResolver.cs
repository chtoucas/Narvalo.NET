// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;

    public partial interface IPresenterTypeResolver
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

    [ContractClass(typeof(IPresenterTypeResolverContract))]
    public partial interface IPresenterTypeResolver { }

    [ContractClassFor(typeof(IPresenterTypeResolver))]
    internal abstract class IPresenterTypeResolverContract : IPresenterTypeResolver
    {
        Type IPresenterTypeResolver.Resolve(Type viewType)
        {
            Requires(viewType != null);
            // NB: We allow null return value.

            return default(Type);
        }
    }
}

#endif