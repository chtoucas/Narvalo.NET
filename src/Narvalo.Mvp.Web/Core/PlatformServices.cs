// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public static class PlatformServices
    {
        static readonly IPlatformServices Default_
            = new DefaultPlatformServices_();

        static readonly PlatformServicesVirtualProxy Instance_
            = new PlatformServicesVirtualProxy(() => Default_);

        public static IPlatformServices Default
        {
            get { return Default_; }
        }

        public static IPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.Reset(value); }
        }

        class DefaultPlatformServices_ : DefaultPlatformServices
        {
            public DefaultPlatformServices_()
            {
                SetMessageCoordinatorFactory(() => new AspNetMessageCoordinatorFactory());

                // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
                // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
                SetPresenterDiscoveryStrategy(
                    () => new CompositePresenterDiscoveryStrategy(
                        new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new AspNetConventionBasedPresenterDiscoveryStrategy()}));
            }
        }
    }
}
