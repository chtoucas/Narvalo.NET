// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    // Meant to be a message bus to enable cross-presenter communication.
    // Depending on the underlying platform, the message bus will be either global
    // or shared only by the presenters bound during the same binding operation.
    // The default behaviour is to use a global one.
    public interface IMessageBus
    {
        //void Unsubscribe();

        //IObservable<T> Register<T>();

        void Publish<T>(T message);

        void Subscribe<T>(Action<T> onNext);
    }
}
