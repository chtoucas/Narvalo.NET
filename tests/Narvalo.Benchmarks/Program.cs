// Copyright (c) Narvalo.Org. All rights reserved. See LICENSE.txt in the project root for license information.

namespace Narvalo
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Narvalo.Benchmarking;
    using Narvalo.Fx;
    using NodaTime;

    public static class Program
    {
        public static void Main(string[] args)
        {
            var processor = new BenchmarkProcessor {
                DiscoveryBindings = BindingFlags.Public | BindingFlags.Static,
                TestDuration = Duration.FromMilliseconds(1000),
                WarmUpDuration = Duration.FromMilliseconds(100),
            };

            var metricsByCategory = processor
                .Process(typeof(Program).Assembly)
                //.Process(typeof(Narvalo.Int64EncoderBenchmarks))
                //.Process(typeof(TempBenchmarks_))
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

        private static class TempBenchmarks_
        {
            //private static int? s_Sample = 1;

            [Benchmark]
            public static void Create_ReferenceType()
            {
                Maybe.Create("Some").Consume();
            }

            //[Benchmark]
            //public static void Create_NullableValueType()
            //{
            //    Maybe.Create(s_Sample).Consume();
            //}

            [Benchmark]
            public static void Create_ValueType()
            {
                Maybe.Create(1).Consume();
            }
        }
    }
}
