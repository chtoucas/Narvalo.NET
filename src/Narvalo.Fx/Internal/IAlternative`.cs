// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Degenerate case, really an if/then/else.
    // NB: Equivalent to IMatchable<T, Unit>.
    internal interface IAlternative<T>
    {
        TResult Match<TResult>(Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T, TResult> selector, TResult other);

        TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult);

        // Equivalent to Coalesce<Unit>().
        void Do(Func<T, bool> predicate, Action<T> action, Action otherwise);

        // Equivalent to Match<Unit>().
        void Do(Action<T> action, Action otherwise);
    }

    // NB: Equivalent to IMatchable<T1, T2, Unit> (not defined).
    internal interface IAlternative<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, Func<TResult> otherwise);

        TResult Match<TResult>(Func<T1, TResult> selector1, Func<T2, TResult> selector2, TResult other);

        // Equivalent to Match<Unit>().
        void Do(Action<T1> action1, Action<T2> action2, Action otherwise);
    }
}
