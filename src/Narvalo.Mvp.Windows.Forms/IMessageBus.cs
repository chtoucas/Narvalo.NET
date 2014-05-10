// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;

    // IMPORTANT: This is not meant to be be used as a global message bus.
    // Only views bound by the same "PresenteBinder" will share this bus.
    public interface IMessageBus
    {
        void Publish<T>(T message);

        //IObservable<T> Register<T>();

        void Subscribe<T>(Action<T> onNext);
    }
}
