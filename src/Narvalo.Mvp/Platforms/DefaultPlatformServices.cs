// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Platforms
{
    using System;

    using Narvalo.Mvp.PresenterBinding;

    public class DefaultPlatformServices : IPlatformServices
    {
        private Func<ICompositeViewFactory> _compositeViewFactoryThunk
           = () => new CompositeViewFactory();

        private Func<IMessageCoordinatorFactory> _messageCoordinatorFactoryThunk
           = () => new MessageCoordinatorFactory();

        private Func<IPresenterDiscoveryStrategy> _presenterDiscoveryStrategyThunk
           = () => new AttributedPresenterDiscoveryStrategy();

        private Func<IPresenterFactory> _presenterFactoryThunk
           = () => new PresenterFactory();

        private ICompositeViewFactory _compositeViewFactory;
        private IMessageCoordinatorFactory _messageCoordinatorFactory;
        private IPresenterDiscoveryStrategy _presenterDiscoveryStrategy;
        private IPresenterFactory _presenterFactory;

        public ICompositeViewFactory CompositeViewFactory
        {
            get
            {
                return _compositeViewFactory
                    ?? (_compositeViewFactory = _compositeViewFactoryThunk());
            }
        }

        public IMessageCoordinatorFactory MessageCoordinatorFactory
        {
            get
            {
                return _messageCoordinatorFactory
                    ?? (_messageCoordinatorFactory = _messageCoordinatorFactoryThunk());
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

        protected void SetMessageCoordinatorFactory(Func<IMessageCoordinatorFactory> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            _messageCoordinatorFactoryThunk = thunk;
        }

        protected void SetCompositeViewFactory(Func<ICompositeViewFactory> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            _compositeViewFactoryThunk = thunk;
        }

        protected void SetPresenterDiscoveryStrategy(Func<IPresenterDiscoveryStrategy> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            _presenterDiscoveryStrategyThunk = thunk;
        }

        protected void SetPresenterFactory(Func<IPresenterFactory> thunk)
        {
            Require.NotNull(thunk, nameof(thunk));

            _presenterFactoryThunk = thunk;
        }
    }
}
