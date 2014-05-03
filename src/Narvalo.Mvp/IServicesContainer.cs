// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using Narvalo.Mvp.PresenterBinding;

    public interface IServicesContainer
    {
        ICompositeViewFactory CompositeViewFactory { get; }

        IMessageBus MessageBus { get; }

        IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; }

        IPresenterFactory PresenterFactory { get; }
    }
}
