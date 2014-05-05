// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using Narvalo.Mvp.PresenterBinding;

    public class ReactiveMessageBusFactory : IMessageBusFactory
    {
        public IMessageBus Create()
        {
            return new ReactiveMessageBus();
        }
    }
}
