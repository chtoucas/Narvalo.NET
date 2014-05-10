// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    public static class PlatformServices
    {
        static readonly PlatformServicesProxy Instance_ = new PlatformServicesProxy();

        public static IPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.InnerSet(value); }
        }
    }
}
