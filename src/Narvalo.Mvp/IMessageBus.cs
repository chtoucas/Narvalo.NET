// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public interface IMessageBus
    {
        void Close();

        void Publish<TMessage>(TMessage message);

        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback);

        void Subscribe<TMessage>(Action<TMessage> messageReceivedCallback, Action neverReceivedCallback);
    }
}
