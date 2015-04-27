// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Narvalo.BenchmarkCommon;
    using Narvalo.Comparisons;

    public static class Program
    {
        public static void Main(string[] args)
        {
            Compare();
        }

        public static void Benchmark()
        {
            var processor = new BenchmarkProcessor {
                DiscoveryBindings = BindingFlags.Public | BindingFlags.Static,
                TestDuration = TimeSpan.FromMilliseconds(1000),
                WarmUpDuration = TimeSpan.FromMilliseconds(100),
            };

            var metricsByCategory = processor
                .Process(typeof(Program).Assembly)
                ////.Process(typeof(Narvalo.Int64EncoderBenchmarks))
                ////.Process(typeof(TempBenchmarks_))
                .GroupBy(_ => _.CategoryName);

            foreach (var metrics in metricsByCategory)
            {
                Console.WriteLine(metrics.Key);

                foreach (var metric in metrics.OrderBy(_ => _.CallsPerSecond))
                {
                    Console.WriteLine("- " + metric.ToString());
                }
            }
        }

        public static void Compare()
        {
            var processor = new BenchmarkComparisonProcessor {
                DiscoveryBindings = BindingFlags.Public | BindingFlags.Static,
            };

            ////IEnumerable<BenchmarkMetricCollection> metrics
            ////    = processor.Process(
            ////        typeof(RemoveDiacriticsComparison),
            ////        RemoveDiacriticsComparison.GenerateTestData());
            BenchmarkMetricCollection metrics = processor.Process(typeof(EmptyCtorComparison));

            foreach (var item in metrics)
            {
                Console.WriteLine(item.ToString());
            }
        }

        ////private static class TempBenchmarks_
        ////{
        ////    private static int? s_Sample = 1;

        ////    [Benchmark]
        ////    public static void Create_ReferenceType()
        ////    {
        ////        Maybe.Of("Some").Consume();
        ////    }

        ////    [Benchmark]
        ////    public static void Create_NullableValueType()
        ////    {
        ////        Maybe.Of(s_Sample).Consume();
        ////    }

        ////    [Benchmark]
        ////    public static void Create_ValueType()
        ////    {
        ////        Maybe.Of(1).Consume();
        ////    }
        ////}
    }
}
