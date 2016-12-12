// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    public partial interface IMessageCoordinatorFactory
    {
        IMessageCoordinator Create();
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp.PresenterBinding
{
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IMessageCoordinatorFactoryContract))]
    public partial interface IMessageCoordinatorFactory { }

    [ContractClassFor(typeof(IMessageCoordinatorFactory))]
    internal abstract class IMessageCoordinatorFactoryContract : IMessageCoordinatorFactory
    {
        IMessageCoordinator IMessageCoordinatorFactory.Create()
        {
            Ensures(Result<IMessageCoordinator>() != null);

            return default(IMessageCoordinator);
        }
    }
}

#endif
