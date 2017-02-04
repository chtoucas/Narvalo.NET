// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DivCollection<T> : IEnumerable<T>
    {
        private readonly int _count;
        private readonly T _quotient;
        private readonly T _remainder;

        private DivCollection(T quotient, T remainder, int count)
        {
            _quotient = quotient;
            _remainder = remainder;
            _count = count;
        }

        public int Count => _count;

        public T Quotient => _quotient;

        public T Remainder => _remainder;

        internal static DivCollection<T> Create(T quotient, T remainder, int count)
            => new DivCollection<T>(quotient, remainder, count);

        public IEnumerator<T> GetEnumerator() => Enumerable.Repeat(_quotient, _count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
