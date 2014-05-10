// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;
    using Narvalo.Mvp.PresenterBinding;
    using Narvalo.Mvp.Platforms;

    public sealed class DefaultFormsPlatformServices : DefaultPlatformServices, IFormsPlatformServices 
    {
        Func<IMessageBusFactory> _messageBusFactoryThunk = () => new MessageBusFactory();

        IMessageBusFactory _messageBusFactory;

        public DefaultFormsPlatformServices()
        {
            // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
            // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
            SetPresenterDiscoveryStrategy(
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new FormsConventionBasedPresenterDiscoveryStrategy()}));
        }

        public IMessageBusFactory MessageBusFactory
        {
            get
            {
                return _messageBusFactory
                    ?? (_messageBusFactory = _messageBusFactoryThunk());
            }
        }

        public void SetMessageCoordinatorFactory(Func<IMessageBusFactory> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _messageBusFactoryThunk = thunk;
        }
    }
}
