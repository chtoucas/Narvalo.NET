// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine.Core
{
    using Narvalo.Mvp.Platforms;

    public static class CommandsPlatformServices
    {
        static readonly CommandsPlatformServicesVirtualProxy Instance_
            = new CommandsPlatformServicesVirtualProxy();

        public static IPlatformServices Current
        {
            get { return Instance_; }
            set { Instance_.Reset(value); }
        }
    }
}
