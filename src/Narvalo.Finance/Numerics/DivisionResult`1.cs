// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DivisionResult<T>
    {
        private readonly IEnumerable<T> _coll;
        private readonly T _remainder;

        public DivisionResult(T value, int n, T remainder)
        {
            _coll = Enumerable.Repeat(value, n);
            _remainder = remainder;
        }

        public T Remainder => _remainder;

        public IEnumerable<T> ToEnumerable() => _coll;
    }
}
