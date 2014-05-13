// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public class MvpConfiguration<T> where T : MvpConfiguration<T>
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        readonly IPlatformServices _defaultServices;

        ICompositeViewFactory _compositeViewFactory;
        IMessageBusFactory _messageBusFactory;
        IPresenterFactory _presenterFactory;

        public MvpConfiguration() : this(new DefaultPlatformServices()) { }

        public MvpConfiguration(IPlatformServices defaultServices)
        {
            Require.NotNull(defaultServices, "defaultServices");

            _defaultServices = defaultServices;
        }

        public Setter<T, ICompositeViewFactory> CompositeViewFactory
        {
            get
            {
                return new Setter<T, ICompositeViewFactory>(
                    (T)this, _ => _compositeViewFactory = _);
            }
        }

        public Appender<T, IPresenterDiscoveryStrategy> DiscoverPresenter
        {
            get
            {
                return new Appender<T, IPresenterDiscoveryStrategy>(
                    (T)this, _ => _presenterDiscoveryStrategies.Add(_));
            }
        }

        public Setter<T, IMessageBusFactory> MessageBusFactory
        {
            get
            {
                return new Setter<T, IMessageBusFactory>(
                    (T)this, _ => _messageBusFactory = _);
            }
        }

        public Setter<T, IPresenterFactory> PresenterFactory
        {
            get
            {
                return new Setter<T, IPresenterFactory>(
                    (T)this, _ => _presenterFactory = _);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IPlatformServices CreatePlatformServices()
        {
            var result = new PlatformServices_();

            result.CompositeViewFactory = _compositeViewFactory != null
                ? _compositeViewFactory
                : _defaultServices.CompositeViewFactory;

            result.MessageBusFactory = _messageBusFactory != null
                ? _messageBusFactory
                : _defaultServices.MessageBusFactory;

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

            public IMessageBusFactory MessageBusFactory { get; set; }

            public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; set; }

            public IPresenterFactory PresenterFactory { get; set; }
        }
    }
}
