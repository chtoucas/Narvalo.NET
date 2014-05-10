// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using Narvalo.Mvp.Platforms;

    public sealed class MvpBootstrapper
    {
        readonly AspNetMvpConfiguration _configuration = new AspNetMvpConfiguration();

        public AspNetMvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            AspNetPlatformServices.Current = _configuration.CreateAspNetPlatformServices();
        }
    }
}
