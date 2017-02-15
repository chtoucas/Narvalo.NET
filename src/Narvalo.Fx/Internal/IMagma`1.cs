// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface IMagma<T>
    {
        TResult Coalesce<TResult>(Func<T, bool> predicate, Func<T, TResult> selector, Func<TResult> otherwise);

        TResult Coalesce<TResult>(Func<T, bool> predicate, TResult thenResult, TResult elseResult);

        // There is another closely related method which is automatically generated for monads:
        // > void Unless(Func<T, bool> predicate, Action<T> action);
        void When(Func<T, bool> predicate, Action<T> action);

        // Equivalent to Coalesce<Unit>().
        void Do(Func<T, bool> predicate, Action<T> action, Action otherwise);

        void Do(Action<T> action);
    }
}
