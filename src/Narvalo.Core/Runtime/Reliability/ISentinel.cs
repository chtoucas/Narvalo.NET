// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(ISentinelContract))]
    public interface ISentinel
    {
        // TODO: Ajouter les variantes async : Task, Begin/End, async ?
        void Execute(Action action);
    }

    [ContractClassFor(typeof(ISentinel))]
    internal abstract class ISentinelContract : ISentinel
    {
        void ISentinel.Execute(Action action)
        {
            Contract.Requires(action != null);
        }
    }
}
