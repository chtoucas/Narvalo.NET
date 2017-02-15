// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    internal interface IMagma<T>
    {
        // There is another closely related method which is automatically generated for monads:
        // > void Unless(Func<T, bool> predicate, Action<T> action);
        void When(Func<T, bool> predicate, Action<T> action);

        void Do(Action<T> action);
    }
}
