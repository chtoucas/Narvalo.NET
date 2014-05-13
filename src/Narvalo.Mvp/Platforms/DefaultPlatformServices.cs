// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;
    using Narvalo.Mvp.PresenterBinding;

    public class DefaultPlatformServices : IPlatformServices
    {
        Func<ICompositeViewFactory> _compositeViewFactoryThunk
           = () => new CompositeViewFactory();

        Func<IMessageBusFactory> _messageBusFactoryThunk
           = () => new MessageBusFactory();

        Func<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategyThunk
           = () => new AttributeBasedPresenterDiscoveryStrategy();

        Func<IPresenterFactory> _presenterFactoryThunk
           = () => new PresenterFactory();

        ICompositeViewFactory _compositeViewFactory;
        IMessageBusFactory _messageBusFactory;
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

        public IMessageBusFactory MessageBusFactory
        {
            get
            {
                return _messageBusFactory
                    ?? (_messageBusFactory = _messageBusFactoryThunk());
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

        public void SetMessageBusFactory(Func<IMessageBusFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _messageBusFactoryThunk = thunk;
        }

        public void SetCompositeViewFactory(Func<ICompositeViewFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _compositeViewFactoryThunk = thunk;
        }

        public void SetPresenterDiscoveryStrategy(Func<IPresenterDiscoveryStrategy> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterDiscoveryStrategyThunk = thunk;
        }

        public void SetPresenterFactory(Func<IPresenterFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _presenterFactoryThunk = thunk;
        }
    }
}
