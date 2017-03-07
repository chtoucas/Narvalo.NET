// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Applicative;

    public static class Fibonacci
    {
        public static Func<int, int> Recurse()
            => Recursion.Fix<int, int>(iter => i => i > 1 ? iter(i - 1) + iter(i - 2) : i);

        public static IEnumerable<int> Iterate()
        {
            int i = 1;
            int j = 1;

            while (true)
            {
                int next = i + j;
                i = j;
                j = next;

                yield return next;
            }
        }

        public static IEnumerable<int> Gather()
            => Sequence.Gather<Tuple<int, int>, int>(
                seed: Tuple.Create(1, 1),
                iterator: _ => Tuple.Create(_.Item2, _.Item1 + _.Item2),
                resultSelector: _ => _.Item1);

        public static IEnumerable<int> Unfold()
            => Sequence.Unfold(
                seed: Tuple.Create(1, 1),
                generator: _ => Iteration.Create(_.Item1, Tuple.Create(_.Item2, _.Item1 + _.Item2)));
    }
}
