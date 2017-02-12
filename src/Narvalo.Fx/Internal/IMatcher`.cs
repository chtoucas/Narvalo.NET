// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Degenerate case, really an if/then.
    // NB: Equivalent to IDisjointUnionOf<T, Unit>.
    // You should also implements the following methods:
    // - void OnValue(Action<T>)
    // - void OnElse(Action)
    // There are not included since each type has a better way of naming them.
    internal interface IMatcher<T>
    {
        TResult Match<TResult>(Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T, TResult> selector, TResult other);

        TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult then, TResult other);

        // Equivalent to Coalesce<Unit>().
        void Do(Func<T, bool> predicate, Action<T> action, Action otherwise);

        // Equivalent to Match<Unit>().
        void Do(Action<T> action, Action otherwise);
    }

    // NB: Equivalent to IDisjointUnionOf<T1, T2, Unit> (not defined).
    // You should also implements the following methods:
    // - void OnValue1(Action<T1>)
    // - void OnValue2(Action<T1>)
    // - void OnElse(Action)
    // There are not included since each type has a better way of naming them.
    internal interface IMatcher<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, TResult other);

        // Equivalent to Match<Unit>().
        void Do(Action<T1> action1, Action<T2> action2, Action otherwise);
    }
}
