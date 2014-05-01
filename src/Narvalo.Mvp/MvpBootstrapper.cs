// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System.Collections.Generic;
    using System.Linq;
    using Narvalo.Mvp.Binder;
    using Narvalo.Mvp.Configuration;
    using Narvalo.Mvp.Internal;

    /// <summary>
    /// Provides a single entry point to configure Narvalo.Mvp.
    /// </summary>
    public sealed class MvpBootstrapper
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        ICompositeViewFactory _compositeViewFactory;
        IMessageBus _messageBus;
        IPresenterFactory _presenterFactory;

        public Setter<MvpBootstrapper, ICompositeViewFactory> CompositeViewFactory
        {
            get
            {
                return new Setter<MvpBootstrapper, ICompositeViewFactory>(
                    this, _ => _compositeViewFactory = _);
            }
        }

        public Setter<MvpBootstrapper, IMessageBus> MessageBus
        {
            get
            {
                return new Setter<MvpBootstrapper, IMessageBus>(
                    this, _ => _messageBus = _);
            }
        }

        public Setter<MvpBootstrapper, IPresenterFactory> PresenterFactory
        {
            get
            {
                return new Setter<MvpBootstrapper, IPresenterFactory>(
                    this, _ => _presenterFactory = _);
            }
        }

        public Appender<MvpBootstrapper, IPresenterDiscoveryStrategy> DiscoverPresenter
        {
            get
            {
                return new Appender<MvpBootstrapper, IPresenterDiscoveryStrategy>(
                    this, _ => _presenterDiscoveryStrategies.Add(_));
            }
        }

        public void Run()
        {
            var bindingServices = BindingServicesContainer.Current;

            if (_compositeViewFactory != null) {
                bindingServices.CompositeViewFactory = _compositeViewFactory;
            }

            if (_presenterFactory != null) {
                bindingServices.PresenterFactory = _presenterFactory;
            }

            var strategies = _presenterDiscoveryStrategies.Where(_ => _ != null).Distinct();

            var count = strategies.Count();

            if (count == 1) {
                bindingServices.PresenterDiscoveryStrategy = strategies.First();
            }
            else if (count > 1) {
                bindingServices.PresenterDiscoveryStrategy
                    = new CompositePresenterDiscoveryStrategy(strategies);
            }

            if (_messageBus != null) {
                MessageBusContainer.Current.Reset(_messageBus);
            }
        }
    }
}
