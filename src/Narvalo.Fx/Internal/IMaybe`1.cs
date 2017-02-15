// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // NB: Equivalent to IEither<T, Unit>.
    internal interface IMaybe<T> : IMagma<T>
    {
        TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone);

        TResult Match<TResult>(Func<T, TResult> caseSome, TResult caseNone);

        TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult);

        // Equivalent to Coalesce<Unit>().
        void When(Func<T, bool> predicate, Action<T> action, Action otherwise);

        // Equivalent to Match<Unit>().
        void Do(Action<T> onSome, Action onNone);
    }
}
