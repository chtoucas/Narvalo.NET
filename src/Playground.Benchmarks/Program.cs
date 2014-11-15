namespace Playground.Benchmarks
{
    using System;
    using Playground.Benchmarks.Comparisons;
    using Playground.Benchmarks.Internal;
    using Narvalo.Benchmarking;

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
