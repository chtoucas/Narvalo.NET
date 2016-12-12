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

#if CONTRACTS_FULL

namespace Narvalo.Mvp
{
    using System;
    using System.Diagnostics.Contracts;

    using static System.Diagnostics.Contracts.Contract;

    [ContractClass(typeof(IMessageCoordinatorContract))]
    public partial interface IMessageCoordinator { }

    [ContractClassFor(typeof(IMessageCoordinator))]
    internal abstract class IMessageCoordinatorContract : IMessageCoordinator
    {
        void IMessageCoordinator.Close() { }

        void IMessageCoordinator.Publish<T>(T message)
        {
            Requires(message != null);
        }

        void IMessageCoordinator.Subscribe<T>(Action<T> onNext)
        {
            Requires(onNext != null);
        }
    }
}

#endif
