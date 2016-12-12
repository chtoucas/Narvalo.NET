// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public partial interface IPresenter
    {
        IMessageCoordinator Messages { get; }
    }
}

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPresenterContract))]
    public partial interface IPresenter { }

    [ContractClassFor(typeof(IPresenter))]
    internal abstract class IPresenterContract : IPresenter
    {
        IMessageCoordinator IPresenter.Messages
        {
            get { Ensures(Result<IMessageCoordinator>() != null); return default(IMessageCoordinator); }
        }
    }
}

#endif
