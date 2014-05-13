// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web.Core
{
    using Narvalo.Mvp.Platforms;

    public static class AspNetPlatformServices
    {
        static readonly AspNetPlatformServicesVirtualProxy Instance_ 
            = new AspNetPlatformServicesVirtualProxy();

        public static IPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.Reset(value); }
        }
    }
}
