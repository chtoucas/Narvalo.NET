// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System.Collections;
    using System.Collections.Generic;

    public class Allocation<T> : IEnumerable<T>
    {
        public Allocation(T total, IEnumerable<T> parts, T remainder)
        {
            Require.NotNull(parts, nameof(parts));

            Parts = parts;
            Remainder = remainder;
            Total = total;
        }

        public T Total { get; }

        public IEnumerable<T> Parts { get; }

        public T Remainder { get; }

        public IEnumerator<T> GetEnumerator() => Parts.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
