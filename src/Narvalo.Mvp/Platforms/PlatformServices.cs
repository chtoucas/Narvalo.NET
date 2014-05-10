// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    public static class PlatformServices
    {
        static readonly PlatformServicesVirtualProxy Instance_ = new PlatformServicesVirtualProxy();

        public static IPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.Reset(value); }
        }
    }
}
