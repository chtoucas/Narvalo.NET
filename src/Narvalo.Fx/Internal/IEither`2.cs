// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Disjoint union of TLeft and TRight.
    internal interface IEither<TLeft, TRight> : IMagma<TLeft>
    {
        TResult Match<TResult>(Func<TLeft, TResult> caseLeft, Func<TRight, TResult> caseRight);

        // Equivalent to Match<Unit>().
        void Do(Action<TLeft> onLeft, Action<TRight> onRight);
    }
}
