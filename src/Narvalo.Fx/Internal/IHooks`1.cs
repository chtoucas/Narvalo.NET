// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface IHooks<T>
    {
        // Normally, you also implement the following methods with a more appropriate name:
        // > void Do(Action<T> action);
        // There is another closely related method which is automatically generated for monads:
        // > void Unless(Func<T, bool> predicate, Action<T> action);

        void When(Func<T, bool> predicate, Action<T> action);
    }
}
