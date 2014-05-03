// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.PresenterBinding;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.
    /// </summary>
    public sealed class MvpConfiguration
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        ICompositeViewFactory _compositeViewFactory;
        IMessageBus _messageBus;
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

        public Setter<MvpConfiguration, IMessageBus> MessageBus
        {
            get
            {
                return new Setter<MvpConfiguration, IMessageBus>(
                    this, _ => _messageBus = _);
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

        public IServicesContainer CreateServicesContainer(DefaultServices container)
        {
            if (_compositeViewFactory != null) {
                container.CompositeViewFactory = _compositeViewFactory;
            }

            if (_presenterFactory != null) {
                container.PresenterFactory = _presenterFactory;
            }

            var strategies = _presenterDiscoveryStrategies.Where(_ => _ != null).Distinct();

            var count = strategies.Count();

            if (count == 1) {
                container.PresenterDiscoveryStrategy = strategies.First();
            }
            else if (count > 1) {
                container.PresenterDiscoveryStrategy
                    = new CompositePresenterDiscoveryStrategy(strategies);
            }

            if (_messageBus != null) {
                container.MessageBus = _messageBus;
            }

            return container;
        }
    }
}
