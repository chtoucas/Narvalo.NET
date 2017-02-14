// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface IOptional<T>
    {
        void When(Func<T, bool> predicate, Action<T> action);
    }
}
