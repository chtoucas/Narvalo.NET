// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Reflection.Emit;

    public partial interface IPresenterConstructorResolver
    {
        DynamicMethod Resolve(Type presenterType, Type viewType);
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.Resolvers
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Reflection.Emit;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterConstructorResolverContract))]
    public partial interface IPresenterConstructorResolver { }

    [ContractClassFor(typeof(IPresenterConstructorResolver))]
    internal abstract class IPresenterConstructorResolverContract : IPresenterConstructorResolver
    {
        DynamicMethod IPresenterConstructorResolver.Resolve(Type presenterType, Type viewType)
        {
            Requires(presenterType != null);
            Requires(viewType != null);
            Ensures(Result<DynamicMethod>() != null);

            return default(DynamicMethod);
        }
    }
}

#endif
