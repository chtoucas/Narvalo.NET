// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Reliability
{
    using System;

    public partial interface IReliabilitySentinel
    {
        // TODO: Ajouter les variantes async : Task, Begin/End, async ?
        void Invoke(Action action);
    }
}

#if CONTRACTS_FULL // Contract Class and Object Invariants.

namespace Narvalo.Reliability
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ISentinelContract))]
    public partial interface IReliabilitySentinel { }

    [ContractClassFor(typeof(IReliabilitySentinel))]
    internal abstract class IReliabilitySentinelContract : IReliabilitySentinel
    {
        void IReliabilitySentinel.Invoke(Action action)
        {
            Contract.Requires(action != null);
        }
    }
}

#endif
