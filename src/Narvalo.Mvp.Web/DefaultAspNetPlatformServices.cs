// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Platforms;

    public sealed class DefaultAspNetPlatformServices : DefaultPlatformServices, IAspNetPlatformServices 
    {
        Func<IMessageCoordinatorFactory> _messageCoordinatorFactoryThunk
           = () => new MessageCoordinatorFactory();

        IMessageCoordinatorFactory _messageCoordinatorFactory;

        public DefaultAspNetPlatformServices()
        {
            // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
            // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
            SetPresenterDiscoveryStrategy(
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new DefaultConventionBasedPresenterDiscoveryStrategy()}));
        }

        public IMessageCoordinatorFactory MessageCoordinatorFactory
        {
            get
            {
                return _messageCoordinatorFactory
                    ?? (_messageCoordinatorFactory = _messageCoordinatorFactoryThunk());
            }
        }

        public void SetMessageCoordinatorFactory(Func<IMessageCoordinatorFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _messageCoordinatorFactoryThunk = thunk;
        }
    }
}
