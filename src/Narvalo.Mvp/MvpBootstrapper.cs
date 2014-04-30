// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System.Collections.Generic;
    using Narvalo.Mvp.Binder;

    public sealed class MvpBootstrapper
    {
        readonly IList<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategies
            = new List<IPresenterDiscoveryStrategy>();

        public ICompositeViewFactory CompositeViewFactory { get; set; }
        public IMessageBus MessageBus { get; set; }
        public IPresenterFactory PresenterFactory { get; set; }

        public MvpBootstrapper Append(IPresenterDiscoveryStrategy value)
        {
            _presenterDiscoveryStrategies.Add(value);

            return this;
        }

        public void Run()
        {
            if (CompositeViewFactory != null) {
                CompositeViewFactoryProvider.Current.SetService(CompositeViewFactory);
            }

            if (MessageBus != null) {
                MessageBusProvider.Current.SetService(MessageBus);
            }

            if (PresenterFactory != null) {
                PresenterFactoryProvider.Current.SetService(PresenterFactory);
            }

            IPresenterDiscoveryStrategy strategy = null;

            if (_presenterDiscoveryStrategies.Count > 1) {
                strategy = new CompositePresenterDiscoveryStrategy(_presenterDiscoveryStrategies);
            }
            else if (_presenterDiscoveryStrategies.Count == 1) {
                strategy = _presenterDiscoveryStrategies[0];
            }

            if (strategy != null) {
                PresenterDiscoveryStrategyProvider.Current.SetService(strategy);
            }
        }
    }
}
