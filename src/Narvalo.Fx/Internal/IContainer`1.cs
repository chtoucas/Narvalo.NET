// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx.Internal
{
    using System;

    /// <summary>
    /// Represents a container of zero or more values of type <typeparamref name="T"/>.
    /// <para>All monads should implement this interface.</para>
    /// </summary>
    /// <typeparam name="T">The type of the underlying value.</typeparam>
    // **WARNING** If we update this interface, we should mirror the modifications in ISecondaryContainer<T>.
    internal interface IContainer<T>
    {
        // This method is also automatically generated for all monads. Another one is:
        // > void Unless(Func<T, bool> predicate, Action<T> action);
        // for which the default implementation calls back to When() [FIXME: This is no longer the case].
        void When(Func<T, bool> predicate, Action<T> action);

        void Do(Action<T> action);

        //void Forever(Action<T> action);

        //void While(Func<bool> predicate, Action<T> action);
    }
}
