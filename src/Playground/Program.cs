// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using System.Collections.Generic;

    using Narvalo.Benchmarking;
    using Playground.Benchmarks;
    using Playground.Benchmarks.Comparisons;

    public static class Program
    {
        public static void Main()
        {
            IEnumerable<BenchmarkMetricCollection> metrics
                = BenchmarkComparisonProcessor
                .Create()
                .Process(typeof(RemoveDiacriticsComparison), RemoveDiacriticsComparison.GenerateTestData());

            var fmt = new BenchMetricConsoleFormatter();

            foreach (var item in metrics)
            {
                Console.WriteLine(item.ToString(fmt));
            }
        }
    }
}
