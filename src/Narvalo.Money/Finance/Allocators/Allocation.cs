// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Finance.Allocators
{
    using System.Linq;

    public static class Allocation
    {
        public static Allocation<T> Create<T>(T total, T part, T remainder, int count)
            => new Allocation<T>(total, Enumerable.Repeat(part, count), remainder);
    }
}
