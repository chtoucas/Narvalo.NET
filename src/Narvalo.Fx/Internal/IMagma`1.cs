// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    // Monad of T.
    internal interface IMagma<T>
    {
        // This method is also automatically generated for all monads.
        // Another auto-generated method is:
        // > void Unless(Func<T, bool> predicate, Action<T> action);
        // for which the default callback to When().
        void When(Func<T, bool> predicate, Action<T> action);

        void Do(Action<T> action);
    }
}
