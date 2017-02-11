// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Degenerate case, really an if/then.
    // NB: Equivalent to IDisjointUnionOf<T, Unit>.
    internal interface ISwitch<T>
    {
        TResult Match<TResult>(Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T, TResult> selector, TResult other);

        // Equivalent to Match<Unit>().
        void Trigger(Action<T> action, Action otherwise);
    }

    // NB: Equivalent to IDisjointUnionOf<T1, T2, Unit> (not defined).
    internal interface ISwitch<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, TResult other);

        // Equivalent to Match<Unit>().
        void Trigger(Action<T1> action1, Action<T2> action2, Action otherwise);
    }
}
