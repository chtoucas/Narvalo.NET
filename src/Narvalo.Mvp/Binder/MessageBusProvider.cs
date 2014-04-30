// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Binder
{
    using Narvalo.Mvp.Internal;

    public sealed class MessageBusProvider : ServiceProvider<IMessageBus>
    {
        static readonly MessageBusProvider Instance_ = new MessageBusProvider();

        MessageBusProvider() : base(() => new MessageBus()) { }

        public static MessageBusProvider Current { get { return Instance_; } }
    }
}
