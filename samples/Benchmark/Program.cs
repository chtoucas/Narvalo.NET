namespace Benchmark
{
    using System;
    using Benchmark.Internal;
    using Narvalo.Runtime.Benchmarking;

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
