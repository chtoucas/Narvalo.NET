// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // NB: Equivalent to IDisjointUnionOf<T, Unit>.
    internal interface ISwitch<T1>
    {
        TResult Match<TResult>(Func<T1, TResult> case1, Func<TResult> otherwise);

        // Equivalent to Match<Unit>().
        void Match(Action<T1> case1, Action otherwise);
    }

    // NB: Equivalent to IDisjointUnionOf<T1, T2, Unit> (not defined).
    internal interface ISwitch<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> case1, Func<T2, TResult> case2, Func<TResult> otherwise);

        // Equivalent to Match<Unit>().
        void Match(Action<T1> case1, Action<T2> case2, Action otherwise);
    }

    internal interface IDisjointUnionOf<T1, T2>
    {
        TResult Match<TResult>(Func<T1, TResult> case1, Func<T2, TResult> case2);

        // Equivalent to Match<Unit>().
        void Match(Action<T1> case1, Action<T2> case2);
    }
}
