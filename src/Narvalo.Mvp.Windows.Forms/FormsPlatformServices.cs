// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    public static class FormsPlatformServices
    {
        static readonly FormsPlatformServicesProxy Instance_ = new FormsPlatformServicesProxy();

        public static IFormsPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.InnerSet(value); }
        }
    }
}
