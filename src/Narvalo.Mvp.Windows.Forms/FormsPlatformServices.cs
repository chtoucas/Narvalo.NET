// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    public static class FormsPlatformServices
    {
        static readonly FormsPlatformServicesVirtualProxy Instance_ = new FormsPlatformServicesVirtualProxy();

        public static IFormsPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.Reset(value); }
        }
    }
}
