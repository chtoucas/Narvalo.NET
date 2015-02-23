// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public static class PlatformServices
    {
        private static readonly IPlatformServices s_Default
             = new DefaultPlatformServices_();

        private static readonly PlatformServicesVirtualProxy s_Instance
            = new PlatformServicesVirtualProxy(() => s_Default);

        public static IPlatformServices Default
        {
            get { return s_Default; }
        }

        public static IPlatformServices Current
        {
            get { return s_Instance; }
            set { s_Instance.Reset(value); }
        }

        private class DefaultPlatformServices_ : DefaultPlatformServices
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
                            new AspNetConventionBasedPresenterDiscoveryStrategy() }));
            }
        }
    }
}
