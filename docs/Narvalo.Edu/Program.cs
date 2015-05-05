// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;

    using Narvalo.Fx;

    internal static class Program
    {
        public static void Main()
        {
            var q = SequenceEx.PythagoreanTriples();

            int step = 0;
            foreach (var t in q)
            {
                Console.WriteLine(t.Item1 + ", " + t.Item2 + ", " + t.Item3);
                step++;
                if (step > 100) { break; }
            }
        }
    }
}
