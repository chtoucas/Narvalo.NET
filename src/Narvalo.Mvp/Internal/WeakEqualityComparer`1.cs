// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Mvp.Internal
{
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class WeakEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
        where T : class
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return WeakEquality.AreEqual(x, y);
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            Require.NotNull(obj, "obj");

            var result = obj
                .Aggregate<T, int?>(null, (acc, source) =>
                    acc == null ? source.GetHashCode() : acc.Value | source.GetHashCode());

            return result ?? 0;
        }
    }
}
