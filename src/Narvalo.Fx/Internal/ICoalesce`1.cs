// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface ICoalesce<T>
    {
        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult then, TResult otherwise);
    }
}
