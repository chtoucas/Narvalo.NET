// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Services
{
    using Narvalo.Mvp.PresenterBinding;

    public interface IServicesContainer
    {
        ICompositeViewFactory CompositeViewFactory { get; }

        IMessageBusFactory MessageBusFactory { get; }

        IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; }

        IPresenterFactory PresenterFactory { get; }
    }
}
