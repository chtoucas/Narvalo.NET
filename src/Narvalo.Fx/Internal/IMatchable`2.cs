// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface IMatchable<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2);

        // Equivalent to Match<Unit>().
        void Do(Action<T1> action1, Action<T2> action2);
    }
}
