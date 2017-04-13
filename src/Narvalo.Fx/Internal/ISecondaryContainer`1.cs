// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System;
    using System.Collections.Generic;

    // Strictly identical to IContainer<T>.
    // **WARNING** If we update this interface, we should mirror the modifications in IContainer<T>.
    // Only necessary to avoid the (expected) compiler error (CS0695):
    //   'generic type' cannot implement both 'generic interface' and 'generic interface'
    //   because they may unify for some type parameter substitutions.
    internal interface ISecondaryContainer<T>
    {
        bool Contains(T value);

        bool Contains(T value, IEqualityComparer<T> comparer);

        bool Do(Action<T> action);
    }
}
