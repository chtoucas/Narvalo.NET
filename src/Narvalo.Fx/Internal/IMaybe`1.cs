// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    /// <summary>
    /// Represents a container of zero or one value of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>Equivalent to IEither&lt;T, Unit&gt;.</remarks>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    internal interface IMaybe<T> : IContainer<T>
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
