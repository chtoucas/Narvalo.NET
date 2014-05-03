// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    public sealed class AspNetBootstrapper : MvpBootstrapper
    {
        protected override void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
            defaultServices.SetPresenterDiscoveryStrategy(
                () => new DefaultAspNetPresenterDiscoveryStrategy());
        }
    }
}
