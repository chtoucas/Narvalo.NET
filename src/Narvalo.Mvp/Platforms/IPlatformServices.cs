// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using Narvalo.Mvp.PresenterBinding;

    public partial interface IPlatformServices
    {
        ICompositeViewFactory CompositeViewFactory { get; }

        IMessageCoordinatorFactory MessageCoordinatorFactory { get; }

        IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; }

        IPresenterFactory PresenterFactory { get; }
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Mvp.Platforms
{
    using System.Diagnostics.Contracts;
    using PresenterBinding;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IPlatformServicesContract))]
    public partial interface IPlatformServices { }

    [ContractClassFor(typeof(IPlatformServices))]
    internal abstract class IPlatformServicesContract : IPlatformServices
    {
        ICompositeViewFactory IPlatformServices.CompositeViewFactory
        {
            get { Ensures(Result<ICompositeViewFactory>() != null); return default(ICompositeViewFactory); }
        }

        IMessageCoordinatorFactory IPlatformServices.MessageCoordinatorFactory
        {
            get { Ensures(Result<IMessageCoordinatorFactory>() != null); return default(IMessageCoordinatorFactory); }
        }

        IPresenterDiscoveryStrategy IPlatformServices.PresenterDiscoveryStrategy
        {
            get { Ensures(Result<IPresenterDiscoveryStrategy>() != null); return default(IPresenterDiscoveryStrategy); }
        }

        IPresenterFactory IPlatformServices.PresenterFactory
        {
            get { Ensures(Result<IPresenterFactory>() != null); return default(IPresenterFactory); }
        }
    }
}

#endif
