// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Fx;

    // **WARNING** These sequences are infinite!
    public static class PythagoreanTriples
    {
        public static IEnumerable<Tuple<int, int, int>> Gather()
            => from z in Sequence.Gather(1, i => i + 1)
               from x in Sequence.Gather(1, i => i + 1, i => i <= z)
               from y in Sequence.Gather(x, i => i + 1, i => i <= z)
               where x * x + y * y == z * z
               select Tuple.Create(x, y, z);
    }
}
