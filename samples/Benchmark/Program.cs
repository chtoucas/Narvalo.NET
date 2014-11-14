using Narvalo.Benchmark;

namespace Benchmark
{
    using System;
    using Benchmark.Internal;

    class Program
    {
        static void Main()
        {
            var metrics = BenchComparisonProcessor
                .Create()
                .Process(typeof(RemoveDiacriticsComparison));

            var fmt = new BenchMetricConsoleFormatter();

            Console.WriteLine(metrics.ToString(fmt));
        }
    }
}
