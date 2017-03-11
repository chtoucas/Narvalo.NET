// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;
    using System.ComponentModel;

    using Narvalo.Mvp.PresenterBinding;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public sealed class PlatformServicesVirtualProxy
        : LazyValueHolder<IPlatformServices>, IPlatformServices
    {
        public PlatformServicesVirtualProxy(Func<IPlatformServices> valueFactory)
            : base(valueFactory) { }

        public ICompositeViewFactory CompositeViewFactory => Value.CompositeViewFactory;

        public IMessageCoordinatorFactory MessageCoordinatorFactory => Value.MessageCoordinatorFactory;

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy => Value.PresenterDiscoveryStrategy;

        public IPresenterFactory PresenterFactory => Value.PresenterFactory;
    }
}
