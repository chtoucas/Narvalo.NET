// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System.ComponentModel;
    using Narvalo.Mvp.PresenterBinding;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class PlatformServicesProxy : LazyLazy<IPlatformServices>, IPlatformServices
    {
        public PlatformServicesProxy() : base(() => new DefaultPlatformServices()) { }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return Value.CompositeViewFactory; }
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
