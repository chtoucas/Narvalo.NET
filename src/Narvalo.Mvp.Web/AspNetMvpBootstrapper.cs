// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System.ComponentModel;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class AspNetMvpBootstrapper
    {
        readonly AspNetMvpConfiguration _configuration;
        readonly MvpBootstrapper _inner;

        public AspNetMvpBootstrapper() : this(new AspNetMvpConfiguration()) { }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public AspNetMvpBootstrapper(AspNetMvpConfiguration configuration)
        {
            Require.NotNull(configuration, "configuration");

            _configuration = configuration;
            _inner = new MvpBootstrapper(configuration);

            _inner.DefaultServicesCreated += (sender, e) =>
            {
                // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
                // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
                e.DefaultServices.SetDefaultPresenterDiscoveryStrategy(
                    () => new CompositePresenterDiscoveryStrategy(
                        new IPresenterDiscoveryStrategy[] {
                            new AttributeBasedPresenterDiscoveryStrategy(),
                            new AspNetConventionBasedPresenterDiscoveryStrategy()}));
            };
        }

        public AspNetMvpConfiguration Configuration { get { return _configuration; } }

        public void Run()
        {
            _inner.Run();
        }
    }
}
