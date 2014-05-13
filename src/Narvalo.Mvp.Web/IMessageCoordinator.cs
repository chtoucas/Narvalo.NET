// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Web
{
    using System;

    // Only views bound by the same "HttpPresenteBinder" will share this message bus.
    // In practice, cross-presenter communication is applicable to all presenters
    // activated during the same HTTP request.
    // When a message is published, it will be handled by all present AND future handlers.
    // You then do not have to worry about the order of the pub/sub events.
    // A good rule of thumb is to subscribe or publish messages during the Load event. 
    // IMPORTANT: After pre-render completes, the message bus is closed.
    public interface IMessageCoordinator : IMessageBus, IDisposable
    {
        //void Close();
    }
}
