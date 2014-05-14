// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using Narvalo.Mvp.PresenterBinding;

    public interface IPlatformServices
    {
        ICompositeViewFactory CompositeViewFactory { get; }

        IMessageCoordinatorFactory MessageCoordinatorFactory { get; }

        IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; }

        IPresenterFactory PresenterFactory { get; }
    }
}
