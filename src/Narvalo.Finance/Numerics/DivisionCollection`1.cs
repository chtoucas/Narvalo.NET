// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DivisionCollection<T> : IEnumerable<T>
    {
        private readonly int _count;
        private readonly T _quotient;
        private readonly T _remainder;

        private DivisionCollection(T quotient, T remainder, int count)
        {
            _quotient = quotient;
            _remainder = remainder;
            _count = count;
        }

        public int Count => _count;

        public T Quotient => _quotient;

        public T Remainder => _remainder;

        internal static DivisionCollection<T> Create(T quotient, T remainder, int count)
            => new DivisionCollection<T>(quotient, remainder, count);

        public IEnumerator<T> GetEnumerator() => Enumerable.Repeat(_quotient, _count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
