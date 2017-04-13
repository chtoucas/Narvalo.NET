// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;

    /// <summary>
    /// Represents a container of zero or one value of type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>Equivalent to IEither&lt;T, Unit&gt;.</remarks>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    internal interface IMaybe<T> : IContainer<T>, Iterable<T>
    {
        TResult Match<TResult>(Func<T, TResult> caseSome, Func<TResult> caseNone);

        // Complements IContainer<T>.Do(Action<T> action).
        void Do(Action<T> onSome, Action onNone);
    }
}
