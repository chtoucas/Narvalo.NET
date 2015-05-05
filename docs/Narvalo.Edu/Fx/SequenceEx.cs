// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // WARNING: These sequences are infinite!
    public static class SequenceEx
    {
        public static IEnumerable<Tuple<int, int, int>> PythagoreanTriples()
        {
            return from z in Sequence.Gather(1, i => i + 1)
                   from x in Sequence.Gather(1, i => i + 1, i => i <= z)
                   from y in Sequence.Gather(x, i => i + 1, i => i <= z)
                   where x * x + y * y == z * z
                   select Tuple.Create(x, y, z);
        }

        private static IEnumerable<int> FibonacciViaIterator()
        {
            int i = 1;
            int j = 1;

            while (true)
            {
                int retval = i + j;
                i = j;
                j = retval;

                yield return retval;
            }
        }

        private static Func<int, int> FibonacciViaYCombinator()
        {
            return Recursion.Fix<int, int>(iter => i => i > 1 ? iter(i - 1) + iter(i - 2) : i);
        }

        private static IEnumerable<int> FibonacciViaAnamorphism()
        {
            return Sequence.Gather<Tuple<int, int>, int>(
                seed: Tuple.Create(1, 1),
                iterator: _ => Tuple.Create(_.Item2, _.Item1 + _.Item2),
                resultSelector: _ => _.Item1);
        }

        ////static IEnumerable<int> CreateViaAnamorphismAlternate()
        ////{
        ////    return Sequence.Create(
        ////        _ => Iteration.MayCreate(_.Item1, Tuple.Create(_.Item2, _.Item1 + _.Item2)),
        ////        Tuple.Create(1, 1));
        ////}
    }
}
