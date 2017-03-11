// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    using System;

    // Meant to be a message bus to enable cross-presenter communication.
    // The message bus is shared only by the presenters bound during
    // the same binding operation. It is automatically closed when
    // the binder is released.
    public partial interface IMessageCoordinator
    {
        // NB: Repeated call to this method should not fail.
        void Close();

        void Publish<T>(T message);

        void Subscribe<T>(Action<T> onNext);
    }
}
