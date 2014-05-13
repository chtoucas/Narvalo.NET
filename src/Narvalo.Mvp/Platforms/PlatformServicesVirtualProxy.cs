// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

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

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Value.CompositeViewFactory; }
        }
        
        public IMessageBusFactory MessageBusFactory
        {
            get { return Value.MessageBusFactory; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return Value.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return Value.PresenterFactory; }
        }
    }
}
