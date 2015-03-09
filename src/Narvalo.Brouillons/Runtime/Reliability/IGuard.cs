// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Runtime.Reliability
{
    using System;

    // TODO: Ajouter les variantes async : Task, Begin/End, async ?
    public interface IGuard
    {
        void Execute(Action action);
    }
}
