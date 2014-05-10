// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.CommandLine
{
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class MvpBootstrapper
    {
        readonly MvpConfiguration _configuration;

        public MvpBootstrapper()
        {
            var defaultServices = new DefaultPlatformServices();
            defaultServices.SetPresenterDiscoveryStrategy(
                // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
                // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                    new AttributeBasedPresenterDiscoveryStrategy(),
                    new DefaultConventionBasedPresenterDiscoveryStrategy()}));

            _configuration = new MvpConfiguration(defaultServices);
        }

        public MvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            PlatformServices.Current = _configuration.CreatePlatformServices();
        }
    }
}
