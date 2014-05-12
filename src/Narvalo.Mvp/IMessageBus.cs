// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    public interface IMessageBus
    {
        //bool CanClose { get; }

        //bool Closed { get; }

        //void Close();

        void Publish<T>(T message);

        IObservable<T> Register<T>();

        void Subscribe<T>(Action<T> onNext);
    }
}
