// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System.Collections.Generic;
    using Narvalo.Mvp.Binder;
    using Narvalo.Mvp.Internal;

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
            var bindingServices = BindingServices.Current;

            if (CompositeViewFactory != null) {
                bindingServices.CompositeViewFactory = CompositeViewFactory;
            }

            if (PresenterFactory != null) {
                bindingServices.PresenterFactory = PresenterFactory;
            }

            var count = _presenterDiscoveryStrategies.Count;

            if (count == 1) {
                bindingServices.PresenterDiscoveryStrategy = _presenterDiscoveryStrategies[0];
            }
            else if (count > 1) {
                bindingServices.PresenterDiscoveryStrategy
                    = new CompositePresenterDiscoveryStrategy(_presenterDiscoveryStrategies);
            }

            if (MessageBus != null) {
                MessageBusProvider.Current.SetService(MessageBus);
            }
        }
    }
}
