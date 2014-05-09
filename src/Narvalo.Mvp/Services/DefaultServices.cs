// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Services
{
    using System;
    using Narvalo.Mvp.PresenterBinding;

    public class DefaultServices : IServicesContainer
    {
        Func<ICompositeViewFactory> _compositeViewFactoryThunk
           = () => new CompositeViewFactory();

        Func<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategyThunk
           = () => new AttributeBasedPresenterDiscoveryStrategy();

        Func<IPresenterFactory> _presenterFactoryThunk
           = () => new PresenterFactory();

        ICompositeViewFactory _compositeViewFactory;
        IPresenterDiscoveryStrategy _presenterDiscoveryStrategy;
        IPresenterFactory _presenterFactory;

        public ICompositeViewFactory CompositeViewFactory
        {
            get
            {
                return _compositeViewFactory
                    ?? (_compositeViewFactory = _compositeViewFactoryThunk());
            }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get
            {
                return _presenterDiscoveryStrategy
                    ?? (_presenterDiscoveryStrategy = _presenterDiscoveryStrategyThunk());
            }
        }

        public IPresenterFactory PresenterFactory
        {
            get
            {
                return _presenterFactory
                    ?? (_presenterFactory = _presenterFactoryThunk());
            }
        }

        public void SetDefaultCompositeViewFactory(Func<ICompositeViewFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _compositeViewFactoryThunk = thunk;
        }

        public void SetDefaultPresenterDiscoveryStrategy(Func<IPresenterDiscoveryStrategy> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterDiscoveryStrategyThunk = thunk;
        }

        public void SetDefaultPresenterFactory(Func<IPresenterFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterFactoryThunk = thunk;
        }
    }
}
