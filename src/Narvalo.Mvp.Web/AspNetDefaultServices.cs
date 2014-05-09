// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Services;

    public class AspNetDefaultServices : IAspNetServicesContainer
    {
        readonly IServicesContainer _inner;

        Func<IMessageCoordinatorFactory> _messageCoordinatorFactoryThunk
           = () => new MessageCoordinatorFactory();

        IMessageCoordinatorFactory _messageCoordinatorFactory;

        public AspNetDefaultServices(DefaultServices inner)
        {
            Require.NotNull(inner, "inner");

            _inner = inner;
        }

        public IMessageCoordinatorFactory MessageCoordinatorFactory
        {
            get
            {
                return _messageCoordinatorFactory
                    ?? (_messageCoordinatorFactory = _messageCoordinatorFactoryThunk());
            }
        }

        public ICompositeViewFactory CompositeViewFactory
        {
            get { return _inner.CompositeViewFactory; }
        }

        public IPresenterDiscoveryStrategy PresenterDiscoveryStrategy
        {
            get { return _inner.PresenterDiscoveryStrategy; }
        }

        public IPresenterFactory PresenterFactory
        {
            get { return _inner.PresenterFactory; }
        }
    }
}
