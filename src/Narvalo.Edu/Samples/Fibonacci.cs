// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo.Edu.Samples
{
    using System;
    using System.Collections.Generic;
    using Narvalo.Edu.Collections.Recursion;

    public static class Fibonacci
    {
        static Func<int, int> CreateYCombinator()
        {
            return YCombinator.Fix<int, int>(iter => i => i > 1 ? iter(i - 1) + iter(i - 2) : i);
        }

        static IEnumerable<int> CreateViaAna0()
        {
            return Anamorphism.Ana(
                _ => Iteration.MayCreate(_.Item1, Tuple.Create(_.Item2, _.Item1 + _.Item2)),
                Tuple.Create(1, 1));
        }

        static IEnumerable<int> CreateViaAna1()
        {
            return Lenses.Ana<Tuple<int, int>, int>(
                iter: _ => Tuple.Create(_.Item2, _.Item1 + _.Item2),
                seed: Tuple.Create(1, 1),
                resultSelector: _ => _.Item1);
        }

    }
}
