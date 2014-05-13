// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.PresenterBinding
{
    public class /*Default*/MessageBusFactory : IMessageBusFactory
    {
        static readonly IMessageBus Unique_ = new MessageBus();

        public bool IsStatic { get { return true; } }

        public IMessageBus Create()
        {
            return Unique_;
        }
    }
}
