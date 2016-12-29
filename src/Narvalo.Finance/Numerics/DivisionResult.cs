// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Numerics
{
    public static class DivisionResult
    {
        public static DivisionResult<T> Create<T>(T value, int n, T remainder)
            => new DivisionResult<T>(value, n, remainder);
    }
}
