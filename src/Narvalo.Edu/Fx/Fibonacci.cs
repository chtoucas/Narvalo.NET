// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Fx
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Narvalo.Fx;

    // WARNING: Do not run, these sequence never end!
    public static class Fibonacci
    {
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        static IEnumerable<int> CreateViaIterator()
        {
            int i = 1;
            int j = 1;

            yield return 1;

            while (true) {
                int result = i + j;
                i = j;
                j = result;

                yield return result;
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        static Func<int, int> CreateViaYCombinator()
        {
            return Recursion.Fix<int, int>(iter => i => i > 1 ? iter(i - 1) + iter(i - 2) : i);
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        static IEnumerable<int> CreateViaAna0()
        {
            return Sequence.Create(
                _ => Iteration.MayCreate(_.Item1, Tuple.Create(_.Item2, _.Item1 + _.Item2)),
                Tuple.Create(1, 1));
        }

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        static IEnumerable<int> CreateViaAna1()
        {
            return Sequence.Create<Tuple<int, int>, int>(
                iter: _ => Tuple.Create(_.Item2, _.Item1 + _.Item2),
                seed: Tuple.Create(1, 1),
                resultSelector: _ => _.Item1);
        }
    }
}
