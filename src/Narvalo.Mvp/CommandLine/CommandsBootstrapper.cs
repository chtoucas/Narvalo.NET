// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    public sealed class CommandsBootstrapper : MvpBootstrapper
    {
        protected override void OnDefaultServicesCreated(DefaultServices defaultServices)
        {
            defaultServices.SetDefaultPresenterDiscoveryStrategy(
                () => new DefaultCommandPresenterDiscoveryStrategy());
        }
    }
}
