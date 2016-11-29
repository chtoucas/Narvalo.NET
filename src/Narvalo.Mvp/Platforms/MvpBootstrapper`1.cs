// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System.Collections.Generic;
    using System.ComponentModel;
#if CONTRACTS_FULL // Contract Class and Object Invariants.
    using System.Diagnostics.Contracts;
#endif
    using System.Linq;

    using Narvalo.Mvp.PresenterBinding;

    using static System.Diagnostics.Contracts.Contract;

    public abstract class MvpBootstrapper<T> where T : MvpBootstrapper<T>
    {
        private readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        private readonly IPlatformServices _defaultServices;

        private ICompositeViewFactory _compositeViewFactory;
        private IMessageCoordinatorFactory _messageCoordinatorFactory;
        private IPresenterFactory _presenterFactory;

        protected MvpBootstrapper(IPlatformServices defaultServices)
        {
            Require.NotNull(defaultServices, nameof(defaultServices));

            _defaultServices = defaultServices;
        }

        public Setter<T, ICompositeViewFactory> CompositeViewFactory
        {
            get
            {
                Ensures(Result<Setter<T, ICompositeViewFactory>>() != null);

                return new Setter<T, ICompositeViewFactory>(
                    (T)this, _ => _compositeViewFactory = _);
            }
        }

        public Appender<T, IPresenterDiscoveryStrategy> DiscoverPresenter
        {
            get
            {
                Ensures(Result<Appender<T, IPresenterDiscoveryStrategy>>() != null);

                return new Appender<T, IPresenterDiscoveryStrategy>(
                    (T)this, _ => _presenterDiscoveryStrategies.Add(_));
            }
        }

        public Setter<T, IMessageCoordinatorFactory> MessageCoordinatorFactory
        {
            get
            {
                Ensures(Result<Setter<T, IMessageCoordinatorFactory>>() != null);

                return new Setter<T, IMessageCoordinatorFactory>(
                    (T)this, _ => _messageCoordinatorFactory = _);
            }
        }

        public Setter<T, IPresenterFactory> PresenterFactory
        {
            get
            {
                Ensures(Result<Setter<T, IPresenterFactory>>() != null);

                return new Setter<T, IPresenterFactory>(
                    (T)this, _ => _presenterFactory = _);
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IPlatformServices CreatePlatformServices()
        {
            Ensures(Result<IPlatformServices>() != null);

            var platformServices = new PlatformServices_();

            platformServices.CompositeViewFactory = _compositeViewFactory != null
                ? _compositeViewFactory
                : _defaultServices.CompositeViewFactory;

            platformServices.MessageCoordinatorFactory = _messageCoordinatorFactory != null
                ? _messageCoordinatorFactory
                : _defaultServices.MessageCoordinatorFactory;

            platformServices.PresenterFactory = _presenterFactory != null
                ? _presenterFactory
                : _defaultServices.PresenterFactory;

            var strategies = _presenterDiscoveryStrategies.Where(_ => _ != null).Distinct();
            var count = strategies.Count();

            if (count == 0)
            {
                platformServices.PresenterDiscoveryStrategy = _defaultServices.PresenterDiscoveryStrategy;
            }
            else if (count == 1)
            {
                platformServices.PresenterDiscoveryStrategy = strategies.First();
            }
            else
            {
                platformServices.PresenterDiscoveryStrategy
                    = new CompositePresenterDiscoveryStrategy(strategies);
            }

            return platformServices;
        }

        private sealed class PlatformServices_ : IPlatformServices
        {
            public ICompositeViewFactory CompositeViewFactory { get; set; }

            public IMessageCoordinatorFactory MessageCoordinatorFactory { get; set; }

            public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy { get; set; }

            public IPresenterFactory PresenterFactory { get; set; }
        }
    }
}
