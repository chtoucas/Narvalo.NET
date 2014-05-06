// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Services;

    public sealed class CommandsMvpBootstrapper : MvpBootstrapper
    {
        protected override void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
            defaultServices.SetDefaultPresenterDiscoveryStrategy(
                   () => new CompositePresenterDiscoveryStrategy(
                       new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new CommandConventionBasedPresenterDiscoveryStrategy()}));

            base.OnDefaultServicesCreated(defaultServices);
        }
    }
}
