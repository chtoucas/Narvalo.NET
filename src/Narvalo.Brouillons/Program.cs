// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Narvalo.Diagnostics.Benchmarking;
    using Narvalo.Edu.Comparisons;

    internal static class Program
    {
        public static void Main()
        {
            Run();
        }

        public static void Run()
        {
        }

        public static void Benchmark()
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
