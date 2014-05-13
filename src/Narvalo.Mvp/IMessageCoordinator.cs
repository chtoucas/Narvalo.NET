// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp
{
    public interface IMessageCoordinator : IMessageBus
    {
        bool Closed { get; }

        // NB: Repeated call to Close() must be allowed.
        void Close();
    }
}
