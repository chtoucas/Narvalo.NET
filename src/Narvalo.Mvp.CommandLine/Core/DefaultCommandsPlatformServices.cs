// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine.Core
{
    using Narvalo.Mvp.Platforms;

    public sealed class DefaultCommandsPlatformServices : DefaultPlatformServices
    {
        public DefaultCommandsPlatformServices()
        {
            SetPresenterDiscoveryStrategy(
                () => new DefaultConventionBasedPresenterDiscoveryStrategy());
        }
    }
}
