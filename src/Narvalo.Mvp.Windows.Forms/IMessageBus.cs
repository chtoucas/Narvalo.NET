// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Windows.Forms
{
    using System;

    // TODO: Unsubscribe & co
    public interface IMessageBus
    {
        void Publish<T>(T message);

        void Subscribe<T>(Action<T> onNext);
    }
}
