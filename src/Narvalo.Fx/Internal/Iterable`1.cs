// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Internal
{
    using System.Collections.Generic;

    internal interface Iterable<out T>
    {
        IEnumerable<T> ToEnumerable();

        IEnumerator<T> GetEnumerator();
    }
}
