// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public sealed class MessageBusBlackhole : IMessageBus
    {
        static readonly IMessageBus Instance_ = new MessageBusBlackhole();

        public static IMessageBus Instance { get { return Instance_; } }

        public void Publish<T>(T message)
        {
            throw new NotSupportedException();
        }

        public void Subscribe<T>(Action<T> onNext)
        {
            throw new NotSupportedException();
        }
    }
}
