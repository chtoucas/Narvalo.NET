﻿// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultAspNetPlatformServices : DefaultPlatformServices
    {
        public DefaultAspNetPlatformServices()
        {
            SetMessageBusFactory(() => new DefaultMessageBusFactory());

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
