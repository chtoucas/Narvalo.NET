// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms.Core
{
    using System;
    using Narvalo.Mvp.Platforms;
    using Narvalo.Mvp.PresenterBinding;

    public sealed class DefaultFormsPlatformServices : DefaultPlatformServices, IFormsPlatformServices 
    {
        Func<IMessageBus> _messageBusThunk = () => new MessageBus();

        IMessageBus _messageBus;

        public DefaultFormsPlatformServices()
        {
            // Since "AttributeBasedPresenterDiscoveryStrategy" provides the most complete 
            // implementation of "IPresenterDiscoveryStrategy", we keep it on top the list.
            SetPresenterDiscoveryStrategy(
                () => new CompositePresenterDiscoveryStrategy(
                    new IPresenterDiscoveryStrategy[] {
                        new AttributeBasedPresenterDiscoveryStrategy(),
                        new DefaultConventionBasedPresenterDiscoveryStrategy()}));
        }

        public IMessageBus MessageBus
        {
            get
            {
                return _messageBus ?? (_messageBus = _messageBusThunk());
            }
        }

        public void SetMessageBus(Func<IMessageBus> thunk)
        {
            Require.NotNull(thunk, "thunk");

            _messageBusThunk = thunk;
        }
    }
}
