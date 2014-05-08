// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Services;

    public sealed class AspNetMvpBootstrapper : MvpBootstrapper
    {
        protected override void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
            // We keep "AttributeBasedPresenterDiscoveryStrategy" on top of the list
            // since it is the most complete implementation.
            defaultServices.SetDefaultPresenterDiscoveryStrategy(
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new AspNetConventionBasedPresenterDiscoveryStrategy()}));

            defaultServices.SetDefaultMessageBusFactory(() => new AspNetMessageBusFactory());

            base.OnDefaultServicesCreated(defaultServices);
        }
    }
}
