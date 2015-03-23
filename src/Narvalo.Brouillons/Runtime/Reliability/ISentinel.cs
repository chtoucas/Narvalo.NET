// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
#if CONTRACTS_FULL // Using directive.
    using System.Diagnostics.Contracts;
#endif

    public partial interface ISentinel
    {
        // TODO: Ajouter les variantes async : Task, Begin/End, async ?
        void Execute(Action action);
    }

#if CONTRACTS_FULL // Contract Class and Object Invariants.

    [ContractClass(typeof(ISentinelContract))]
    public partial interface ISentinel { }

    [ContractClassFor(typeof(ISentinel))]
    internal abstract class ISentinelContract : ISentinel
    {
        void ISentinel.Execute(Action action)
        {
            Contract.Requires(action != null);
        }
    }

#endif
}
