// Copyright (c) 2014, Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using Narvalo.Benchmarking;
    using Playground.Benchmarks;
    using Playground.Benchmarks.Comparisons;

    class Program
    {
        static void Main()
        {
            var metrics = BenchmarkComparisonProcessor
                .Create()
                .Process(typeof(RemoveDiacriticsComparison));

            var fmt = new BenchMetricConsoleFormatter();

            Console.WriteLine(metrics.ToString(fmt));
        }
    }
}
