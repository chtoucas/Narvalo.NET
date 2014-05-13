// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultFormsPlatformServices : DefaultPlatformServices, IPlatformServices 
    {
        public DefaultFormsPlatformServices()
        {
            // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
            // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
            SetPresenterDiscoveryStrategy(
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new DefaultConventionBasedPresenterDiscoveryStrategy()}));
        }
    }
}
