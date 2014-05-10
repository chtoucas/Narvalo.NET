// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Platforms;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.
    /// </summary>
    public class MvpConfiguration
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        readonly IPlatformServices _defaultServices;

        ICompositeViewFactory _compositeViewFactory;
        IPresenterFactory _presenterFactory;

        public MvpConfiguration() : this(new DefaultPlatformServices()) { }

        public MvpConfiguration(IPlatformServices defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<MvpConfiguration, ICompositeViewFactory> CompositeViewFactory
        {
            get
            {
                return new Setter<MvpConfiguration, ICompositeViewFactory>(
                    this, _ => _compositeViewFactory = _);
            }
        }

        public Appender<MvpConfiguration, IPresenterDiscoveryStrategy> DiscoverPresenter
        {
            get
            {
                return new Appender<MvpConfiguration, IPresenterDiscoveryStrategy>(
                    this, _ => _presenterDiscoveryStrategies.Add(_));
            }
        }

        public Setter<MvpConfiguration, IPresenterFactory> PresenterFactory
        {
            get
            {
                return new Setter<MvpConfiguration, IPresenterFactory>(
                    this, _ => _presenterFactory = _);
            }
        }

        public IPlatformServices CreatePlatformServices()
        {
            var result = new PlatformServices_();

            result.CompositeViewFactory = _compositeViewFactory != null
                ? _compositeViewFactory
                : _defaultServices.CompositeViewFactory;

            result.PresenterFactory = _presenterFactory != null
                ? _presenterFactory
                : _defaultServices.PresenterFactory;

            var strategies = _presenterDiscoveryStrategies.Where(_ => _ != null).Distinct();
            var count = strategies.Count();

            if (count == 0) {
                result.PresenterDiscoveryStrategy = _defaultServices.PresenterDiscoveryStrategy;
            }
            else if (count == 1) {
                result.PresenterDiscoveryStrategy = strategies.First();
            }
            else if (count > 1) {
                result.PresenterDiscoveryStrategy
                    = new CompositePresenterDiscoveryStrategy(strategies);
            }

            return result;
        }

        class PlatformServices_ : IPlatformServices
        {
            public ICompositeViewFactory CompositeViewFactory { get; set; }

            public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; set; }

            public IPresenterFactory PresenterFactory { get; set; }
        }
    }
}
