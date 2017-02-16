// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // MonadOr of T.
    // NB: Equivalent to IEither<T, Unit>.
    internal interface IMaybe<T> : IMagma<T>
    {
        TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone);

        TResult Match<TResult>(Func<T, TResult> caseSome, TResult caseNone);

        // See also the auto-generated methods:
        // > Monad<TResult> Coalesce<TResult>(Func<T, bool> predicate, Monad<TResult> thenResult, Monad<TResult> elseResult);
        // > Monad<TResult> If<TResult>(Func<T, bool> predicate, Monad<TResult> thenResult);
        TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult);

        // See also:
        // > IMagma<T>.When(Func<T, bool> predicate, Action<T> action);
        // Equivalent to Coalesce<Unit>().
        void When(Func<T, bool> predicate, Action<T> action, Action otherwise);

        // See also:
        // > IMagma<T>.Do(Action<T> action);
        // Equivalent to Match<Unit>().
        void Do(Action<T> onSome, Action onNone);
    }
}
