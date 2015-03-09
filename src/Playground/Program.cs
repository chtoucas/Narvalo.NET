// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Playground
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Playground.Benchmarks.Comparisons;
    using Narvalo.Diagnostics.Benchmarking;

    public static class Program
    {
        public static void Main()
        {
            var processor = new BenchmarkComparisonProcessor {
                DiscoveryBindings = BindingFlags.Public | BindingFlags.Static,
            };

            IEnumerable<BenchmarkMetricCollection> metrics
                = processor.Process(
                    typeof(RemoveDiacriticsComparison),
                    RemoveDiacriticsComparison.GenerateTestData());

            var fmt = new BenchMetricConsoleFormatter();

            foreach (var item in metrics)
            {
                Console.WriteLine(item.ToString(fmt));
            }
        }
    }
}
