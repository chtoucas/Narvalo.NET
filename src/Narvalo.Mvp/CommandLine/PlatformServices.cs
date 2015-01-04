// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using Narvalo.Mvp.Platforms;

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
                SetPresenterDiscoveryStrategy(
                    () => new DefaultConventionBasedPresenterDiscoveryStrategy());
            }
        }
    }
}
