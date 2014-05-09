// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Services;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.
    /// </summary>
    public class MvpConfiguration
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        ICompositeViewFactory _compositeViewFactory;
        IPresenterFactory _presenterFactory;

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

        public IServicesContainer CreateServicesContainer()
        {
            return CreateServicesContainer(new DefaultServices());
        }

        public IServicesContainer CreateServicesContainer(DefaultServices defaultServices)
        {
            var result = new ServicesContainer_();

            result.CompositeViewFactory = _compositeViewFactory != null
                ? _compositeViewFactory
                : defaultServices.CompositeViewFactory;

            result.PresenterFactory = _presenterFactory != null
                ? _presenterFactory
                : defaultServices.PresenterFactory;

            var strategies = _presenterDiscoveryStrategies.Where(_ => _ != null).Distinct();
            var count = strategies.Count();

            if (count == 0) {
                result.PresenterDiscoveryStrategy = defaultServices.PresenterDiscoveryStrategy;
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

        class ServicesContainer_ : IServicesContainer
        {
            public ICompositeViewFactory CompositeViewFactory { get; set; }

            public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; set; }

            public IPresenterFactory PresenterFactory { get; set; }
        }
    }
}
