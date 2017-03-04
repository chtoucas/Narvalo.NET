// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Edufun.Sequences
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Narvalo.Applicative;

    public static class Factorial
    {
        public static Func<int, int> Recurse()
            => YCombinator.Fix<int, int>(iter => i => i == 0 ? 1 : i * iter(i - 1));

        public static Func<int, int> Aggregate()
            => i => i == 0 ? 1 : Enumerable.Range(1, i).Aggregate((acc, j) => j * acc);

        public static int Recurse(int n) => n == 0 ? 1 : n * Recurse(n - 1);

        public static int TailRecurse(int n) => TailRecurse_(n, 1);

        private static int TailRecurse_(int n, int acc) => n == 0 ? acc : TailRecurse_(n - 1, n * acc);

        public static int Aggregate(int n)
            => n == 0 ? 1 : Enumerable.Range(1, n).Aggregate((acc, j) => j * acc);

        public static int Iterate(int n)
        {
            int acc = 1;
            for (var i = 1; i <= n; i++) { acc *= i; }
            return acc;
        }

        public static IEnumerable<int> Iterate()
        {
            int acc = 1;
            int i = 1;

            yield return acc;

            while (true)
            {
                acc *= i++;

                yield return acc;
            }
        }
    }
}
