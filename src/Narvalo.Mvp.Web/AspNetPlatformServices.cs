// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    public static class AspNetPlatformServices
    {
        static readonly AspNetPlatformServicesProxy Instance_ = new AspNetPlatformServicesProxy();

        public static IAspNetPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.InnerSet(value); }
        }
    }
}
