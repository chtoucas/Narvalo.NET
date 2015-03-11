// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
#if CONTRACTS_FULL
    using System.Diagnostics.Contracts;
#endif

#if CONTRACTS_FULL
    [ContractClass(typeof(IGuardContract))]
#endif
    public interface IGuard
    {
        // TODO: Ajouter les variantes async : Task, Begin/End, async ?
        void Execute(Action action);
    }

#if CONTRACTS_FULL

    [ContractClassFor(typeof(IGuard))]
    internal abstract class IGuardContract : IGuard
    {
        void IGuard.Execute(Action action)
        {
            Contract.Requires(action != null);
        }
    }

#endif
}
